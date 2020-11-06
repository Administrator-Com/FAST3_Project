using System.Collections.Generic;

namespace FAST3_BaseLib
{
    /// <summary>
    /// 请求返回标准对象
    /// </summary>
    public class FBResult
    {
        public string RequestPK { get; set; }
        public string ResultCode { get; set; }
        public string ResultMsg { get; set; }

        public FBResult()
        {
            this.RequestPK = "";
            this.ResultCode = "";
            this.ResultMsg = "";
        }
    }

    public class WMSFeedBackResult
    {
        public List<WMSResult> BODY { get; set; }
        public WMSFeedBackResult()
        {
            BODY = new List<WMSResult>();
        }
    }

    /// <summary>
    /// WMS接口用返回标准（其他上位系统也通用）
    /// </summary>
    public class WMSResult
    {
        /// <summary>
        /// TRUE: 操作成功 FALSE:操作失败
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 当Success = False需要有消息反馈
        /// </summary>
        public string Message { get; set; }

        public string Result { get; set; }

        public WMSResult()
        {
            this.Success = true;
            this.Message = "成功";
            this.Result = "";
        }
    }
}
