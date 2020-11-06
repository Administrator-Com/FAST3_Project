using FAST3_BaseLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAST3_ServiceUI
{
    /// <summary>
    /// 入库任务的线程
    /// </summary>
    public class TaskOutThread : IThreadMission
    {
        public object[] GetParam() => null; /*不需要返回参数*/

        /// <summary>
        /// 用于实现业务(具体业务在对应Bll文件中实现)
        /// </summary>
        /// <param name="pro"></param>
        public void StartTask(string pro)
        {
            OkaLogCollect.WriteLog("TaskOutThread被调用", LogType.Msg);
        }
    }
}
