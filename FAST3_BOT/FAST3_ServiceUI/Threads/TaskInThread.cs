using FAST3_BaseLib;

namespace FAST3_ServiceUI
{
    /// <summary>
    /// 入库任务的线程
    /// </summary>
    public class TaskInThread : IThreadMission
    {
        public object[] GetParam() => null; /*不需要返回参数*/

        /// <summary>
        /// 用于实现业务(具体业务在对应Bll文件中实现)
        /// </summary>
        /// <param name="pro"></param>
        public void StartTask(string pro)
        {
            TaskInBll taskInBll = new TaskInBll();
            taskInBll.GetTaskByIn("");
            //OkaLogCollect.WriteOutPutMsg("线程调用", LogType.Msg);
            OkaLogCollect.WriteLog("TaskInThread被调用", LogType.Msg);
        }
    }
}
