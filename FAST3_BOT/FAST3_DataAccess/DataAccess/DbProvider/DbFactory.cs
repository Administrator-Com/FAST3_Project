﻿using Devart.Data.Oracle;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace FAST3_DataAccess
{
    /// <summary>
    /// 数据库服务工厂
    /// </summary>
    public class DbFactory
    {
        /// <summary>
        /// 根据配置文件中所配置的数据库类型
        /// 来获取命令参数中的参数符号Oracle为":",Sqlserver为"@"
        /// </summary>
        /// <returns></returns>
        public static string CreateDbParmCharacter()
        {
            string character = string.Empty;
            switch (DbHelper.DbType)
            {
                case DatabaseType.SqlServer:
                    character = "@";
                    break;
                case DatabaseType.Oracle:
                    character = ":";
                    break;
                default:
                    throw new Exception("数据库类型目前不支持!");
            }
            return character;
        }

        #region 创建数据库链接对象CreateDbConnection
        /// <summary>
        /// 根据配置文件中所配置的数据库类型和传入的
        /// 数据库连接字符串来创建相应数据库连接对象
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static DbConnection CreateDbConnection(string connectionString)
        {
            DbConnection conn = null;
            switch (DbHelper.DbType)
            {
                case DatabaseType.SqlServer:
                    conn = new SqlConnection(connectionString);
                    break;
                case DatabaseType.Oracle:
                    conn = new OracleConnection(connectionString);
                    break;
                default:
                    throw new Exception("数据库类型目前不支持!");
            }
            return conn;
        }
        #endregion

        #region 创建数据库命令对象CreateDbCommand
        /// <summary>
        /// 根据配置文件中所配置的数据库类型
        /// 来创建相应数据库命令对象
        /// </summary>
        /// <returns></returns>
        public static DbCommand CreateDbCommand()
        {
            DbCommand cmd = null;
            switch (DbHelper.DbType)
            {
                case DatabaseType.SqlServer:
                    cmd = new SqlCommand();
                    break;
                case DatabaseType.Oracle:
                    cmd = new OracleCommand();
                    break;
                default:
                    throw new Exception("数据库类型目前不支持!");
            }
            return cmd;
        }
        #endregion

        #region 创建数据库适配器对象CreateDataAdapter
        /// <summary>
        /// 根据配置文件中所配置的数据库类型
        /// 来创建相应数据库适配器对象
        /// </summary>
        /// <returns></returns>
        public static IDbDataAdapter CreateDataAdapter()
        {
            IDbDataAdapter adapter = null;
            switch (DbHelper.DbType)
            {
                case DatabaseType.SqlServer:
                    adapter = new SqlDataAdapter();
                    break;
                case DatabaseType.Oracle:
                    adapter = new OracleDataAdapter();
                    break;
                default:
                    throw new Exception("数据库类型目前不支持!");
            }
            return adapter;
        }

        /// <summary>
        /// 根据配置文件中所配置的数据库类型
        /// 和传入的命令对象来创建相应数据库适配器对象
        /// </summary>
        /// <returns></returns>
        public static IDbDataAdapter CreateDataAdapter(DbCommand cmd)
        {
            IDbDataAdapter adapter = null;
            switch (DbHelper.DbType)
            {
                case DatabaseType.SqlServer:
                    adapter = new SqlDataAdapter((SqlCommand)cmd);
                    break;
                case DatabaseType.Oracle:
                    adapter = new OracleDataAdapter((OracleCommand)cmd);
                    break;
                default: throw new Exception("数据库类型目前不支持！");
            }
            return adapter;
        }
        #endregion

        #region 创建数据库参数对象CreateDbParameter
        /// <summary>
        /// 根据配置文件中所配置的数据库类型
        /// 来创建相应数据库的参数对象
        /// </summary>
        /// <returns></returns>
        public static DbParameter CreateDbParameter()
        {
            DbParameter param = null;
            switch (DbHelper.DbType)
            {
                case DatabaseType.SqlServer:
                    param = new SqlParameter();
                    break;
                case DatabaseType.Oracle:
                    param = new OracleParameter();
                    break;
                default:
                    throw new Exception("数据库类型目前不支持！");
            }
            return param;
        }

        /// <summary>
        /// 根据配置文件中所配置的数据库类型
        /// 来创建相应数据库的参数对象
        /// </summary>
        /// <returns></returns>
        public static DbParameter CreateDbParameter(string paramName, object value)
        {
            DbParameter param = CreateDbParameter();
            param.ParameterName = paramName;
            param.Value = value;
            return param;
        }

        /// <summary>
        /// 根据配置文件中所配置的数据库类型
        /// 来创建相应数据库的参数对象
        /// </summary>
        /// <returns></returns>
        public static DbParameter CreateDbParameter(string paramName, object value, DbType dbType)
        {
            DbParameter param = CreateDbParameter();
            param.DbType = dbType;
            param.ParameterName = paramName;
            param.Value = value;
            return param;
        }

        /// <summary>
        /// 根据配置文件中所配置的数据库类型
        /// 来创建相应数据库的参数对象
        /// </summary>
        /// <returns></returns>
        public static DbParameter CreateDbParameter(string paramName, object value, DbType dbType, int size)
        {
            DbParameter param = CreateDbParameter();
            param.DbType = dbType;
            param.ParameterName = paramName;
            param.Value = value;
            param.Size = size;
            return param;
        }

        /// <summary>
        /// 根据配置文件中所配置的数据库类型
        /// 来创建相应数据库的参数对象
        /// </summary>
        /// <returns></returns>
        public static DbParameter CreateDbParameter(string paramName, object value, int size)
        {
            DbParameter param = CreateDbParameter();
            param.ParameterName = paramName;
            param.Value = value;
            param.Size = size;
            return param;
        }

        /// <summary>
        /// 根据配置文件中所配置的数据库类型
        /// 来创建相应数据库的参数对象
        /// </summary>
        /// <returns></returns>
        public static DbParameter CreateDbOutParameter(string paramName, int size)
        {
            DbParameter param = CreateDbParameter();
            param.Direction = ParameterDirection.Output;
            param.ParameterName = paramName;
            param.Size = size;
            return param;
        }

        /// <summary>
        /// 根据配置文件中所配置的数据库类型
        /// 和传入的参数来创建相应数据库的sql语句对应参数对象
        /// </summary>
        /// <returns></returns>
        public static DbParameter[] CreateDbParameters(int size)
        {
            int i = 0;
            DbParameter[] param = null;
            switch (DbHelper.DbType)
            {
                case DatabaseType.SqlServer:
                    param = new SqlParameter[size];
                    while (i < size) { param[i] = new SqlParameter(); i++; }
                    break;
                case DatabaseType.Oracle:
                    param = new OracleParameter[size];
                    while (i < size) { param[i] = new OracleParameter(); i++; }
                    break;
                default:
                    throw new Exception("数据库类型目前不支持！");
            }
            return param;
        }
        #endregion

    }
}
