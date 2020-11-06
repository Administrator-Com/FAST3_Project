using System;
using System.Threading;

namespace FAST3_BaseLib
{
    /// <summary>
    /// 线程管理类
    /// </summary>
    public static class OkaThreadClass
    {
        #region 线程启动
        /// <summary>
        /// 开启对应线程
        /// </summary>
        /// <param name="thread">线程</param>
        /// <param name="threadName">告知线程名称，线程名称需要唯一</param>
        /// <param name="threadMission">各个线程的业务方法</param>
        public static Thread ThreadStart(Thread thread, string threadName, IThreadMission threadMission)
        {
            try
            {
                if (thread == null || thread.ThreadState != ThreadState.Running && thread.ThreadState != ThreadState.WaitSleepJoin)
                {
                    thread = new Thread(new ParameterizedThreadStart(OkaMissionClass.TableListiner))
                    {
                        Name = threadName
                    };
                    object[] obj = new object[] { thread.Name, threadMission };
                    /*设置线程开关*/
                    OkaMissionClass.SetMissionSwitch(thread, true);
                    /*启动线程*/
                    thread.Start(obj);
                }
            }
            catch (Exception ex)
            {
                OkaLogCollect.WriteLog("[" + ex.StackTrace.ToString() + "]:" + ex.Message, LogType.Error);
            }
            return thread;
        }
        #endregion

        #region 线程终止
        /// <summary>
        /// 终止线程
        /// </summary>
        /// <param name="thread">线程</param>
        public static void TheadStop(Thread thread)
        {
            try
            {
                if (thread != null)
                {
                    OkaMissionClass.SetMissionSwitch(thread, false);
                }
            }
            catch (Exception ex)
            {
                OkaLogCollect.WriteLog("[" + ex.StackTrace.ToString() + "]:" + ex.Message, LogType.Error);
            }
        }
        #endregion
    }
}
