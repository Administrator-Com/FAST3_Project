using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FAST3_ServiceUI
{
    public class CommonMethod
    {
        /// <summary>
        /// POST访问目标RestFul接口
        /// </summary>
        /// <param name="strUri">URL地址</param>
        /// <param name="strContent">JSON</param>
        /// <returns></returns>
        public string PostRequest(string strUri, string strContent)
        {
            string dataJson = string.Empty;
            try
            {
                //调用上位接口将数据上传并接收反馈信息（JSON）

                Uri address = new Uri(strUri);

                HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;

                request.Method = "POST";
                request.ContentType = "application/json;charset=UTF-8";
                /*StreamWriter类允许直接将字符和字符串写入文件，一般不针对二进制数据*/
                using (StreamWriter dataStream = new StreamWriter(request.GetRequestStream()))
                {
                    dataStream.Write(strContent);
                    dataStream.Close();
                }

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                dataJson = GetResponseString(response);
                return dataJson;
            }
            catch (Exception ex)
            {
                return "{\"Success\":false,\"Message\":" + "\"" + ex.Message.ToString() + "\"" + ",\"Result\":null}";
            }
        }

        #region 从目标API获取反馈信息
        /// <summary>
        /// 获取反馈信息
        /// </summary>
        /// <param name="respone"></param>
        /// <returns></returns>
        private static string GetResponseString(HttpWebResponse respone)
        {
            string fbJson = null;

            using (StreamReader reader = new StreamReader(respone.GetResponseStream(), Encoding.GetEncoding("UTF-8")))
            {
                fbJson = reader.ReadToEnd();
            }
            return fbJson;
        } 
        #endregion
    }
}
