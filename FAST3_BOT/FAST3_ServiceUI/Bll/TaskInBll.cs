using FAST3_Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAST3_ServiceUI
{
    public class TaskInBll: RepositoryFactory<V_WCS_TRK>
    {
        /// <summary>
        /// 获取任务信息
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataTable GetTaskByIn(string strWhere)
        {
            string strSql = "";
            strSql = string.Format(@"SELECT V1.TRK_ID,V1.CONT_NO FROM V_WCS_TRK V1 WHERE 1=1{0}", strWhere);
            return Repository().FindTableBySql(strSql);
        }
    }
}
