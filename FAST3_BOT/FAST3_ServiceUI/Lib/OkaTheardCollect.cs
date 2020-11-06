using FAST3_BaseLib;
using System.Threading;

namespace FAST3_ServiceUI
{
    /// <summary>
    /// 线程汇总类
    /// </summary>
    public static class OkaTheardCollect
    {
        /// <summary>
        /// 入库任务线程
        /// </summary>
        private static Thread ThreadWithTaskIn { get; set; }
        private static Thread ThreadWithTaskOut { get; set; }

        /*备用...*/

        /// <summary>
        /// 根据Key值获取对应线程
        /// </summary>
        /// <param name="threadKey">Key</param>
        /// <param name="threadMission">线程</param>
        /// <returns></returns>
        public static Thread GetThreadByKey(string threadKey, out IThreadMission threadMission)
        {
            threadMission = null;
            Thread thread = GetThreadByKey(threadKey);
            switch (threadKey)
            {
                case "ThreadWithTaskIn":
                    threadMission = new TaskInThread();
                    break;
                case "ThreadWithTaskOut":
                    threadMission = new TaskOutThread();
                    break;
                default:
                    return null;
            }

            return thread;
        }

        public static Thread GetThreadByKey(string threadKey)
        {
            switch (threadKey)
            {
                case "ThreadWithTaskIn":
                    return ThreadWithTaskIn;
                case "ThreadWithTaskOut":
                    return ThreadWithTaskOut;
                default:
                    return null;
            }
        }

        /// <summary>
        /// 线程的绑定
        /// </summary>
        /// <param name="threadKey">Key</param>
        /// <param name="thread">线程</param>
        public static void SetThreadByKey(string threadKey, Thread thread)
        {
            switch (threadKey)
            {
                case "ThreadWithTaskIn":
                    if (ThreadWithTaskIn == null)
                        ThreadWithTaskIn = thread;
                    break;
                case "ThreadWithTaskOut":
                    if (ThreadWithTaskOut == null)
                        ThreadWithTaskOut = thread;
                    break;
                default:
                    break;
            }
        }
    }
}
