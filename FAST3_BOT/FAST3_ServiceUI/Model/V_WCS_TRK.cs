using System.ComponentModel;

namespace FAST3_ServiceUI
{
    /// <summary>
    /// 设备任务信息
    /// </summary>
    [Description("库存信息")]
    public class V_WCS_TRK
    {
        [DisplayName("任务ID")]
        public int TASK_ID { get; set; }
        [DisplayName("托盘号")]
        public string CONT_NO { get; set; }
    }
}
