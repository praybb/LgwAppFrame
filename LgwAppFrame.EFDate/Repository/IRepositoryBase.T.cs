using LgwAppFrame.Code;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;

namespace LgwAppFrame.EFDate
{
    /// <summary>
    /// 单个实体仓储接口-定义仓储模型中的数据标准操作
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public interface IRepositoryBase<TEntity> where TEntity : class, new()
    {
        #region 插入实体
        /// <summary>
        /// 插入实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Insert(TEntity entity);
        #endregion
        #region 插入实体集
        /// <summary>
        /// 插入实体集
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        int Insert(List<TEntity> entitys);
        #endregion
        #region 修改实体
        /// <summary>
        /// 修改实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Update(TEntity entity);
        #endregion
        #region 删除实体
        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Delete(TEntity entity);
        #endregion
        #region 删除指定表达树的实体
        /// <summary>
        /// 删除指定表达树的实体
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        int Delete(Expression<Func<TEntity, bool>> predicate);
        #endregion
        #region 查询指定主键值的实体
        /// <summary>
        /// 查询指定主键值的实体
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        TEntity FindEntity(object keyValue);
        #endregion
        #region 查询指定表达式树的实体
        /// <summary>
        /// 查询指定表达式树的实体
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        TEntity FindEntity(Expression<Func<TEntity, bool>> predicate);
        #endregion
        #region 延时获取实体集
        /// <summary>
        /// 延时获取实体
        /// </summary>
        /// <returns>实体集</returns>
        IQueryable<TEntity> IQueryable();
        #endregion
        #region 延时获取指定表达式树的实体
        /// <summary>
        /// 延时获取指定表达式树的实体
        /// </summary>
        /// <param name="predicate">表达式</param>
        /// <returns></returns>
        IQueryable<TEntity> IQueryable(Expression<Func<TEntity, bool>> predicate);
        #endregion
        #region 通过SQL查询结果返回实体集
        /// <summary>
        /// 通过SQL查询结果返回实体集
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        List<TEntity> FindList(string strSql);
        #endregion
        #region 通过SQL与参数查询实体返回实体集
        /// <summary>
        /// 通过SQL与参数查询实体返回实体集
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="dbParameter"></param>
        /// <returns></returns>
        List<TEntity> FindList(string strSql, DbParameter[] dbParameter);
        #endregion
        #region 通过查询实体并分页返回实体集
        /// <summary>
        /// 通过查询实体并分页返回实体集
        /// </summary>
        /// <param name="pagination">分页类</param>
        /// <returns></returns>
        List<TEntity> FindList(Pagination pagination);
        #endregion
        #region 通过树表达式查询并分页结果返回实体集
        /// <summary>
        /// 通过树表达式查询并分页结果返回实体集
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        List<TEntity> FindList(Expression<Func<TEntity, bool>> predicate, Pagination pagination);
        #endregion
    }
}
