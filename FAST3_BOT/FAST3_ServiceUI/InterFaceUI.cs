using FAST3_BaseLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace FAST3_ServiceUI
{
    public partial class InterFaceUI : Form
    {
        DateTime _currDateTime = DateTime.Now;
        private int _curHH = 0;
        private int _curMM = 0;
        private int _curSS = 0;

        //数据列表中，按钮显示值
        private string _strStartText = "开启";
        private string _strStopText = "停止";
        private string _MsgIndexKey { get; set; }
        /// <summary>
        /// 映射名称
        /// </summary>
        private Dictionary<string, string> _ThreedChineseName = new Dictionary<string, string>();

        #region 构造函数InterFaceUI
        public InterFaceUI()
        {
            //验证进程唯一性
            List<Process> pc =
                Process.GetProcesses().Where(
                    p => p.ProcessName == Process.GetCurrentProcess().ProcessName
                ).ToList();//获取当前进程数组。  

            if (pc.Count > 1)
            {
                MessageBox.Show("接口系统已运行!", "提示", MessageBoxButtons.OK);
                this.Close();
                return;
            }

            InitializeComponent(); /*初始化组件*/

            /*线程校验timer预设时间*/
            timerCheck.Interval = 6000;
            timerCheck.Start();

            /*线程列表初始化*/
            DataTable dtInterface = new DataTable();
            dtInterface.Columns.Add("InterfaceName", typeof(string));
            dtInterface.Columns.Add("InterfaceKey", typeof(string));
            dtInterface.Columns.Add("tabPageBelone", typeof(string));

            dtInterface.Rows.Add(new object[] { "上传WMS入库任务接口", "ThreadWithTaskIn", "tabPageUpLoad" });
            dtInterface.Rows.Add(new object[] { "上传WMS出库任务接口", "ThreadWithTaskOut", "tabPageUpLoad" });

            //将中文名称映射
            comboBoxMsgInterface.Items.Add("所有接口");
            comboBoxMsgInterface.Items.Add("主程序");
            foreach (DataRow item in dtInterface.Rows)
            {
                _ThreedChineseName.Add(item["InterfaceKey"].ToString(), item["InterfaceName"].ToString());
                comboBoxMsgInterface.Items.Add(item["InterfaceName"].ToString());
            }
            comboBoxMsgInterface.SelectedIndex = 0;
            /*初始化线程列表*/
            InitData(dataGridViewInterface, dtInterface);
            /*程序启动时【全部停止】按钮不可用*/
            toolStripBtnStop.Enabled = false;
            /*最小化菜单相关*/
            this.ShowInTaskbar = true;
            notifyIconMini.Visible = false;
        }
        #endregion

        #region InitData  动态生成【停止】、【开启】按钮
        /// <summary>
        /// 初始化grid数据列表
        /// </summary>
        /// <param name="dg"></param>
        /// <param name="dt"></param>
        public void InitData(DataGridView dg, DataTable dt)
        {
            try
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dg.Rows.Add();
                    DataGridViewCell cellKey = dg.Rows[i].Cells[0];//key
                    cellKey.Value = dt.Rows[i]["InterfaceKey"];

                    DataGridViewCell cellName = dg.Rows[i].Cells[1];//name
                    cellName.Value = dt.Rows[i]["InterfaceName"];

                    DataGridViewCell cellState = dg.Rows[i].Cells[2];//state
                    cellState.Value = _strStopText;

                    DataGridViewButtonCell cellButton = dg.Rows[i].Cells[3] as DataGridViewButtonCell;//操作按钮
                    cellButton.Value = _strStartText;

                    DataGridViewCell cellBelones = dg.Rows[i].Cells[4];//所属
                    cellBelones.Value = dt.Rows[i]["tabPageBelone"];

                    if (cellBelones.Value.ToString() != tabControl.TabPages[0].Name)
                    {
                        dg.Rows[i].Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region 线程监控
        /// <summary>
        /// 线程监视器
        /// </summary>
        private void TimerCheck_Tick(object sender, EventArgs e)
        {
            /*获取数据库信息（测试结果：成功）
                TaskInThread taskInThread = new TaskInThread();
                DataTable dtTaskIn = new DataTable();
                dtTaskIn = taskInThread.GetTaskByIn(" ");
            */
            lock (this)
            {
                SetCurrentTime();
                SetThreadState();
                //textBoxMsg.AppendText(_currDateTime + ":主程序成功启动!" + "\r\n");
                //GetLogMsg(); /*日志记录*/
            }
        }

        /// <summary>
        /// 获取并设置线程状态
        /// </summary>
        /// <param name="rowIndex">对应展示的行号</param>
        private void SetThreadState()
        {
            string threadKey = string.Empty; /*线程主键*/
            Thread thread = null;

            try
            {
                for (int index = 0; index < dataGridViewInterface.Rows.Count; index++)
                {
                    threadKey = dataGridViewInterface.Rows[index].Cells[0].Value.ToString();
                    thread = OkaTheardCollect.GetThreadByKey(threadKey, out IThreadMission threadMission);

                    /*防止 线程Listner无限递归没有释放内存产生的问题*/
                    if (thread == null)
                    {
                        textBoxMsg.AppendText(_currDateTime + ":线程[" + threadKey + "]未启动!" + "\r\n");
                        continue;
                    }

                    if (OkaMissionClass.CheckMissionSwitch(thread))
                    {
                        textBoxMsg.AppendText(_currDateTime + ":线程[" + threadKey + "]旧状态为[" + thread.ThreadState + "]!" + "\r\n");
                        thread = OkaThreadClass.ThreadStart(thread, threadKey, threadMission);
                        textBoxMsg.AppendText(_currDateTime + ":线程[" + threadKey + "]新状态为[" + thread.ThreadState + "]!" + "\r\n");
                        //OkaTheardCollect.SetThreadByKey(threadKey, thread); /*线程绑定（方案待定）*/
                    }
                    else
                    {
                        textBoxMsg.AppendText(_currDateTime + ":线程[" + threadKey + "]状态[" + thread.ThreadState + "]!" + "\r\n");
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /*通过按钮【开启】/【关闭】线程服务*/
        private void DataGridViewInterface_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            CellClickMethod(sender, e);
        }

        public void CellClickMethod(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (sender == null || e == null)
                {
                    return;
                }
                DataGridView dgv = (DataGridView)sender;
                if (e.RowIndex >= 0 && e.ColumnIndex == 3)
                {
                    /*校验：当且仅当操作栏中的按钮被点击时触发*/
                    string stateValue = dgv.Rows[e.RowIndex].Cells[2].Value.ToString(); /*控件状态*/
                    if (stateValue == _strStartText || stateValue == _strStopText)
                    {
                        /*获取要操作的线程的Key与Name*/
                        string threadKey = dgv.Rows[e.RowIndex].Cells[0].Value.ToString();
                        string threadName = dgv.Rows[e.RowIndex].Cells[1].Value.ToString();

                        DataGridViewButtonCell btnState = dgv.Rows[e.RowIndex].Cells[3] as DataGridViewButtonCell;

                        /*锁定控件的状态（只有当按钮的状态稳定后（开始/结束）才能操作，处于中间状态的（启动中.../结束中...）按钮无法操作）*/
                        dgv.Rows[e.RowIndex].Cells[2].Value = btnState.Value + "中";

                        Thread curThread = OkaTheardCollect.GetThreadByKey(threadKey, out IThreadMission threadMission);

                        if (btnState.Value.ToString() == _strStartText)
                        {
                            /*线程为关闭状态,设置线程开启*/
                            textBoxMsg.AppendText(_currDateTime + ":线程[" + threadName + "]启动!" + "\r\n");

                            /*启动线程*/
                            curThread = OkaThreadClass.ThreadStart(curThread, threadKey, threadMission);
                            /*绑定线程*/
                            OkaTheardCollect.SetThreadByKey(threadKey, curThread);
                            /*成功启动则更新操作按钮的状态*/
                            dgv.Rows[e.RowIndex].Cells[2].Value = _strStartText;
                            dgv.Rows[e.RowIndex].Cells[3].Value = _strStopText;
                        }
                        else
                        {
                            /*线程为开启状态,设置线程关闭*/
                            OkaThreadClass.TheadStop(curThread);
                            textBoxMsg.AppendText(_currDateTime + ":线程[" + threadName + "]关闭!" + "\r\n");
                            //OkaLogCollect.WriteOutPutMsg("线程[" + threadName + "]关闭", LogType.Msg);
                            /*成功关闭*/
                            dgv.Rows[e.RowIndex].Cells[2].Value = _strStopText;
                            dgv.Rows[e.RowIndex].Cells[3].Value = _strStartText;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                OkaLogCollect.WriteLog("[" + ex.StackTrace.ToString() + "]:" + ex.Message, LogType.Error);
            }
        }
        #endregion

        #region 获取当前时间 SetCurrentTime
        /// <summary>
        /// 获取当前系统时间
        /// </summary>
        private void SetCurrentTime()
        {
            _currDateTime = DateTime.Now;
            _curHH = _currDateTime.Hour;
            _curMM = _currDateTime.Minute;
            _curSS = _currDateTime.Second;
        }
        #endregion

        #region 窗体最小化
        /// <summary>
        /// 将界面最小化至菜单栏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InterFaceUI_SizeChanged(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                //隐藏任务栏区图标
                this.ShowInTaskbar = false;
                //图标显示在托盘区
                notifyIconMini.Visible = true;
                notifyIconMini.ShowBalloonTip(2000);
            }
        }
        #endregion

        #region 获取日志相关输出信息GetLogMsg
        /// <summary>
        /// 获取日志相关输出信息
        /// </summary>
        private void GetLogMsg()
        {
            try
            {
                if (textBoxMsg.Text.Length > 50)
                {
                    //清理过量文本
                    //string leftText = textBoxMsg.Text.Substring(textBoxMsg.Text.LastIndexOf("\r\n"));
                    string leftText = textBoxMsg.Text;
                    textBoxMsg.Text = "";
                    //textBoxMsg.AppendText(leftText);
                }

                List<object[]> logMsg = OkaLogCollect.OutPutMsg;
                if (logMsg == null)
                {
                    logMsg = new List<object[]>();
                }

                if (!string.IsNullOrEmpty(_MsgIndexKey))
                {
                    logMsg =
                        logMsg.Where(
                            l => l[0].ToString().CompareTo(_MsgIndexKey) > 0).ToList();
                }

                if (comboBoxMsgInterface.SelectedIndex > 0)
                {
                    //获取指定接口的信息
                    string selectInterfaceValue = comboBoxMsgInterface.Text;

                    KeyValuePair<string, string> selectInterfaceObj =
                        _ThreedChineseName.Where(
                            t => t.Value == selectInterfaceValue
                        ).FirstOrDefault();

                    logMsg =
                        logMsg.Where(
                            l => l[1].ToString().IndexOf("{" + selectInterfaceObj.Key + "}") >= 0
                            ).ToList();
                }

                //写入信息
                foreach (var item in logMsg)
                {
                    //映射中文名称
                    int itemIndexLeft = item[1].ToString().IndexOf('{');
                    int itemIndexRight = item[1].ToString().IndexOf('}');

                    itemIndexLeft = (itemIndexLeft < 0 ? 0 : itemIndexLeft);
                    itemIndexRight = (itemIndexRight - itemIndexLeft < 0 ? itemIndexLeft : itemIndexRight);

                    string getThreadName = item[1].ToString().Substring(itemIndexLeft + 1, itemIndexRight - itemIndexLeft - 1);
                    string getThreadChineseName = "";

                    if (_ThreedChineseName.ContainsKey(getThreadName))
                    {
                        getThreadChineseName = _ThreedChineseName[getThreadName];
                    }

                    getThreadChineseName =
                        (string.IsNullOrEmpty(getThreadChineseName)
                        ? "主程序"
                        : getThreadChineseName);

                    textBoxMsg.AppendText("\r\n" + item[1].ToString().Replace("{" + getThreadName + "}", "{" + getThreadChineseName + "}"));
                    _MsgIndexKey = item[0].ToString();
                }

                //将光标移至末端
                textBoxMsg.ScrollToCaret();
            }
            catch (Exception ex)
            {
                OkaLogCollect.WriteLog(
                   "[" + ex.StackTrace.ToString() + "]:" + ex.Message,
                   LogType.Error);
            }

        }
        #endregion
    }
}
