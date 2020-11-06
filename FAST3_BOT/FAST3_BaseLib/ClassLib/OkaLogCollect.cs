using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAST3_BaseLib
{
    /// <summary>
    /// 日志类型（异常，信息，警告，测试）
    /// </summary>
    public enum LogType
    {
        Error,
        Msg,
        Warning,
        Debug
    }

    public static class OkaLogCollect
    {
        /// <summary>
        /// 日志路径，注意带上\
        /// </summary>
        public static string _logPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\InterfaceLog\\";
        /// <summary>
        /// 日志文件保存的根名称
        /// </summary>
        public static string _logRootName = "InterfaceLog";
        /// <summary>
        /// 日志文件保存间隔(为了性能考虑请尽量设置在10分钟以上，当设置时间低于5秒不能保证运行)
        /// </summary>
        public static TimeSpan _logTimeSpan = new TimeSpan(24, 0, 0);

        /// <summary>
        /// 日志写入级别 Debug->Wrong->Msg->Error 选择的级别越低，写入的东西越少，当选择Debug时，所有的日志都会被写入
        /// </summary>
        public static LogType _logLevel = LogType.Debug;

        /// <summary>
        /// 用于向外输出的信息集合（副本）
        /// </summary>
        public static List<object[]> OutPutMsg
        {
            get
            {
                return _OutPutMsg.GetRange(0, _OutPutMsg.Count);
            }
        }
        /// <summary>
        /// 向外输出信息集合
        /// </summary>
        private static List<object[]> _OutPutMsg = new List<object[]>();

        /// <summary>
        /// 日志文件
        /// </summary>
        private static FileInfo _logFile { get; set; }

        /// <summary>
        /// 最后写入日志时间
        /// </summary>
        private static readonly DateTime _lastLogInputTime = DateTime.Now;

        /// <summary>
        /// 日志写入（以String维度）
        /// </summary>
        /// <param name="logMsg"></param>
        /// <param name="logType"></param>
        public static void WriteLog(string logMsg, LogType logType)
        {
            try
            {
                List<string> logMsgList = new List<string>
                {
                    logMsg
                };
                WriteLog(logMsgList, logType); /*以集合的形式写入日志*/
            }
            catch (Exception ex)
            {
                //当写入日志出现异常会进行强制抛出
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 日志的写入（以List维度）
        /// </summary>
        /// <param name="logMsgList"></param>
        /// <param name="logType"></param>
        public static void WriteLog(List<string> logMsgList, LogType logType)
        {
            try
            {
                if (CheckLogLevel(logType))
                {
                    if (_logFile == null)
                    {
                        //需要更换新的文件
                        if (!Directory.Exists(_logPath))
                        {
                            //创建文件夹路径
                            Directory.CreateDirectory(_logPath);
                        }

                        _logFile = new FileInfo(_logPath + _logRootName + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt");
                    }
                    lock (_logFile)
                    {
                        //写日志
                        if (CheckLogTime())
                        {
                            //需要更换新的文件
                            if (!Directory.Exists(_logPath))
                            {
                                //创建文件夹路径
                                Directory.CreateDirectory(_logPath);
                            }

                            _logFile = new FileInfo(_logPath + _logRootName + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt");
                        }
                        using (StreamWriter sw = _logFile.AppendText())
                        {
                            foreach (var item in logMsgList)
                            {
                                sw.WriteLine("[" + GetLevelString(logType) + "]  " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " : " + item);
                            }
                            sw.Close();
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                //当写入日志出现异常会进行强制抛出
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 日志的写入(异步)
        /// </summary>
        /// <param name="logMsg">日志内容</param>
        /// <param name="logType">日志类型</param>
        public static async void WriteLogAsync(List<string> logMsg, LogType logType)
        {
            try
            {
                if (CheckLogLevel(logType))
                {
                    await AnsyWriteFile(logMsg, logType);
                }
            }
            catch (Exception ex)
            {
                //当写入日志出现异常会进行强制抛出
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 用于等待写入日志
        /// </summary>
        /// <param name="logMsg">日志内容</param>
        /// <param name="logType">日志级别</param>
        private static Task AnsyWriteFile(List<string> logMsg, LogType logType)
        {
            //写日志
            Task result = null;
            if (_logFile == null)
            {
                //需要更换新的文件
                if (!Directory.Exists(_logPath))
                {
                    //创建文件夹路径
                    Directory.CreateDirectory(_logPath);
                }

                _logFile = new FileInfo(_logPath + _logRootName + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt");
            }

            lock (_logFile)
            {
                //写日志
                if (CheckLogTime())
                {
                    //需要更换新的文件
                    if (!Directory.Exists(_logPath))
                    {
                        //创建文件夹路径
                        Directory.CreateDirectory(_logPath);
                    }

                    _logFile = new FileInfo(_logPath + _logRootName + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt");
                }
                using (StreamWriter sw = _logFile.AppendText())
                {
                    foreach (var item in logMsg)
                    {
                        result = sw.WriteLineAsync("[" + GetLevelString(logType) + "]  " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " : " + item);
                    }
                    sw.Close();
                }

            }
            return result;
        }

        /// <summary>
        /// 向输出文本集合内写入。同时该方法也验证信息集合量数据量。
        /// </summary>
        public static void WriteOutPutMsg(string logMsg, LogType logType)
        {
            List<string> logMsgList = new List<string>
            {
                logMsg
            };
            WriteOutPutMsg(logMsgList, logType);
        }

        /// <summary>
        /// 向输出文本集合内写入。同时该方法也验证信息集合量数据量。
        /// </summary>
        public static void WriteOutPutMsg(List<string> msg, LogType logType)
        {
            //在清理数据同时，防止之前某个特定时常的数据没有外传，暂时保留5分钟之内的数据。
            //现在的数据比率是5分钟远不足99999条信息（这个数值比率取决于接口数量与扫描频率）
            //如果数据比率变大，请修改本方法。
            try
            {
                lock (_OutPutMsg)
                {
                    if (_OutPutMsg.Count > 100)
                    {
                        string dateOldKey =
                            DateTime.Now.AddMinutes(-3).ToString("yyyyMMddHHmm")
                            + "0000";

                        _OutPutMsg =
                            _OutPutMsg.Where(
                                o => o[0].ToString().CompareTo(dateOldKey) > 0
                                ).ToList();
                    }

                    foreach (var item in msg)
                    {
                        string threadName = System.Threading.Thread.CurrentThread.Name; //将线程名称记录（接口INTERFACE特殊需求）

                        _OutPutMsg.Add(
                            new object[] {
                                DateTime.Now.ToString("yyyyMMddHHmm")
                                + (_OutPutMsg.Count + 1).ToString().PadLeft(4, '0'), //序号
                                "{"
                                +  threadName
                                + "}["
                                + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                                + " " + GetLevelString(logType) + "] "
                                + item //信息
                            });
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 衡量写入等级，当请求等级高于等于写入等级时，返回TRUE
        /// </summary>
        /// <param name="logType">请求等级</param>
        /// <returns>比较结果</returns>
        private static bool CheckLogLevel(LogType logType)
        {
            bool result = false;

            if (Convert.ToInt32(_logLevel) >= Convert.ToInt32(logType))
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// 检查写入时间，当写入时间超过一定时间后，需要自动保存
        /// </summary>
        /// <returns>是否需要自动保存</returns>
        private static bool CheckLogTime()
        {
            bool result = false;
            if (DateTime.Now - _lastLogInputTime > _logTimeSpan)
            {
                result = true;
            }
            return result;
        }

        #region 获取日志类型
        /// <summary>
        /// 获取日志类型描述
        /// </summary>
        /// <param name="logType">名称</param>
        /// <returns>类型名字</returns>
        private static string GetLevelString(LogType logType)
        {
            switch (logType)
            {
                case LogType.Debug:
                    return "测试";

                case LogType.Error:
                    return "错误";

                case LogType.Msg:
                    return "信息";

                case LogType.Warning:
                    return "警告";

                default:
                    return "其他";
            }
        }
        #endregion
    }
}
