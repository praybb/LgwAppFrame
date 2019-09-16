using LgwAppFrame.Code;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;

namespace LgwAppFrame.EFDate
{
    /// <summary>
    /// 多模型 仓储接口
    /// </summary>
    /// <remarks>继承IDisposable实现自动释放这样才能使用using()</remarks>>
    public interface IRepositoryBase : IDisposable
    {
        #region 开始事务
        /// <summary>
        /// 开启事务
        /// </summary>
        /// <returns></returns>
        IRepositoryBase BeginTrans();
        #endregion
        #region 提交数据
        /// <summary>
        /// 提交数据
        /// </summary>
        /// <returns></returns>
        int Commit();
        #endregion
        #region 插入实体
        /// <summary>
        /// 插入实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Insert<TEntity>(TEntity entity) where TEntity : class;
        #endregion
        #region 插入实体集
        /// <summary>
        /// 插入实体集
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entitys"></param>
        /// <returns></returns>
        int Insert<TEntity>(List<TEntity> entitys) where TEntity : class;
        #endregion
        #region 修改实体
        /// <summary>
        /// 修改实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Update<TEntity>(TEntity entity) where TEntity : class;
        #endregion
        #region 删除实体
        /// <summary>
        /// 删除实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Delete<TEntity>(TEntity entity) where TEntity : class;
        #endregion
        #region 删除指定表达式树的实体
        /// <summary>
        /// 删除指定表达式树的实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        int Delete<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;
        #endregion
        #region 查询指定主键值的实体
        /// <summary>
        /// 查询指定主键值的实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        TEntity FindEntity<TEntity>(object keyValue) where TEntity : class;
        #endregion
        #region 查询指定表达式树的实体
        /// <summary>
        /// 查询指定表达式树的实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        TEntity FindEntity<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;
        #endregion
        #region 延时获取实体
        /// <summary>
        /// 延时获取实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        IQueryable<TEntity> IQueryable<TEntity>() where TEntity : class;
        #endregion
        #region 延时获取指定表达式树的实体
        /// <summary>
        /// 延时获取指定表达式树的实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IQueryable<TEntity> IQueryable<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;
        #endregion
        #region 通过SQL查询结果返回实体集
        /// <summary>
        /// 通过SQL查询结果返回实体集
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="strSql"></param>
        /// <returns></returns>
        List<TEntity> FindList<TEntity>(string strSql) where TEntity : class;
        #endregion
        #region 通过SQL与参数查询实体返回实体集
        /// <summary>
        /// 通过SQL与参数查询实体返回实体集
        /// </summary>
        /// <typeparam name="TEntity">泛实体</typeparam>
        /// <param name="strSql">SQL语句</param>
        /// <param name="dbParameter">DbCommand 的参数</param>
        /// <returns></returns>
        List<TEntity> FindList<TEntity>(string strSql, DbParameter[] dbParameter) where TEntity : class;
        #endregion
        #region 通过查询实体并分页返回实体集
        /// <summary>
        /// 通过查询实体并分页返回实体集
        /// </summary>
        /// <typeparam name="TEntity">泛实体</typeparam>
        /// <param name="pagination">分页类</param>
        /// <returns></returns>
        List<TEntity> FindList<TEntity>(Pagination pagination) where TEntity : class, new();
        #endregion
        #region 通过树表达式查询并分页结果返回实体集
        /// <summary>
        /// 通过树表达式查询并分页结果返回实体集
        /// </summary>
        /// <typeparam name="TEntity">泛实体</typeparam>
        /// <param name="predicate">树表达式</param>
        /// <param name="pagination">分页类</param>
        /// <returns></returns>
        List<TEntity> FindList<TEntity>(Expression<Func<TEntity, bool>> predicate, Pagination pagination) where TEntity : class, new();
        #endregion
    }
}
