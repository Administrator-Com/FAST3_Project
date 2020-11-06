using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FAST3_BaseLib
{
    public static class OkaMissionClass
    {
        /// <summary>
        /// 线程开关字典《线程名称, 是否开启 true:开启 false:关闭》
        /// </summary>
        private static Dictionary<string, bool> _threadSwitchDic = new Dictionary<string, bool>();

        /// <summary>
        /// 线程Sleep时间
        /// object[]{ 间隔（毫秒），开始时间（整点），结束时间（整点） } 
        /// eg: new boject[]{ 1200, 89, 18  }
        /// </summary>
        public static Dictionary<string, object[]> _threadInterval = new Dictionary<string, object[]>();

        #region 线程监听
        /// <summary>
        /// 表数据监听
        /// </summary>
        /// <param name="paramter">参数列表 固定有2个 string线程名称 IThreadMission对象</param>
        public static void TableListiner(object paramter)
        {
            object[] paramters = paramter as object[];

            string threadName = paramters[0] as string;
            IThreadMission threadMission = paramters[1] as IThreadMission;
            if (!string.IsNullOrEmpty(threadName) && threadMission != null)
            {
                try
                {
                    /*防止脏读*/
                    lock (_threadSwitchDic)
                    {
                        /*开启业务场景的递归*/
                        threadMission.StartTask(threadName);
                    }
                }
                catch (Exception ex)
                {
                    OkaLogCollect.WriteLog("[" + ex.StackTrace.ToString() + "]:" + ex.Message, LogType.Error);
                }
            }
        }
        #endregion

        #region 线程处理
        /// <summary>
        /// 设置线程任务开关
        /// </summary>
        /// <param name="thread">线程</param>
        /// <param name="missionSwitch">开关 true:任务开启</param>
        public static void SetMissionSwitch(Thread thread, bool missionSwitch)
        {
            lock (_threadSwitchDic)
            {
                if (_threadSwitchDic.Keys.Any(k => k == thread.Name))
                {
                    _threadSwitchDic[thread.Name] = missionSwitch;
                }
                else
                {
                    _threadSwitchDic.Add(thread.Name, missionSwitch);
                }
            }
        }

        /// <summary>
        /// 检查线程任务开关
        /// </summary>
        /// <param name="thread">线程</param>
        /// <returns>任务开关 true:任务开启</returns>
        public static bool CheckMissionSwitch(Thread thread)
        {
            bool result = false;
            lock (_threadSwitchDic)
            {
                if (thread != null)
                {
                    if (_threadSwitchDic.Keys.Any(k => k == thread.Name))
                    {
                        result = _threadSwitchDic[thread.Name];
                    }
                }
            }

            return result;
        }
        #endregion
    }
}
