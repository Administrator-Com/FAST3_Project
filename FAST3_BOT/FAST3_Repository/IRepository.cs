using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace FAST3_Repository
{
    /// <summary>
    /// 定义通用的Repository接口
    /// </summary>
    public interface IRepository<T> where T : new()
    {
        #region 事务管理
        /// <summary>
        /// 事务开始
        /// </summary>
        /// <returns></returns>
        DbTransaction BeginTrans();
        /// <summary>
        /// 提交事务
        /// </summary>
        void Commit();
        /// <summary>
        /// 回滚事务
        /// </summary>
        void Rollback();
        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        void Close();
        #endregion

        /// <summary>
        /// 大批量数据插入
        /// </summary>
        /// <param name="dataTable">资料表</param>
        /// <returns></returns>
        bool BulkInsert(DataTable dataTable);

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        int ExecuteBySql(StringBuilder strSql);

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procName"></param>
        /// <returns></returns>
        int ExecuteByProc(string procName);

        #region 插入数据Insert
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">实体类对象</param>
        /// <returns></returns>
        int Insert(T entity);

        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="entity">实体类对象</param>
        /// <returns></returns>
        int Insert(List<T> entity);
        #endregion

        #region 修改数据Update
        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        int Update(T entity);

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="propertyName">实体属性名称</param>
        /// <param name="propertyValue">字段值</param>
        /// <returns></returns>
        int Update(string propertyName, string propertyValue);

        /// <summary>
        /// 批量修改数据
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        int Update(List<T> entity);
        #endregion

        #region 删除数据Delete
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        int Delete(T entity);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="propertyValue">主键值</param>
        /// <returns></returns>
        int Delete(object propertyValue);
        /// <summary>
        /// 批量删除数据
        /// </summary>
        /// <param name="propertyValue">主键值</param>
        /// <returns></returns>
        int Delete(object[] propertyValue);
        #endregion

        #region 查询数据列表、返回List
        /// <summary>
        /// 查询数据列表、返回List
        /// </summary>
        /// <param name="Top">显示条数</param>
        /// <returns></returns>
        List<T> FindListTop(int Top);

        /// <summary>
        /// 查询数据列表、返回List
        /// </summary>
        /// <param name="Top">显示条数</param>
        /// <param name="WhereSql">条件</param>
        /// <returns></returns>
        List<T> FindListTop(int Top, string WhereSql);

        /// <summary>
        /// 查询数据列表、返回List
        /// </summary>
        /// <param name="Top">显示条数</param>
        /// <param name="propertyName">实体属性名称</param>
        /// <param name="propertyValue">字段值</param>
        /// <returns></returns>
        List<T> FindListTop(int Top, string propertyName, string propertyValue);

        List<T> FindList(string propertyName, string propertyValue);
        #endregion

        #region 查询数据列表、返回 DataTable
        /// <summary>
        /// 查询数据列表、返回 DataTable
        /// </summary>
        /// <param name="Top">显示条数</param>
        /// <returns></returns>
        DataTable FindTableTop(int Top);

        /// <summary>
        /// 查询数据列表、返回 DataTable
        /// </summary>
        /// <param name="Top">显示条数</param>
        /// <param name="WhereSql">条件</param>
        /// <returns></returns>
        DataTable FindTableTop(int Top, string WhereSql);

        /// <summary>
        /// 查询数据列表、返回 DataTable
        /// </summary>
        /// <param name="WhereSql">条件</param>
        /// <returns></returns>
        DataTable FindTable(string WhereSql);

        /// <summary>
        /// 查询数据列表、返回 DataTable
        /// </summary>
        /// <param name="strSql">Sql语句</param>
        /// <returns></returns>
        DataTable FindTableBySql(string strSql);
        
        #endregion

        /// <summary>
        /// 查询对象、返回实体
        /// </summary>
        /// <param name="propertyValue">主键值</param>
        /// <returns></returns>
        T FindEntity(object propertyValue);

        /// <summary>
        /// 查询对象、返回实体
        /// </summary>
        /// <param name="WhereSql">条件</param>
        /// <returns></returns>
        T FindEntityByWhere(string WhereSql);

        /// <summary>
        /// 查询对象、返回实体
        /// </summary>
        /// <param name="propertyName">实体属性名称</param>
        /// <param name="propertyValue">字段值</param>
        /// <returns></returns>
        T FindEntity(string propertyName, object propertyValue);

        /// <summary>
        /// 查询数据、返回条数
        /// </summary>
        /// <param name="strSql">Sql语句</param>
        /// <returns></returns>
        int FindCountBySql(string strSql);

        /// <summary>
        /// 查询数据、返回最大数
        /// </summary>
        /// <param name="strSql">Sql语句</param>
        /// <returns></returns>
        object FindMaxBySql(string strSql);
    }
}
