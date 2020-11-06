using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace FAST3_DataAccess
{
    public class Database : IDatabase, IDisposable
    {
        public static string ConnString { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connstring"></param>
        public Database(string connstring)
        {
            DbHelper dbHelper = new DbHelper(connstring);
        }

        /// <summary>
        /// 数据库连接对象
        /// </summary>
        private DbConnection dbConnection { get; set; }

        /// <summary>
        /// 事务对象
        /// </summary>
        private DbTransaction IsOpenTrans { get; set; }

        /// <summary>
        /// 是否已在事务之中
        /// </summary>
        public bool InTransaction { get; set; }

        #region 数据库连接、事务、内存管理
        /// <summary>
        /// 事务开始
        /// </summary>
        /// <returns></returns>
        public DbTransaction BeginTrans()
        {
            if (!this.InTransaction)
            {
                dbConnection = DbFactory.CreateDbConnection(DbHelper.ConnectionString);
                if (dbConnection.State == ConnectionState.Closed)
                {
                    dbConnection.Open();
                }
                this.InTransaction = true;
                this.IsOpenTrans = dbConnection.BeginTransaction();
            }
            return IsOpenTrans;
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        public void Rollback()
        {
            if (this.InTransaction)
            {
                this.InTransaction = false;
                this.IsOpenTrans.Rollback();
                this.Close();
            }
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public void Commit()
        {
            if (this.InTransaction)
            {
                this.InTransaction = false;
                this.IsOpenTrans.Commit();
                this.Close();
            }
        }

        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public void Close()
        {
            //关闭连接
            if (this.dbConnection != null)
            {
                this.dbConnection.Close();
                this.dbConnection.Dispose();
            }
            //关闭事务
            if (this.IsOpenTrans != null)
            {
                this.IsOpenTrans.Dispose();
            }
            this.dbConnection = null;
            this.IsOpenTrans = null;
        }

        /// <summary>
        /// 内存回收
        /// </summary>
        public void Dispose()
        {
            if (this.dbConnection != null)
            {
                this.dbConnection.Dispose();
            }
            if (this.IsOpenTrans != null)
            {
                this.IsOpenTrans.Dispose();
            }
        }
        #endregion

        /// <summary>
        /// 大批量数据插入
        /// </summary>
        /// <param name="datatable">资料表</param>
        /// <returns></returns>
        public bool BulkInsert(DataTable datatable)
        {
            return false;
        }

        #region 执行SQL语句ExecuteBySql
        public int ExecuteBySql(StringBuilder strSql)
        {
            return DbHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString());
        }

        public int ExecuteBySql(StringBuilder strSql, DbTransaction isOpenTrans)
        {
            return DbHelper.ExecuteNonQuery(isOpenTrans, CommandType.Text, strSql.ToString());
        }

        public int ExecuteBySql(StringBuilder strSql, DbParameter[] parameters)
        {
            return DbHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameters);
        }

        public int ExecuteBySql(StringBuilder strSql, DbParameter[] parameters, DbTransaction isOpenTrans)
        {
            return DbHelper.ExecuteNonQuery(isOpenTrans, CommandType.Text, strSql.ToString(), parameters);
        }
        #endregion

        #region 执行存储过程
        public int ExecuteByProc(string procName)
        {
            return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, procName);
        }

        public int ExecuteByProc(string procName, DbTransaction isOpenTrans)
        {
            return DbHelper.ExecuteNonQuery(isOpenTrans, CommandType.StoredProcedure, procName);
        }

        public int ExecuteByProc(string procName, DbParameter[] parameters)
        {
            return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, procName, parameters);
        }

        public int ExecuteByProc(string procName, DbParameter[] parameters, DbTransaction isOpenTrans)
        {
            return DbHelper.ExecuteNonQuery(isOpenTrans, CommandType.StoredProcedure, procName, parameters);
        }
        #endregion

        #region 插入数据Insert
        public int Insert<T>(T entity)
        {
            object val = 0;
            StringBuilder strSql = DatabaseCommon.InsertSql(entity);
            DbParameter[] parameter = DatabaseCommon.GetParameter(entity);
            val = DbHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameter);
            return Convert.ToInt32(val);
        }

        public int Insert<T>(T entity, DbTransaction isOpenTrans)
        {
            object val = 0;
            StringBuilder strSql = DatabaseCommon.InsertSql<T>(entity);
            DbParameter[] parameter = DatabaseCommon.GetParameter<T>(entity);
            val = DbHelper.ExecuteNonQuery(isOpenTrans, CommandType.Text, strSql.ToString(), parameter);
            return Convert.ToInt32(val);
        }

        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">实体类对象</param>
        /// <returns></returns>
        public int Insert<T>(List<T> entity)
        {
            object val = 0;
            DbTransaction isOpenTrans = this.BeginTrans();
            try
            {
                foreach (var item in entity)
                {
                    this.Insert(item, isOpenTrans);
                }
                this.Commit();
                val = 1;
            }
            catch (Exception ex)
            {
                this.Rollback();
                this.Close();
                val = -1;
                throw ex;
            }
            return Convert.ToInt32(val);
        }

        public int Insert<T>(List<T> entity, DbTransaction isOpenTrans)
        {
            object val = 0;
            try
            {
                foreach (var item in entity)
                {
                    this.Insert(item, isOpenTrans);
                }
                val = 1;
            }
            catch (Exception ex)
            {
                val = -1;
                throw ex;
            }
            return Convert.ToInt32(val);
        }

        public int Insert(string tableName, Hashtable ht)
        {
            object val = 0;
            StringBuilder strSql = DatabaseCommon.InsertSql(tableName, ht);
            DbParameter[] parameter = DatabaseCommon.GetParameter(ht);
            val = DbHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameter);
            return Convert.ToInt32(val);
        }

        public int Insert(string tableName, Hashtable ht, DbTransaction isOpenTrans)
        {
            object val = 0;
            StringBuilder strSql = DatabaseCommon.InsertSql(tableName, ht);
            DbParameter[] parameter = DatabaseCommon.GetParameter(ht);
            val = DbHelper.ExecuteNonQuery(isOpenTrans, CommandType.Text, strSql.ToString(), parameter);
            return Convert.ToInt32(val);
        }
        #endregion

        #region 修改数据Update
        public int Update<T>(T entity)
        {
            int val = 0;
            StringBuilder strSql = DatabaseCommon.UpdateSql(entity);
            DbParameter[] parameter = DatabaseCommon.GetParameter(entity);
            val = DbHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(),parameter);
            return val;
        }

        public int Update<T>(T entity, DbTransaction isOpenTrans)
        {
            int val = 0;
            StringBuilder strSql = DatabaseCommon.UpdateSql(entity);
            DbParameter[] parameter = DatabaseCommon.GetParameter(entity);
            val = DbHelper.ExecuteNonQuery(isOpenTrans, CommandType.Text, strSql.ToString(), parameter);
            return val;
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName">实体属性名称</param>
        /// <param name="propertyValue">字段值</param>
        /// <returns></returns>
        public int Update<T>(string propertyName, string propertyValue)
        {
            int val = 0;
            StringBuilder sb = new StringBuilder();
            sb.Append("Update ");
            sb.Append(typeof(T).Name);
            sb.Append(" Set ");
            sb.Append(propertyName);
            sb.Append("=");
            sb.Append(DbHelper.DbParmChar + propertyName);
            IList<DbParameter> parameter = new List<DbParameter>
            {
                DbFactory.CreateDbParameter(DbHelper.DbParmChar + propertyName, propertyValue)
            };
            val = DbHelper.ExecuteNonQuery(CommandType.Text, sb.ToString(), parameter.ToArray());
            return val;
        }

        public int Update<T>(string propertyName, string propertyValue, DbTransaction isOpenTrans)
        {
            int val = 0;
            StringBuilder sb = new StringBuilder();
            sb.Append("Update ");
            sb.Append(typeof(T).Name);
            sb.Append(" Set ");
            sb.Append(propertyName);
            sb.Append("=");
            sb.Append(DbHelper.DbParmChar + propertyName);
            IList<DbParameter> parameter = new List<DbParameter>
            {
                DbFactory.CreateDbParameter(DbHelper.DbParmChar + propertyName, propertyValue)
            };
            val = DbHelper.ExecuteNonQuery(isOpenTrans, CommandType.Text, sb.ToString(), parameter.ToArray());
            return val;
        }

        /// <summary>
        /// 批量修改数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public int Update<T>(List<T> entity)
        {
            int val = 0;
            DbTransaction isOpenTrans = this.BeginTrans();
            try
            {
                foreach (var item in entity)
                {
                    this.Update(entity, isOpenTrans);
                }
                this.Commit();
                val = 1;
            }
            catch (Exception ex)
            {
                this.Rollback();
                this.Close();
                val = -1;
                throw ex;
            }
            return val;
        }

        public int Update<T>(List<T> entity, DbTransaction isOpenTrans)
        {
            int val = 0;
            try
            {
                foreach (var item in entity)
                {
                    this.Update(item, isOpenTrans);
                }
                val = 1;
            }
            catch (Exception ex)
            {
                val = -1;
                throw ex;
            }
            return val;
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="tableName">表明</param>
        /// <param name="ht">哈希表</param>
        /// <param name="propertyName">主键字段</param>
        /// <returns></returns>
        public int Update(string tableName, Hashtable ht, string propertyName)
        {
            int val = 0;
            StringBuilder strSql = DatabaseCommon.UpdateSql(tableName, ht, propertyName);
            DbParameter[] parameter = DatabaseCommon.GetParameter(ht);
            val = DbHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameter);
            return val;
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="ht">哈希表</param>
        /// <param name="propertyName">主键字段</param>
        /// <param name="isOpenTrans">事务对象</param>
        /// <returns></returns>
        public int Update(string tableName, Hashtable ht, string propertyName, DbTransaction isOpenTrans)
        {
            int val = 0;
            StringBuilder strSql = DatabaseCommon.UpdateSql(tableName, ht, propertyName);
            DbParameter[] parameter = DatabaseCommon.GetParameter(ht);
            val = DbHelper.ExecuteNonQuery(isOpenTrans, CommandType.Text, strSql.ToString(), parameter);
            return val;
        }
        #endregion

        #region 删除数据Delete
        public int Delete<T>(T entity)
        {
            int val = 0;
            StringBuilder strSql = DatabaseCommon.DeleteSql(entity);
            DbParameter[] parameter = DatabaseCommon.GetParameter(entity);
            val = DbHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameter);
            return val;
        }

        public int Delete<T>(T entity, DbTransaction isOpenTrans)
        {
            int val = 0;
            StringBuilder strSql = DatabaseCommon.DeleteSql(entity);
            DbParameter[] parameter = DatabaseCommon.GetParameter(entity);
            val = DbHelper.ExecuteNonQuery(isOpenTrans, CommandType.Text, strSql.ToString(), parameter.ToArray());
            return val;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyValue">主键值</param>
        /// <returns></returns>
        public int Delete<T>(object propertyValue)
        {
            int val = 0;
            string tableName = typeof(T).Name;  //获取表名
            string pkName = DatabaseCommon.GetKeyField<T>().ToString(); //获取主键名
            StringBuilder strSql = DatabaseCommon.DeleteSql(tableName, pkName);
            IList<DbParameter> parameter = new List<DbParameter>
            {
                DbFactory.CreateDbParameter(DbHelper.DbParmChar + pkName, propertyValue)
            };
            val = DbHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameter.ToArray());
            return val;
        }

        public int Delete<T>(object propertyValue, DbTransaction isOpenTrans)
        {
            int val = 0;
            string tableName = typeof(T).Name;  //获取表名
            string pkName = DatabaseCommon.GetKeyField<T>().ToString();
            StringBuilder strSql = DatabaseCommon.DeleteSql(tableName, pkName);
            IList<DbParameter> parameter = new List<DbParameter>
            {
                DbFactory.CreateDbParameter(DbHelper.DbParmChar + pkName, propertyValue)
            };
            val = DbHelper.ExecuteNonQuery(isOpenTrans, CommandType.Text, strSql.ToString(), parameter.ToArray());
            return val;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName">主键值</param>
        /// <param name="propertyValue">字段值</param>
        /// <returns></returns>
        public int Delete<T>(string propertyName, string propertyValue)
        {
            int val = 0;
            string tableName = typeof(T).Name;
            StringBuilder strSql = DatabaseCommon.DeleteSql(tableName, propertyName);
            IList<DbParameter> parameter = new List<DbParameter>
            {
                DbFactory.CreateDbParameter(DbHelper.DbParmChar + propertyName, propertyValue)
            };
            val = DbHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameter.ToArray());
            return val;
        }

        public int Delete<T>(string propertyName, string propertyValue, DbTransaction isOpenTrans)
        {
            int val = 0;
            string tableName = typeof(T).Name;//获取表名
            StringBuilder strSql = DatabaseCommon.DeleteSql(tableName, propertyName);
            IList<DbParameter> parameter = new List<DbParameter>
            {
                DbFactory.CreateDbParameter(DbHelper.DbParmChar + propertyName, propertyValue)
            };
            val = DbHelper.ExecuteNonQuery(isOpenTrans, CommandType.Text, strSql.ToString(), parameter.ToArray());
            return val;
        }

        public int Delete(string tableName, string propertyName, string propertyValue)
        {
            int val = 0;
            StringBuilder strSql = DatabaseCommon.DeleteSql(tableName, propertyName);
            IList<DbParameter> parameter = new List<DbParameter>
            {
                DbFactory.CreateDbParameter(DbHelper.DbParmChar + propertyName, propertyValue)
            };
            val = DbHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameter.ToArray());
            return val;
        }

        public int Delete(string tableName, string propertyName, string propertyValue, DbTransaction isOpenTrans)
        {
            int val = 0;
            StringBuilder strSql = DatabaseCommon.DeleteSql(tableName, propertyName);
            IList<DbParameter> parameter = new List<DbParameter>
            {
                DbFactory.CreateDbParameter(DbHelper.DbParmChar + propertyName, propertyValue)
            };
            val = DbHelper.ExecuteNonQuery(isOpenTrans, CommandType.Text, strSql.ToString(), parameter.ToArray());
            return val;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="ht">键值生成SQL条件</param>
        /// <returns></returns>
        public int Delete(string tableName, Hashtable ht)
        {
            int val = 0;
            StringBuilder strSql = DatabaseCommon.DeleteSql(tableName, ht);
            DbParameter[] parameter = DatabaseCommon.GetParameter(ht);
            val = DbHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameter);
            return val;
        }

        public int Delete(string tableName, Hashtable ht, DbTransaction isOpenTrans)
        {
            int val = 0;
            StringBuilder strSql = DatabaseCommon.DeleteSql(tableName, ht);
            DbParameter[] parameter = DatabaseCommon.GetParameter(ht);
            val = DbHelper.ExecuteNonQuery(isOpenTrans, CommandType.Text, strSql.ToString(), parameter);
            return val;
        }

        /// <summary>
        /// 批量删除数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyValue">主键值：数组1,2,3,4,5,6.....</param>
        /// <returns></returns>
        public int Delete<T>(object[] propertyValue)
        {
            int val = 0;
            string tableName = typeof(T).Name;
            string pkName = DatabaseCommon.GetKeyField<T>().ToString();
            StringBuilder strSql = new StringBuilder("DELETE FROM " + tableName + " WHERE " + pkName + " IN (");
            try
            {
                IList<DbParameter> parameter = new List<DbParameter>();
                int index = 0;
                string str = DbHelper.DbParmChar + "ID" + index;
                for (int i = 0; i < (propertyValue.Length - 1); i++)
                {
                    object obj = propertyValue[i];
                    str = DbHelper.DbParmChar + "ID" + index;
                    strSql.Append(str).Append(",");
                    parameter.Add(DbFactory.CreateDbParameter(str, obj));
                    index++;
                }
                str = DbHelper.DbParmChar + "ID" + index;
                strSql.Append(str);
                parameter.Add(DbFactory.CreateDbParameter(str, propertyValue[index]));
                strSql.Append(")");
                val = DbHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameter.ToArray()); ;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return val;
        }

        public int Delete<T>(object[] propertyValue, DbTransaction isOpenTrans)
        {
            string tableName = typeof(T).Name;//获取表名
            string pkName = DatabaseCommon.GetKeyField<T>().ToString();//获取主键
            StringBuilder strSql = new StringBuilder("DELETE FROM " + tableName + " WHERE " + DbHelper.DbParmChar + pkName + " IN (");
            try
            {
                IList<DbParameter> parameter = new List<DbParameter>();
                int index = 0;
                string str = DbHelper.DbParmChar + "ID" + index;
                for (int i = 0; i < (propertyValue.Length - 1); i++)
                {
                    object obj2 = propertyValue[i];
                    str = DbHelper.DbParmChar + "ID" + index;
                    strSql.Append(str).Append(",");
                    parameter.Add(DbFactory.CreateDbParameter(str, obj2));
                    index++;
                }
                str = DbHelper.DbParmChar + "ID" + index;
                strSql.Append(str);
                parameter.Add(DbFactory.CreateDbParameter(str, propertyValue[index]));
                strSql.Append(")");
                return DbHelper.ExecuteNonQuery(isOpenTrans, CommandType.Text, strSql.ToString(), parameter.ToArray()); ;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName">实体属性名称</param>
        /// <param name="propertyValue">字段值：数组1,2,3,4,5,6.....</param>
        /// <returns></returns>
        public int Delete<T>(string propertyName, object[] propertyValue)
        {
            string tableName = typeof(T).Name;//获取表名
            string pkName = propertyName;
            StringBuilder strSql = new StringBuilder("DELETE FROM " + tableName + " WHERE " + DbHelper.DbParmChar + pkName + " IN (");
            try
            {
                IList<DbParameter> parameter = new List<DbParameter>();
                int index = 0;
                string str = DbHelper.DbParmChar + "ID" + index;
                for (int i = 0; i < (propertyValue.Length - 1); i++)
                {
                    object obj2 = propertyValue[i];
                    str = DbHelper.DbParmChar + "ID" + index;
                    strSql.Append(str).Append(",");
                    parameter.Add(DbFactory.CreateDbParameter(str, obj2));
                    index++;
                }
                str = DbHelper.DbParmChar + "ID" + index;
                strSql.Append(str);
                parameter.Add(DbFactory.CreateDbParameter(str, propertyValue[index]));
                strSql.Append(")");
                return DbHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameter.ToArray()); ;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Delete<T>(string propertyName, object[] propertyValue, DbTransaction isOpenTrans)
        {
            string tableName = typeof(T).Name;//获取表名
            string pkName = propertyName;
            StringBuilder strSql = new StringBuilder("DELETE FROM " + tableName + " WHERE " + DbHelper.DbParmChar + pkName + " IN (");
            try
            {
                IList<DbParameter> parameter = new List<DbParameter>();
                int index = 0;
                string str = DbHelper.DbParmChar + "ID" + index;
                for (int i = 0; i < (propertyValue.Length - 1); i++)
                {
                    object obj2 = propertyValue[i];
                    str = DbHelper.DbParmChar + "ID" + index;
                    strSql.Append(str).Append(",");
                    parameter.Add(DbFactory.CreateDbParameter(str, obj2));
                    index++;
                }
                str = DbHelper.DbParmChar + "ID" + index;
                strSql.Append(str);
                parameter.Add(DbFactory.CreateDbParameter(str, propertyValue[index]));
                strSql.Append(")");
                return DbHelper.ExecuteNonQuery(isOpenTrans, CommandType.Text, strSql.ToString(), parameter.ToArray()); ;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Delete(string tableName, string propertyName, object[] propertyValue)
        {
            string pkName = propertyName;
            StringBuilder strSql = new StringBuilder("DELETE FROM " + tableName + " WHERE " + DbHelper.DbParmChar + pkName + " IN (");
            try
            {
                IList<DbParameter> parameter = new List<DbParameter>();
                int index = 0;
                string str = DbHelper.DbParmChar + "ID" + index;
                for (int i = 0; i < (propertyValue.Length - 1); i++)
                {
                    object obj2 = propertyValue[i];
                    str = DbHelper.DbParmChar + "ID" + index;
                    strSql.Append(str).Append(",");
                    parameter.Add(DbFactory.CreateDbParameter(str, obj2));
                    index++;
                }
                str = DbHelper.DbParmChar + "ID" + index;
                strSql.Append(str);
                parameter.Add(DbFactory.CreateDbParameter(str, propertyValue[index]));
                strSql.Append(")");
                return DbHelper.ExecuteNonQuery(CommandType.Text, strSql.ToString(), parameter.ToArray()); ;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Delete(string tableName, string propertyName, object[] propertyValue, DbTransaction isOpenTrans)
        {
            string pkName = propertyName;
            StringBuilder strSql = new StringBuilder("DELETE FROM " + tableName + " WHERE " + DbHelper.DbParmChar + pkName + " IN (");
            try
            {
                IList<DbParameter> parameter = new List<DbParameter>();
                int index = 0;
                string str = DbHelper.DbParmChar + "ID" + index;
                for (int i = 0; i < (propertyValue.Length - 1); i++)
                {
                    object obj2 = propertyValue[i];
                    str = DbHelper.DbParmChar + "ID" + index;
                    strSql.Append(str).Append(",");
                    parameter.Add(DbFactory.CreateDbParameter(str, obj2));
                    index++;
                }
                str = DbHelper.DbParmChar + "ID" + index;
                strSql.Append(str);
                parameter.Add(DbFactory.CreateDbParameter(str, propertyValue[index]));
                strSql.Append(")");
                return DbHelper.ExecuteNonQuery(isOpenTrans, CommandType.Text, strSql.ToString(), parameter.ToArray()); ;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 查询数据、返回条数FindCount
        /// <summary>
        /// 查询数据、返回条数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public int FindCount<T>() where T : new()
        {
            StringBuilder strSql = DatabaseCommon.SelectCountSql<T>();
            return Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, strSql.ToString()));
        }

        /// <summary>
        /// 查询数据、返回条数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName">实体属性名称</param>
        /// <param name="propertyValue">字段值</param>
        /// <returns></returns>
        public int FindCount<T>(string propertyName, string propertyValue) where T : new()
        {
            StringBuilder strSql = DatabaseCommon.SelectCountSql<T>();
            strSql.Append(" AND " + propertyName + " = " + DbHelper.DbParmChar + propertyName);
            IList<DbParameter> parameter = new List<DbParameter>
            {
                DbFactory.CreateDbParameter(DbHelper.DbParmChar + propertyName, propertyValue)
            };
            return Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, strSql.ToString(), parameter.ToArray()));
        }

        /// <summary>
        /// 查询数据、返回条数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="WhereSql">查询条件</param>
        /// <returns></returns>
        public int FindCount<T>(string WhereSql) where T : new()
        {
            StringBuilder strSql = DatabaseCommon.SelectCountSql<T>();
            strSql.Append(WhereSql);
            return Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, strSql.ToString()));
        }

        /// <summary>
        /// 查询数据、返回条数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="WhereSql">条件</param>
        /// <param name="parameters">sql语句对应参数</param>
        /// <returns></returns>
        public int FindCount<T>(string WhereSql, DbParameter[] parameters) where T : new()
        {
            StringBuilder strSql = DatabaseCommon.SelectCountSql<T>();
            strSql.Append(WhereSql);
            return Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 查询数据、返回条数
        /// </summary>
        /// <param name="strSql">Sql语句</param>
        /// <returns></returns>
        public int FindCountBySql(string strSql)
        {
            return Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, strSql));
        }

        public int FindCountBySql(string strSql, DbParameter[] parameters)
        {
            return Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, strSql, parameters));
        }
        #endregion

        #region 查询数据列表、返回List
        /// <summary>
        /// 查询数据列表、返回List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> FindList<T>() where T : new()
        {
            StringBuilder strSql = DatabaseCommon.SelectSql<T>();
            IDataReader dr = DbHelper.ExecuteReader(CommandType.Text, strSql.ToString());
            return DatabaseReader.ReaderToList<T>(dr);
        }

        /// <summary>
        /// 查询数据列表、返回List
        /// </summary>
        /// <param name="propertyName">实体属性名称</param>
        /// <param name="propertyValue">字段值</param>
        /// <returns></returns>
        public List<T> FindList<T>(string propertyName, string propertyValue) where T : new()
        {
            StringBuilder strSql = DatabaseCommon.SelectSql<T>();
            strSql.Append(" AND " + propertyName + " = " + DbHelper.DbParmChar + propertyName);
            IList<DbParameter> parameter = new List<DbParameter>
            {
                DbFactory.CreateDbParameter(DbHelper.DbParmChar + propertyName, propertyValue)
            };
            IDataReader dr = DbHelper.ExecuteReader(CommandType.Text, strSql.ToString(), parameter.ToArray());
            return DatabaseReader.ReaderToList<T>(dr);
        }

        /// <summary>
        /// 查询数据列表、返回List
        /// </summary>
        /// <param name="WhereSql">条件</param>
        /// <returns></returns>
        public List<T> FindList<T>(string WhereSql) where T : new()
        {
            StringBuilder strSql = DatabaseCommon.SelectSql<T>();
            strSql.Append(WhereSql);
            IDataReader dr = DbHelper.ExecuteReader(CommandType.Text, strSql.ToString());
            return DatabaseReader.ReaderToList<T>(dr);
        }

        /// <summary>
        /// 查询数据列表、返回List
        /// </summary>
        /// <param name="WhereSql">条件</param>
        /// <param name="parameters">sql语句对应参数</param>
        /// <returns></returns>
        public List<T> FindList<T>(string WhereSql, DbParameter[] parameters) where T : new()
        {
            StringBuilder strSql = DatabaseCommon.SelectSql<T>();
            strSql.Append(WhereSql);
            IDataReader dr = DbHelper.ExecuteReader(CommandType.Text, strSql.ToString(), parameters);
            return DatabaseReader.ReaderToList<T>(dr);
        }

        /// <summary>
        /// 查询数据列表、返回List
        /// </summary>
        /// <param name="strSql">Sql语句</param>
        /// <returns></returns>
        public List<T> FindListBySql<T>(string strSql)
        {
            IDataReader dr = DbHelper.ExecuteReader(CommandType.Text, strSql.ToString());
            return DatabaseReader.ReaderToList<T>(dr);
        }

        /// <summary>
        /// 查询数据列表、返回List
        /// </summary>
        /// <param name="strSql">Sql语句</param>
        /// <param name="parameters">sql语句对应参数</param>
        /// <returns></returns>
        public List<T> FindListBySql<T>(string strSql, DbParameter[] parameters)
        {
            IDataReader dr = DbHelper.ExecuteReader(CommandType.Text, strSql.ToString(), parameters);
            return DatabaseReader.ReaderToList<T>(dr);
        }

        /// <summary>
        /// 查询数据列表、返回List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="orderField">排序字段</param>
        /// <param name="orderType">排序类型</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="recordCount">返回查询条数</param>
        /// <returns></returns>
        public List<T> FindListPage<T>(string orderField, string orderType, int pageIndex, int pageSize, ref int recordCount) where T : new()
        {
            StringBuilder strSql = DatabaseCommon.SelectSql<T>();
            switch (DbHelper.DbType)
            {
                case DatabaseType.SqlServer:
                    return SqlServerHelper.GetPageList<T>(strSql.ToString(), orderField, orderType, pageIndex, pageSize, ref recordCount);
                case DatabaseType.Oracle:
                    return OracleHelper.GetPageList<T>(strSql.ToString(), orderField, orderType, pageIndex, pageSize, ref recordCount);

                default:
                    throw new Exception("数据库类型目前不支持！");
            }
        }

        /// <summary>
        /// 查询数据列表、返回List
        /// </summary>
        /// <param name="WhereSql">条件</param>
        /// <param name="orderField">排序字段</param>
        /// <param name="orderType">排序类型</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="recordCount">返回查询条数</param>
        /// <returns></returns>
        public List<T> FindListPage<T>(string WhereSql, string orderField, string orderType, int pageIndex, int pageSize, ref int recordCount) where T : new()
        {
            StringBuilder strSql = DatabaseCommon.SelectSql<T>();
            strSql.Append(WhereSql);
            switch (DbHelper.DbType)
            {
                case DatabaseType.SqlServer:
                    return SqlServerHelper.GetPageList<T>(strSql.ToString(), orderField, orderType, pageIndex, pageSize, ref recordCount);
                case DatabaseType.Oracle:
                    return OracleHelper.GetPageList<T>(strSql.ToString(), orderField, orderType, pageIndex, pageSize, ref recordCount);

                default:
                    throw new Exception("数据库类型目前不支持！");
            }
        }

        /// <summary>
        /// 查询数据列表、返回List
        /// </summary>
        /// <param name="WhereSql">条件</param>
        /// <param name="parameters">sql语句对应参数</param>
        /// <param name="orderField">排序字段</param>
        /// <param name="orderType">排序类型</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="recordCount">返回查询条数</param>
        public List<T> FindListPage<T>(string WhereSql, DbParameter[] parameters, string orderField, string orderType, int pageIndex, int pageSize, ref int recordCount) where T : new()
        {
            StringBuilder strSql = DatabaseCommon.SelectSql<T>();
            strSql.Append(WhereSql);
            switch (DbHelper.DbType)
            {
                case DatabaseType.SqlServer:
                    return SqlServerHelper.GetPageList<T>(strSql.ToString(), parameters, orderField, orderType, pageIndex, pageSize, ref recordCount);
                case DatabaseType.Oracle:
                    return OracleHelper.GetPageList<T>(strSql.ToString(), parameters, orderField, orderType, pageIndex, pageSize, ref recordCount);
                default:
                    throw new Exception("数据库类型目前不支持！");
            }
        }

        /// <summary>
        /// 查询数据列表、返回List
        /// </summary>
        /// <param name="strSql">Sql语句</param>
        /// <param name="orderField">排序字段</param>
        /// <param name="orderType">排序类型</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="recordCount">返回查询条数</param>
        /// <returns></returns>
        public List<T> FindListPageBySql<T>(string strSql, string orderField, string orderType, int pageIndex, int pageSize, ref int recordCount)
        {
            switch (DbHelper.DbType)
            {
                case DatabaseType.SqlServer:
                    return SqlServerHelper.GetPageList<T>(strSql, orderField, orderType, pageIndex, pageSize, ref recordCount);
                case DatabaseType.Oracle:
                    return OracleHelper.GetPageList<T>(strSql, orderField, orderType, pageIndex, pageSize, ref recordCount);
                default:
                    throw new Exception("数据库类型目前不支持！");
            }
        }

        /// <summary>
        /// 查询数据列表、返回List
        /// </summary>
        /// <param name="strSql">Sql语句</param>
        /// <param name="parameters">sql语句对应参数</param>
        /// <param name="orderField">排序字段</param>
        /// <param name="orderType">排序类型</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="recordCount">返回查询条数</param>
        /// <returns></returns>
        public List<T> FindListPageBySql<T>(string strSql, DbParameter[] parameters, string orderField, string orderType, int pageIndex, int pageSize, ref int recordCount)
        {
            switch (DbHelper.DbType)
            {
                case DatabaseType.SqlServer:
                    return SqlServerHelper.GetPageList<T>(strSql, parameters, orderField, orderType, pageIndex, pageSize, ref recordCount);
                case DatabaseType.Oracle:
                    return OracleHelper.GetPageList<T>(strSql, parameters, orderField, orderType, pageIndex, pageSize, ref recordCount);
                default:
                    throw new Exception("数据库类型目前不支持！");
            }
        }
        #endregion

        #region 查询数据列表、返回List(TOP)
        /// <summary>
        /// 查询数据列表、返回List
        /// </summary>
        /// <param name="Top">显示条数</param>
        /// <returns></returns>
        public List<T> FindListTop<T>(int Top) where T : new()
        {
            StringBuilder strSql = DatabaseCommon.SelectSql<T>();
            switch (DbHelper.DbType)
            {
                case DatabaseType.SqlServer:
                    strSql.Replace("SELECT", "SELECT TOP " + Top);
                    break;
                case DatabaseType.Oracle:
                    strSql.Append(" and rownum < " + Top + 1);
                    break;
                default:
                    throw new Exception("数据库类型目前不支持！");
            }
            IDataReader dr = DbHelper.ExecuteReader(CommandType.Text, strSql.ToString());
            return DatabaseReader.ReaderToList<T>(dr);
        }

        /// <summary>
        /// 查询数据列表、返回List
        /// </summary>
        /// <param name="Top">显示条数</param>
        /// <param name="propertyName">实体属性名称</param>
        /// <param name="propertyValue">字段值</param>
        /// <returns></returns>
        public List<T> FindListTop<T>(int Top, string propertyName, string propertyValue) where T : new()
        {
            StringBuilder strSql = DatabaseCommon.SelectSql<T>();
            strSql.Append(" AND " + propertyName + " = " + DbHelper.DbParmChar + propertyName);
            switch (DbHelper.DbType)
            {
                case DatabaseType.SqlServer:
                    strSql.Replace("SELECT", "SELECT TOP " + Top);
                    break;
                case DatabaseType.Oracle:
                    strSql.Append(" and rownum < " + Top + 1);
                    break;
                default:
                    throw new Exception("数据库类型目前不支持！");
            }
            IList<DbParameter> parameter = new List<DbParameter>
            {
                DbFactory.CreateDbParameter(DbHelper.DbParmChar + propertyName, propertyValue)
            };
            IDataReader dr = DbHelper.ExecuteReader(CommandType.Text, strSql.ToString(), parameter.ToArray());
            return DatabaseReader.ReaderToList<T>(dr);
        }

        /// <summary>
        /// 查询数据列表、返回List
        /// </summary>
        /// <param name="Top">显示条数</param>
        /// <param name="WhereSql">条件</param>
        /// <returns></returns>
        public List<T> FindListTop<T>(int Top, string WhereSql) where T : new()
        {
            StringBuilder strSql = DatabaseCommon.SelectSql<T>();
            strSql.Append(WhereSql);
            switch (DbHelper.DbType)
            {
                case DatabaseType.SqlServer:
                    strSql.Replace("SELECT", "SELECT TOP " + Top);
                    break;
                case DatabaseType.Oracle:
                    strSql.Append(" and rownum < " + Top + 1);
                    break;
                default:
                    throw new Exception("数据库类型目前不支持！");
            }
            IDataReader dr = DbHelper.ExecuteReader(CommandType.Text, strSql.ToString());
            return DatabaseReader.ReaderToList<T>(dr);
        }

        /// <summary>
        /// 查询数据列表、返回List
        /// </summary>
        /// <param name="Top">显示条数</param>
        /// <param name="WhereSql">条件</param>
        /// <param name="parameters">sql语句对应参数</param>
        /// <returns></returns>
        public List<T> FindListTop<T>(int Top, string WhereSql, DbParameter[] parameters) where T : new()
        {
            StringBuilder strSql = DatabaseCommon.SelectSql<T>();
            strSql.Append(WhereSql);
            switch (DbHelper.DbType)
            {
                case DatabaseType.SqlServer:
                    strSql.Replace("SELECT", "SELECT TOP " + Top);
                    break;
                case DatabaseType.Oracle:
                    strSql.Append(" and rownum < " + Top + 1);
                    break;
                default:
                    throw new Exception("数据库类型目前不支持！");
            }
            IDataReader dr = DbHelper.ExecuteReader(CommandType.Text, strSql.ToString(), parameters);
            return DatabaseReader.ReaderToList<T>(dr);
        }
        #endregion

        #region 查询数据列表、返回 DataTable（TOP）
        /// <summary>
        /// 查询数据列表、返回 DataTable
        /// </summary>
        /// <param name="Top">显示条数</param>
        /// <returns></returns>
        public DataTable FindTableTop<T>(int Top) where T : new()
        {
            StringBuilder strSql = DatabaseCommon.SelectSql<T>();
            switch (DbHelper.DbType)
            {
                case DatabaseType.SqlServer:
                    strSql.Replace("SELECT", "SELECT TOP " + Top);
                    break;
                case DatabaseType.Oracle:
                    strSql.Append(" and rownum < " + Top + 1);
                    break;
                default:
                    throw new Exception("数据库类型目前不支持！");
            }
            IDataReader dr = DbHelper.ExecuteReader(CommandType.Text, strSql.ToString());
            return DatabaseReader.ReaderToDataTable(dr);
        }
        /// <summary>
        /// 查询数据列表、返回 DataTable
        /// </summary>
        /// <param name="Top">显示条数</param>
        /// <param name="propertyName">实体属性名称</param>
        /// <param name="propertyValue">字段值</param>
        /// <returns></returns>
        public DataTable FindTableTop<T>(int Top, string propertyName, string propertyValue) where T : new()
        {
            StringBuilder strSql = DatabaseCommon.SelectSql<T>();
            strSql.Append(" AND " + propertyName + " = " + DbHelper.DbParmChar + propertyName);
            switch (DbHelper.DbType)
            {
                case DatabaseType.SqlServer:
                    strSql.Replace("SELECT", "SELECT TOP " + Top);
                    break;
                case DatabaseType.Oracle:
                    strSql.Append(" and rownum < " + Top + 1);
                    break;
                default:
                    throw new Exception("数据库类型目前不支持！");
            }
            IList<DbParameter> parameter = new List<DbParameter>
            {
                DbFactory.CreateDbParameter(DbHelper.DbParmChar + propertyName, propertyValue)
            };
            IDataReader dr = DbHelper.ExecuteReader(CommandType.Text, strSql.ToString(), parameter.ToArray());
            return DatabaseReader.ReaderToDataTable(dr);
        }

        /// <summary>
        /// 查询数据列表、返回 DataTable
        /// </summary>
        /// <param name="Top">显示条数</param>
        /// <param name="WhereSql">条件</param>
        public DataTable FindTableTop<T>(int Top, string WhereSql) where T : new()
        {
            StringBuilder strSql = DatabaseCommon.SelectSql<T>();
            strSql.Append(WhereSql);
            switch (DbHelper.DbType)
            {
                case DatabaseType.SqlServer:
                    strSql.Replace("SELECT", "SELECT TOP " + Top);
                    break;
                case DatabaseType.Oracle:
                    strSql.Append(" and rownum < " + Top + 1);
                    break;
                default:
                    throw new Exception("数据库类型目前不支持！");
            }
            IDataReader dr = DbHelper.ExecuteReader(CommandType.Text, strSql.ToString());
            return DatabaseReader.ReaderToDataTable(dr);
        }

        /// <summary>
        /// 查询数据列表、返回 DataTable
        /// </summary>
        /// <param name="Top">显示条数</param>
        /// <param name="WhereSql">条件</param>
        /// <param name="parameters">sql语句对应参数</param>
        public DataTable FindTableTop<T>(int Top, string WhereSql, DbParameter[] parameters) where T : new()
        {
            StringBuilder strSql = DatabaseCommon.SelectSql<T>();
            strSql.Append(WhereSql);
            switch (DbHelper.DbType)
            {
                case DatabaseType.SqlServer:
                    strSql.Replace("SELECT", "SELECT TOP " + Top);
                    break;
                case DatabaseType.Oracle:
                    strSql.Append(" and rownum < " + Top + 1);
                    break;
                default:
                    throw new Exception("数据库类型目前不支持！");
            }
            IDataReader dr = DbHelper.ExecuteReader(CommandType.Text, strSql.ToString(), parameters);
            return DatabaseReader.ReaderToDataTable(dr);
        }
        #endregion

        #region 查询数据列表、返回DataSet
        /// <summary>
        /// 查询数据列表、返回DataSet
        /// </summary>
        /// <param name="strSql">Sql语句</param>
        /// <returns></returns>
        public DataSet FindDataSetBySql(string strSql)
        {
            return DbHelper.GetDataSet(CommandType.Text, strSql);
        }

        /// <summary>
        /// 查询数据列表、返回DataSet
        /// </summary>
        /// <param name="strSql">Sql语句</param>
        /// <param name="parameters">sql语句对应参数</param>
        /// <returns></returns>
        public DataSet FindDataSetBySql(string strSql, DbParameter[] parameters)
        {
            return DbHelper.GetDataSet(CommandType.Text, strSql, parameters);
        }

        /// <summary>
        /// 查询数据列表，返回DataSet(存储过程版)
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <returns></returns>
        public DataSet FindDataSetByProc(string procName)
        {
            return DbHelper.GetDataSet(CommandType.StoredProcedure, procName);
        }

        public DataSet FindDataSetByProc(string procName, DbParameter[] parameters)
        {
            return DbHelper.GetDataSet(CommandType.StoredProcedure, procName, parameters);
        }
        #endregion

        #region 查询对象、返回实体
        /// <summary>
        /// 查询对象返回实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyValue">主键值</param>
        /// <returns></returns>
        public T FindEntity<T>(object propertyValue) where T : new()
        {
            string pkName = DatabaseCommon.GetKeyField<T>().ToString(); //获取主键字段
            StringBuilder strSql = DatabaseCommon.SelectSql<T>();
            strSql.Append(" AND ").Append(pkName).Append("=").Append(DbHelper.DbParmChar + pkName);
            switch (DbHelper.DbType)
            {
                case DatabaseType.SqlServer:
                    strSql.Replace("SELECT", "SELECT TOP " + 1);
                    break;
                case DatabaseType.Oracle:
                    break;
                default:
                    throw new Exception("数据库类型目前不支持！");
            }
            IList<DbParameter> parameter = new List<DbParameter>
            {
                DbFactory.CreateDbParameter(DbHelper.DbParmChar + pkName, propertyValue)
            };
            IDataReader dr = DbHelper.ExecuteReader(CommandType.Text, strSql.ToString(), parameter.ToArray());
            return DatabaseReader.ReaderToModel<T>(dr);
        }

        /// <summary>
        /// 查询对象、返回实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName">实体属性名称</param>
        /// <param name="propertyValue">字段值</param>
        /// <returns></returns>
        public T FindEntity<T>(string propertyName, object propertyValue) where T : new()
        {
            string pkName = propertyName;
            StringBuilder strSql = DatabaseCommon.SelectSql<T>();
            strSql.Append(" AND ").Append(pkName).Append("=").Append(DbHelper.DbParmChar + pkName);
            switch (DbHelper.DbType)
            {
                case DatabaseType.SqlServer:
                    strSql.Replace("SELECT", "SELECT TOP " + 1);
                    break;
                case DatabaseType.Oracle:
                    strSql.Append(" and rownum < 2");
                    break;
                default:
                    throw new Exception("数据库类型目前不支持！");
            }
            IList<DbParameter> parameter = new List<DbParameter>
            {
                DbFactory.CreateDbParameter(DbHelper.DbParmChar + pkName, propertyValue)
            };
            IDataReader dr = DbHelper.ExecuteReader(CommandType.Text, strSql.ToString(), parameter.ToArray());
            return DatabaseReader.ReaderToModel<T>(dr);
        }

        public T FindEntityBySql<T>(string strSql)
        {
            IDataReader dr = DbHelper.ExecuteReader(CommandType.Text, strSql.ToString());
            return DatabaseReader.ReaderToModel<T>(dr);
        }

        public T FindEntityBySql<T>(string strSql, DbParameter[] parameters)
        {
            IDataReader dr = DbHelper.ExecuteReader(CommandType.Text, strSql.ToString(), parameters);
            return DatabaseReader.ReaderToModel<T>(dr);
        }

        /// <summary>
        /// 查询对象、返回实体
        /// </summary>
        /// <param name="WhereSql">条件</param>
        public T FindEntityByWhere<T>(string WhereSql) where T : new()
        {
            StringBuilder strSql = DatabaseCommon.SelectSql<T>();
            strSql.Append(WhereSql);
            switch (DbHelper.DbType)
            {
                case DatabaseType.SqlServer:
                    strSql.Replace("SELECT", "SELECT TOP " + 1);
                    break;
                case DatabaseType.Oracle:
                    strSql.Append(" and rownum < 2");
                    break;
                default:
                    throw new Exception("数据库类型目前不支持！");
            }
            IDataReader dr = DbHelper.ExecuteReader(CommandType.Text, strSql.ToString());
            return DatabaseReader.ReaderToModel<T>(dr);
        }

        /// <summary>
        /// 查询对象、返回实体
        /// </summary>
        /// <param name="WhereSql">条件</param>
        /// <param name="parameters">sql语句对应参数</param>
        /// <returns></returns>
        public T FindEntityByWhere<T>(string WhereSql, DbParameter[] parameters) where T : new()
        {
            StringBuilder strSql = DatabaseCommon.SelectSql<T>();
            strSql.Append(WhereSql);
            switch (DbHelper.DbType)
            {
                case DatabaseType.SqlServer:
                    strSql.Replace("SELECT", "SELECT TOP " + 1);
                    break;
                case DatabaseType.Oracle:
                    strSql.Append(" and rownum < 2");
                    break;
                default:
                    throw new Exception("数据库类型目前不支持！");
            }
            IDataReader dr = DbHelper.ExecuteReader(CommandType.Text, strSql.ToString(), parameters);
            return DatabaseReader.ReaderToModel<T>(dr);
        }
        #endregion

        #region 查询对象、返回哈希表
        /// <summary>
        /// 查询对象、返回哈希表
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="propertyName">实体属性名称</param>
        /// <param name="propertyValue">字段值</param>
        /// <returns></returns>
        public Hashtable FindHashtable(string tableName, string propertyName, object propertyValue)
        {
            StringBuilder strSql = DatabaseCommon.SelectSql(tableName);
            strSql.Append(" AND ").Append(propertyName).Append("=").Append(DbHelper.DbParmChar + propertyName);
            switch (DbHelper.DbType)
            {
                case DatabaseType.SqlServer:
                    strSql.Replace("SELECT", "SELECT TOP " + 1);
                    break;
                case DatabaseType.Oracle:
                    strSql.Append(" and rownum < 2");
                    break;
                default:
                    throw new Exception("数据库类型目前不支持！");
            }
            IList<DbParameter> parameter = new List<DbParameter>
            {
                DbFactory.CreateDbParameter(DbHelper.DbParmChar + propertyName, propertyValue)
            };
            IDataReader dr = DbHelper.ExecuteReader(CommandType.Text, strSql.ToString(), parameter.ToArray());
            return DatabaseReader.ReaderToHashtable(dr);
        }

        /// <summary>
        /// 查询对象、返回哈希表
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="WhereSql">条件</param>
        /// <returns></returns>
        public Hashtable FindHashtable(string tableName, StringBuilder WhereSql)
        {
            StringBuilder strSql = DatabaseCommon.SelectSql(tableName);
            strSql.Append(WhereSql);
            switch (DbHelper.DbType)
            {
                case DatabaseType.SqlServer:
                    strSql.Replace("SELECT", "SELECT TOP " + 1);
                    break;
                case DatabaseType.Oracle:
                    strSql.Append(" and rownum < 2");
                    break;
                default:
                    throw new Exception("数据库类型目前不支持！");
            }
            IDataReader dr = DbHelper.ExecuteReader(CommandType.Text, strSql.ToString());
            return DatabaseReader.ReaderToHashtable(dr);
        }

        /// <summary>
        /// 查询对象、返回哈希表
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="WhereSql">条件</param>
        /// <param name="parameters">sql语句对应参数</param>
        public Hashtable FindHashtable(string tableName, StringBuilder WhereSql, DbParameter[] parameters)
        {
            StringBuilder strSql = DatabaseCommon.SelectSql(tableName);
            strSql.Append(WhereSql);
            switch (DbHelper.DbType)
            {
                case DatabaseType.SqlServer:
                    strSql.Replace("SELECT", "SELECT TOP " + 1);
                    break;
                case DatabaseType.Oracle:
                    strSql.Append(" and rownum < 2");
                    break;
                default:
                    throw new Exception("数据库类型目前不支持！");
            }
            IDataReader dr = DbHelper.ExecuteReader(CommandType.Text, strSql.ToString(), parameters);
            return DatabaseReader.ReaderToHashtable(dr);
        }

        /// <summary>
        /// 查询对象、返回哈希表
        /// </summary>
        /// <param name="strSql">Sql语句</param>
        /// <returns></returns>
        public Hashtable FindHashtableBySql(string strSql)
        {
            IDataReader dr = DbHelper.ExecuteReader(CommandType.Text, strSql.ToString());
            return DatabaseReader.ReaderToHashtable(dr);
        }

        public Hashtable FindHashtableBySql(string strSql, DbParameter[] parameters)
        {
            IDataReader dr = DbHelper.ExecuteReader(CommandType.Text, strSql.ToString(), parameters);
            return DatabaseReader.ReaderToHashtable(dr);
        }
        #endregion

        #region 查询数据返回最大数（预留）
        public object FindMax<T>(string propertyName) where T : new()
        {
            throw new NotImplementedException();
        }

        public object FindMax<T>(string propertyName, string WhereSql) where T : new()
        {
            throw new NotImplementedException();
        }

        public object FindMax<T>(string propertyName, string WhereSql, DbParameter[] parameters) where T : new()
        {
            throw new NotImplementedException();
        }

        public object FindMaxBySql(string strSql)
        {
            throw new NotImplementedException();
        }

        public object FindMaxBySql(string strSql, DbParameter[] parameters)
        {
            throw new NotImplementedException();
        }
        #endregion

        public DataTable FindTable<T>() where T : new()
        {
            throw new NotImplementedException();
        }

        public DataTable FindTable<T>(string propertyName, string propertyValue) where T : new()
        {
            throw new NotImplementedException();
        }

        public DataTable FindTable<T>(string WhereSql) where T : new()
        {
            throw new NotImplementedException();
        }

        public DataTable FindTable<T>(string WhereSql, DbParameter[] parameters) where T : new()
        {
            throw new NotImplementedException();
        }

        public DataTable FindTableByProc(string procName)
        {
            throw new NotImplementedException();
        }

        public DataTable FindTableByProc(string procName, DbParameter[] parameters)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 查询数据列表、返回 DataTable
        /// </summary>
        /// <param name="strSql">Sql语句</param>
        /// <returns></returns>
        public DataTable FindTableBySql(string strSql)
        {
            IDataReader dr = DbHelper.ExecuteReader(CommandType.Text, strSql.ToString());
            return DatabaseReader.ReaderToDataTable(dr);
        }

        public DataTable FindTableBySql(string strSql, DbParameter[] parameters)
        {
            throw new NotImplementedException();
        }

        public DataTable FindTablePage<T>(string orderField, string orderType, int pageIndex, int pageSize, ref int recordCount) where T : new()
        {
            throw new NotImplementedException();
        }

        public DataTable FindTablePage<T>(string WhereSql, string orderField, string orderType, int pageIndex, int pageSize, ref int recordCount) where T : new()
        {
            throw new NotImplementedException();
        }

        public DataTable FindTablePage<T>(string WhereSql, DbParameter[] parameters, string orderField, string orderType, int pageIndex, int pageSize, ref int recordCount) where T : new()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 查询数据列表、返回 DataTable
        /// </summary>
        /// <param name="strSql">Sql语句</param>
        /// <param name="orderField">排序字段</param>
        /// <param name="orderType">排序类型</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="recordCount">返回查询条数</param>
        /// <returns></returns>
        public DataTable FindTablePageBySql(string strSql, string orderField, string orderType, int pageIndex, int pageSize, ref int recordCount)
        {
            switch (DbHelper.DbType)
            {
                case DatabaseType.SqlServer:
                    return SqlServerHelper.GetPageTable(strSql, orderField, orderType, pageIndex, pageSize, ref recordCount);
                case DatabaseType.Oracle:
                    return OracleHelper.GetPageTable(strSql, orderField, orderType, pageIndex, pageSize, ref recordCount);
                default:
                    throw new Exception("数据库类型目前不支持！");
            }
        }

        public DataTable FindTablePageBySql(string strSql, DbParameter[] parameters, string orderField, string orderType, int pageIndex, int pageSize, ref int recordCount)
        {
            throw new NotImplementedException();
        }
    }
}
