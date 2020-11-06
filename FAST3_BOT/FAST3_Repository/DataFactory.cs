using FAST3_DataAccess;
using System.Configuration;

namespace FAST3_Repository
{
    /// <summary>
    /// 操作数据库工厂
    /// </summary>
    public class DataFactory
    {
        /// <summary>
        /// 当前数据库类型
        /// </summary>
        private static readonly string DbType = ConfigurationManager.AppSettings["ComponentDbType"].ToString().Trim();
        /// <summary>
        /// 获取指定的数据库连接
        /// </summary>
        /// <param name="connString"></param>
        /// <returns></returns>
        public static IDatabase DataBase(string connString)
        {
            return new Database(connString);
        }

        /// <summary>
        /// 根据配置文件获取指定的数据库连接
        /// SQLServer数据库与Oracle数据分别使用不同的连接方式
        /// </summary>
        /// <returns></returns>
        public static IDatabase DataBase()
        {
            switch (DbType)
            {
                case "SqlServer":
                    return DataBase("FAST3_Sqlserver");
                default:
                    return null;
            }
        }
    }
}
