using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace FAST3_Repository
{
    /// <summary>
    /// 定义通用的Repository
    /// </summary>
    /// <typeparam name="T">定义泛型，约束其是一个类</typeparam>
    public class Repository<T> : IRepository<T> where T : new()
    {
        #region 事务及数据库连接(使用了lambda表达式)
        /// <summary>
        /// 事务开始
        /// </summary>
        /// <returns></returns>
        public DbTransaction BeginTrans()
            => DataFactory.DataBase().BeginTrans();

        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public void Close()
            => DataFactory.DataBase().Close();

        /// <summary>
        /// 提交事务
        /// </summary>
        public void Commit()
            => DataFactory.DataBase().Commit();

        /// <summary>
        /// 回滚事务
        /// </summary>
        public void Rollback()
            => DataFactory.DataBase().Rollback();
        #endregion

        /// <summary>
        /// 大批量数据导入
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public bool BulkInsert(DataTable dataTable)
            => DataFactory.DataBase().BulkInsert(dataTable);

        #region 删除数据Delete
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public int Delete(T entity)
            => DataFactory.DataBase().Delete(entity);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="propertyValue">主键值</param>
        /// <returns></returns>
        public int Delete(object propertyValue)
        {
            return DataFactory.DataBase().Delete<T>(propertyValue);
        }
        /// <summary>
        /// 批量删除数据
        /// </summary>
        /// <param name="propertyValue">主键值</param>
        /// <returns></returns>
        public int Delete(object[] propertyValue)
        {
            return DataFactory.DataBase().Delete<T>(propertyValue);
        }
        #endregion

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <returns></returns>
        public int ExecuteByProc(string procName)
            => DataFactory.DataBase().ExecuteByProc(procName);

        /// <summary>
        /// 执行Sql语句
        /// </summary>
        /// <param name="strSql">Sql语句</param>
        /// <returns></returns>
        public int ExecuteBySql(StringBuilder strSql)
            => DataFactory.DataBase().ExecuteBySql(strSql);

        /// <summary>
        /// 查询对象、返回实体
        /// </summary>
        /// <param name="propertyValue">主键值</param>
        /// <returns></returns>
        public T FindEntity(object propertyValue)
        {
            return DataFactory.DataBase().FindEntity<T>(propertyValue);
        }

        public T FindEntityByWhere(string WhereSql)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 查询对象、返回实体
        /// </summary>
        /// <param name="propertyName">实体属性名称</param>
        /// <param name="propertyValue">字段值</param>
        /// <returns></returns>
        public T FindEntity(string propertyName, object propertyValue)
        {
            return DataFactory.DataBase().FindEntity<T>(propertyName, propertyValue);
        }

        public List<T> FindListTop(int Top)
        {
            throw new NotImplementedException();
        }

        public List<T> FindListTop(int Top, string WhereSql)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 查询数据列表、返回List
        /// </summary>
        /// <param name="propertyName">实体属性名称</param>
        /// <param name="propertyValue">字段值</param>
        /// <returns></returns>
        public List<T> FindList(string propertyName, string propertyValue)
        {
            return DataFactory.DataBase().FindList<T>(propertyName, propertyValue);
        }

        /// <summary>
        /// 查询数据列表、返回List
        /// </summary>
        /// <param name="Top">显示条数</param>
        /// <param name="propertyName">实体属性名称</param>
        /// <param name="propertyValue">字段值</param>
        /// <returns></returns>
        public List<T> FindListTop(int Top, string propertyName, string propertyValue)
        {
            return DataFactory.DataBase().FindListTop<T>(Top, propertyName, propertyValue);
        }

        public object FindMaxBySql(string strSql)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 查询数据列表，返回DataTable
        /// </summary>
        /// <param name="WhereSql"></param>
        /// <returns></returns>
        public DataTable FindTable(string WhereSql)
        {
            return DataFactory.DataBase().FindTableBySql(WhereSql);
        }

        public DataTable FindTableTop(int Top)
        {
            throw new NotImplementedException();
        }

        public DataTable FindTableTop(int Top, string WhereSql)
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
            return DataFactory.DataBase().FindTableBySql(strSql);
        }

        /// <summary>
        /// 查询数据、返回条数
        /// </summary>
        /// <param name="strSql">Sql语句</param>
        /// <returns></returns>
        public int FindCountBySql(string strSql)
        {
            return DataFactory.DataBase().FindCountBySql(strSql);
        }

        public int Insert(T entity)
        {
            return DataFactory.DataBase().Insert(entity);
        }

        public int Insert(List<T> entity)
        {
            return DataFactory.DataBase().Insert<T>(entity);
        }

        public int Update(T entity)
        {
            return DataFactory.DataBase().Update(entity);
        }

        public int Update(string propertyName, string propertyValue)
        {
            throw new NotImplementedException();
        }

        public int Update(List<T> entity)
        {
            throw new NotImplementedException();
        }
    }
}
