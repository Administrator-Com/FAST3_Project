using System;
using System.Configuration;
using System.Data;
using System.Data.Common;

namespace FAST3_DataAccess
{
    /// <summary>
    /// 数据库操作基类
    /// </summary>
    public class DbHelper
    {
        #region 自带属性
        /// <summary>
        /// 连接字符串
        /// </summary>
        public static string ConnectionString { get; set; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public static DatabaseType DbType { get; set; }

        /// <summary>
        /// 数据库连接方式（直连or非直连）
        /// </summary>
        public static string ConnectionWay { get; set; }

        /// <summary>
        /// 数据库命名参数符号
        /// </summary>
        public static string DbParmChar { get; set; }
        #endregion

        #region DbHelper构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connstring">连接字符串</param>
        public DbHelper(string connstring)
        {
            /*此处根据不同数据库进行拆分*/
            if (connstring == "FAST3_Sqlserver")
            {
                ConnectionString = ConfigurationManager.ConnectionStrings[connstring].ConnectionString;
            }
            else
            {
                /*如果是oracle数据则使用拼接的方式*/
                ConnectionWay = ConfigurationManager.AppSettings["ConnectionWay"].ToString().Trim();
                ConfigFile config = new ConfigFile(ConnectionWay);
                ConnectionString = config.GetConnectionString();
            }

            DatabaseTypeEnumParse(ConfigurationManager.ConnectionStrings[connstring].ProviderName);
            DbParmChar = DbFactory.CreateDbParmCharacter();
        }
        #endregion

        #region 执行SQL语句并返回受影响的行数ExecuteNonQuery
        public static int ExecuteNonQuery(CommandType cmdType, string cmdText)
        {
            int num = 0;
            DbCommand cmd = DbFactory.CreateDbCommand();
            try
            {
                using (DbConnection conn = DbFactory.CreateDbConnection(ConnectionString))
                {
                    PrepareCommand(cmd, conn, null, cmdType, cmdText, null);
                    num = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                }
            }
            catch (Exception ex)
            {
                num = -1;
                throw ex;
            }
            finally
            {
                cmd.Dispose();
            }
            return num;
        }

        /// <summary>
        /// 执行 SQL 语句，并返回受影响的行数。
        /// </summary>
        /// <param name="cmdType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="cmdText">存储过程名称或者T-SQL命令行</param>
        /// <param name="parameters">执行命令所需的sql语句对应参数</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(CommandType cmdType, string cmdText, params DbParameter[] parameters)
        {
            int num = 0;
            DbCommand cmd = DbFactory.CreateDbCommand();
            try
            {
                using (DbConnection conn = DbFactory.CreateDbConnection(ConnectionString))
                {
                    PrepareCommand(cmd, conn, null, cmdType, cmdText, parameters);
                    num = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                }
            }
            catch (Exception ex)
            {
                num = -1;
                throw ex;
            }
            finally
            {
                cmd.Dispose();
            }
            return num;
        }

        /// <summary>
        /// 执行SQL语句，并返回受影响的行数
        /// </summary>
        /// <param name="isOpenTrans">事务对象</param>
        /// <param name="cmdType">执行命令类型</param>
        /// <param name="cmdText">存储过程名称或T-SQL语句</param>
        /// <param name="parameters">执行命令所需的sql语句对应参数</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(DbTransaction isOpenTrans, CommandType cmdType, string cmdText, params DbParameter[] parameters)
        {
            int num = 0;
            DbCommand cmd = DbFactory.CreateDbCommand();
            try
            {
                if (isOpenTrans == null || isOpenTrans.Connection == null)
                {
                    using (DbConnection conn = DbFactory.CreateDbConnection(ConnectionString))
                    {
                        PrepareCommand(cmd, conn, isOpenTrans, cmdType, cmdText, parameters);
                        num = cmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    PrepareCommand(cmd, isOpenTrans.Connection, isOpenTrans, cmdType, cmdText, parameters);
                    num = cmd.ExecuteNonQuery();
                }
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                num = -1;
                throw ex;
            }
            return num;
        }
        #endregion

        #region 执行有结果集返回的数据库操作命令，并返回SqlDataReader对象
        /// <summary>
        /// 使用提供的参数，执行有结果集返回的数据库操作命令，并返回SqlDataReader对象
        /// </summary>
        /// <param name="cmdType">执行命令的类型</param>
        /// <param name="cmdText">存储过程名称或者T-SQL命令行</param>
        /// <returns></returns>
        public static IDataReader ExecuteReader(CommandType cmdType, string cmdText)
        {
            DbCommand cmd = DbFactory.CreateDbCommand();
            DbConnection conn = DbFactory.CreateDbConnection(ConnectionString);
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, null);
                IDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return rdr;
            }
            catch (Exception ex)
            {
                conn.Close();
                cmd.Dispose();
                throw ex;
            }
        }

        /// <summary>
        /// 使用提供的参数，执行有结果集返回的数据库操作命令、并返回SqlDataReader对象
        /// </summary>
        /// <param name="commandType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="commandText">存储过程名称或者T-SQL命令行<</param>
        /// <param name="parameters">执行命令所需的sql语句对应参数</param>
        /// <returns>返回SqlDataReader对象</returns>
        public static IDataReader ExecuteReader(CommandType cmdType, string cmdText, params DbParameter[] parameters)
        {
            DbCommand cmd = DbFactory.CreateDbCommand();
            DbConnection conn = DbFactory.CreateDbConnection(ConnectionString);
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, parameters);
                IDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return rdr;
            }
            catch (Exception ex)
            {
                conn.Close();
                cmd.Dispose();
                throw ex;
            }
        }
        #endregion

        #region 查询数据填充到数据集DataSet中
        /// <summary>
        /// 查询数据填充到数据集DataSet中
        /// </summary>
        /// <param name="cmdType">执行命令的类型</param>
        /// <param name="cmdText">命令文本</param>
        /// <returns></returns>
        public static DataSet GetDataSet(CommandType cmdType, string cmdText)
        {
            DataSet ds = new DataSet();
            DbCommand cmd = DbFactory.CreateDbCommand();
            DbConnection conn = DbFactory.CreateDbConnection(ConnectionString);
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, null);
                IDataAdapter ida = DbFactory.CreateDataAdapter();
                ida.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
                cmd.Dispose();
            }
        }

        /// <summary>
        /// 查询数据填充到数据集DataSet中
        /// </summary>
        /// <param name="cmdType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="cmdText">命令文本</param>
        /// <param name="parameters">sql语句对应参数</param>
        /// <returns>数据集DataSet对象</returns>
        public static DataSet GetDataSet(CommandType cmdType, string cmdText, params DbParameter[] parameters)
        {
            DataSet ds = new DataSet();
            DbCommand cmd = DbFactory.CreateDbCommand();
            DbConnection conn = DbFactory.CreateDbConnection(ConnectionString);
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, parameters);
                IDbDataAdapter sda = DbFactory.CreateDataAdapter(cmd);
                sda.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                conn.Close();
                cmd.Dispose();
                throw ex;
            }
            finally
            {
                conn.Close();
                cmd.Dispose();
            }
        }
        #endregion

        public static object ExecuteScalar(CommandType cmdType, string cmdText, params DbParameter[] parameters)
        {
            DbCommand cmd = DbFactory.CreateDbCommand();
            try
            {
                using (DbConnection conn = DbFactory.CreateDbConnection(ConnectionString))
                {
                    PrepareCommand(cmd, conn, null, cmdType, cmdText, parameters);
                    object val = cmd.ExecuteScalar();
                    return val;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                cmd.Dispose();
            }
        }

        /// <summary>
        /// 依靠数据库连接字符串connectionString,
        /// 使用所提供参数，执行返回首行首列命令
        /// </summary>
        /// <param name="commandType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="commandText">存储过程名称或者T-SQL命令行</param>
        /// <returns>返回一个对象，使用Convert.To{Type}将该对象转换成想要的数据类型。</returns>
        public static object ExecuteScalar(CommandType cmdType, string cmdText)
        {
            DbCommand cmd = DbFactory.CreateDbCommand();
            try
            {
                using (DbConnection connection = DbFactory.CreateDbConnection(ConnectionString))
                {
                    PrepareCommand(cmd, connection, null, cmdType, cmdText, null);
                    object val = cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                    return val;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Dispose();
            }
        }

        /// <summary>
        /// 为即将执行准备一个命令
        /// </summary>
        /// <param name="cmd">SqlCommand对象</param>
        /// <param name="conn">Connection对象</param>
        /// <param name="isOpenTrans">DbTransaction对象</param>
        /// <param name="cmdType">执行命令的类型(存储过程或T-SQL等)</param>
        /// <param name="cmdText">存储过程名称或者T-SQL命令行, e.g. Select * from Products</param>
        /// <param name="cmdParms">SqlParameters to use in the command</param>
        private static void PrepareCommand(DbCommand cmd, DbConnection conn, DbTransaction isOpenTrans, CommandType cmdType,
            string cmdText, DbParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            cmd.CommandTimeout = int.Parse(ConfigurationManager.AppSettings["CommandTimeout"]);  //数据库访问超时限制
            if (isOpenTrans != null)
            {
                cmd.Transaction = isOpenTrans;
            }
            cmd.CommandType = cmdType;
            if (cmdParms != null)
            {
                cmd.Parameters.AddRange(cmdParms);
            }
        }

        /// <summary>
        /// 数据库类型的字符串枚举转换
        /// </summary>
        /// <param name="value"></param>
        public void DatabaseTypeEnumParse(string value)
        {
            try
            {
                switch (value)
                {
                    case "System.Data.SqlClient":
                        DbType = DatabaseType.SqlServer;
                        break;
                    case "System.Data.OracleClient":
                        DbType = DatabaseType.Oracle;
                        break;
                    default:
                        break;
                }
            }
            catch
            {
                throw new Exception("数据库类型\"" + value + "\"错误，请检查！");
            }
        }
    }
}
