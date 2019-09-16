using LgwAppFrame.Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace LgwAppFrame.EFDate
{
    /// <summary>
    /// 多个实体的仓储实现
    /// </summary>
    /// <remarks>多个实体时可以使用此类</remarks>
    /// <example>
    /* using (var db = new RepositoryBase().BeginTrans())
    {    foreach (var item in entitys){ 
     db.Insert(item); 
     }
     db.Commit();
     }*/
    /// </example>
    public class RepositoryBase : IRepositoryBase, IDisposable
    {
        #region 实例化数据库上下文
        /// <summary>
        /// 实例化数据库上下文
        /// </summary>
        private FEDbContext dbcontext = new FEDbContext();
        #endregion
        #region 数据库事务
        /// <summary>
        /// 数据库事务
        /// </summary>
        private DbTransaction dbTransaction { get; set; }
        #endregion
        #region 开始事务
        /// <summary>
        /// 开启事务
        /// </summary>
        /// <returns></returns>
        public IRepositoryBase BeginTrans()
        {
            //获取对象上下文使用的连接。
            DbConnection dbConnection = ((IObjectContextAdapter)dbcontext).ObjectContext.Connection;
            if (dbConnection.State == ConnectionState.Closed) //如果是关闭
            {
                dbConnection.Open();//打开
            }
            dbTransaction = dbConnection.BeginTransaction(); //启动事务
            return this;
        }
        #endregion
        #region 提交数据
        /// <summary>
        /// 提交数据
        /// </summary>
        /// <returns></returns>
        public int Commit()
        {
            try
            {
                var returnValue = dbcontext.SaveChanges();
                if (dbTransaction != null)
                {
                    dbTransaction.Commit();
                }
                return returnValue;
            }
            catch (Exception)
            {
                if (dbTransaction != null)
                {
                    this.dbTransaction.Rollback();
                }
                throw;
            }
            finally
            {
                this.Dispose();
            }
        }
        #endregion
        #region 托管释放资源
        public void Dispose()
        {
            if (dbTransaction != null)
            {
                this.dbTransaction.Dispose();
            }
            this.dbcontext.Dispose();
        }
        #endregion
        #region  插入实体
        public int Insert<TEntity>(TEntity entity) where TEntity : class
        {
            dbcontext.Entry<TEntity>(entity).State = EntityState.Added;
            return dbTransaction == null ? this.Commit() : 0;
        }
        #endregion
        #region 插入实体集
        /// <summary>
        /// 插入实体集
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public int Insert<TEntity>(List<TEntity> entitys) where TEntity : class
        {
            foreach (var entity in entitys)
            {
                dbcontext.Entry<TEntity>(entity).State = EntityState.Added;
            }
            return dbTransaction == null ? this.Commit() : 0;
        }
        #endregion
        #region 修改实体
        /// <summary>
        /// 修改实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Update<TEntity>(TEntity entity) where TEntity : class
        {
            dbcontext.Set<TEntity>().Attach(entity);
            PropertyInfo[] props = entity.GetType().GetProperties();
            foreach (PropertyInfo prop in props)
            {
                if (prop.GetValue(entity, null) != null)
                {
                    if (prop.GetValue(entity, null).ToString() == "&nbsp;")
                        dbcontext.Entry(entity).Property(prop.Name).CurrentValue = null;
                    dbcontext.Entry(entity).Property(prop.Name).IsModified = true;
                }
            }
            return dbTransaction == null ? this.Commit() : 0;
        }
        #endregion
        #region 删除实体
        /// <summary>
        /// 删除指定表达式树的实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Delete<TEntity>(TEntity entity) where TEntity : class
        {
            dbcontext.Set<TEntity>().Attach(entity);
            dbcontext.Entry<TEntity>(entity).State = EntityState.Deleted;
            return dbTransaction == null ? this.Commit() : 0;
        }
        #endregion
        #region 删除指定表达式树的实体
        /// <summary>
        /// 删除指定表达式树的实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public int Delete<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            var entitys = dbcontext.Set<TEntity>().Where(predicate).ToList();
            entitys.ForEach(m => dbcontext.Entry<TEntity>(m).State = EntityState.Deleted);
            return dbTransaction == null ? this.Commit() : 0;
        }
        #endregion
        #region 查询指定主键值的实体
        /// <summary>
        /// 查询指定主键值的实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public TEntity FindEntity<TEntity>(object keyValue) where TEntity : class
        {
            return dbcontext.Set<TEntity>().Find(keyValue);
        }
        #endregion
        #region 查询指定表达式树的实体
        /// <summary>
        /// 查询指定表达式树的实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public TEntity FindEntity<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return dbcontext.Set<TEntity>().FirstOrDefault(predicate);
        }
        #endregion
        #region 延时获取实体
        /// <summary>
        /// 延时获取实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public IQueryable<TEntity> IQueryable<TEntity>() where TEntity : class
        {
            return dbcontext.Set<TEntity>();
        }
        #endregion
        #region 延时获取指定表达式树的实体
        /// <summary>
        /// 延时获取指定表达式树的实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IQueryable<TEntity> IQueryable<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return dbcontext.Set<TEntity>().Where(predicate);
        }
        #endregion
        #region 通过SQL查询结果返回实体集
        /// <summary>
        /// 通过SQL查询结果返回实体集
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public List<TEntity> FindList<TEntity>(string strSql) where TEntity : class
        {
            return dbcontext.Database.SqlQuery<TEntity>(strSql).ToList<TEntity>();
        }
        #endregion
        #region 通过SQL与参数查询实体返回实体集
        /// <summary>
        /// 通过SQL与参数查询实体返回实体集
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="strSql"></param>
        /// <param name="dbParameter"></param>
        /// <returns></returns>
        public List<TEntity> FindList<TEntity>(string strSql, DbParameter[] dbParameter) where TEntity : class
        {
            return dbcontext.Database.SqlQuery<TEntity>(strSql, dbParameter).ToList<TEntity>();
        }
        #endregion
        #region 通过查询实体并分页返回实体集
        /// <summary>
        /// 通过查询实体并分页返回实体集
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public List<TEntity> FindList<TEntity>(Pagination pagination) where TEntity : class, new()
        {
            bool isAsc = pagination.sord.ToLower() == "asc" ? true : false;//asc升序否则降序
            string[] _order = pagination.sidx.Split(',');//排序列
            MethodCallExpression resultExp = null;            
            var tempData = dbcontext.Set<TEntity>().AsQueryable(); //表示实体延时加载
            foreach (string item in _order)
            {
                string _orderPart = item;
                _orderPart = Regex.Replace(_orderPart, @"\s+", " ");//正则去除空格与换行等
                string[] _orderArry = _orderPart.Split(' ');//空格分隔字符串
                string _orderField = _orderArry[0];//排序的列属性
                bool sort = isAsc;
                if (_orderArry.Length == 2) //判断空格后是否带有排序类型
                {
                    isAsc = _orderArry[1].ToUpper() == "ASC" ? true : false;
                }
                ParameterExpression parameter = Expression.Parameter(typeof(TEntity), "t");//创建输入参数,参数名为t，类型为泛实体类型(注:这里的t相对应该树表达式的t=>t.xx)
                PropertyInfo property = typeof(TEntity).GetProperty(_orderField);//反射获取实体相对应名称的属性
                MemberExpression propertyAccess = Expression.MakeMemberAccess(parameter, property);
                LambdaExpression orderByExp = Expression.Lambda(propertyAccess, parameter);
                resultExp = Expression.Call(
                    typeof(Queryable), 
                    isAsc ? "OrderBy" : "OrderByDescending",
                    new Type[] { typeof(TEntity), property.PropertyType },
                    tempData.Expression, 
                    Expression.Quote(orderByExp)
                    );
            }
            tempData = tempData.Provider.CreateQuery<TEntity>(resultExp);
            pagination.records = tempData.Count();
            tempData = tempData.Skip<TEntity>(pagination.rows * (pagination.page - 1)).Take<TEntity>(pagination.rows).AsQueryable();//返回指定页的数据
            return tempData.ToList();
        }
        #endregion
        #region 通过树表达式查询并分页结果返回实体集
        /// <summary>
        /// 通过树表达式查询并分页结果返回实体集
        /// </summary>
        /// <typeparam name="TEntity">实体</typeparam>
        /// <param name="predicate">树表达式</param>
        /// <param name="pagination">分页</param>
        /// <returns></returns>
        public List<TEntity> FindList<TEntity>(Expression<Func<TEntity, bool>> predicate, Pagination pagination) where TEntity : class, new()
        {
            bool isAsc = pagination.sord.ToLower() == "asc" ? true : false;
            string[] _order = pagination.sidx.Split(',');
            MethodCallExpression resultExp = null;
            var tempData = dbcontext.Set<TEntity>().Where(predicate);
            foreach (string item in _order)
            {
                string _orderPart = item;
                _orderPart = Regex.Replace(_orderPart, @"\s+", " ");
                string[] _orderArry = _orderPart.Split(' ');
                string _orderField = _orderArry[0];
                bool sort = isAsc;
                if (_orderArry.Length == 2)
                {
                    isAsc = _orderArry[1].ToUpper() == "ASC" ? true : false;
                }
                var parameter = Expression.Parameter(typeof(TEntity), "t");
                var property = typeof(TEntity).GetProperty(_orderField);
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExp = Expression.Lambda(propertyAccess, parameter);
                resultExp = Expression.Call(typeof(Queryable), isAsc ? "OrderBy" : "OrderByDescending", new Type[] { typeof(TEntity), property.PropertyType }, tempData.Expression, Expression.Quote(orderByExp));
            }
            tempData = tempData.Provider.CreateQuery<TEntity>(resultExp);
            pagination.records = tempData.Count();
            tempData = tempData.Skip<TEntity>(pagination.rows * (pagination.page - 1)).Take<TEntity>(pagination.rows).AsQueryable();
            return tempData.ToList();
        }
        #endregion
    }
}
