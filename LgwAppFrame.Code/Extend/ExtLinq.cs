using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LgwAppFrame.Code
{
    public static partial class ExtLinq
    {
        #region 创建一个可访问属性MemberExpression
        /// <summary>
        /// 创建一个可访问属性MemberExpression
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="propertyName"></param>
        /// <example>这里等同于x=>x.propertyName==1中的propertyName</example>
        /// <returns></returns>
        public static Expression Property(this Expression expression, string propertyName)
        {
            return Expression.Property(expression, propertyName);
        }
        #endregion
        #region 依次判断left,如果为真
        /// <summary>
        /// 依次判断left,如果为真
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <example>依次判断left,如果left为真再判断right,如果left为假则不计算right</example>
        /// <returns></returns>
        public static Expression AndAlso(this Expression left, Expression right)
        {
            return Expression.AndAlso(left, right);
        }
        #endregion
        #region 返回MethodCallExpression方法调用表达式
        /// <summary>
        /// 返回MethodCallExpression方法调用表达式
        /// </summary>
        /// <param name="instance">实例</param>
        /// <param name="methodName">方法名</param>
        /// <param name="arguments">参数</param>
        /// <returns></returns>
        public static Expression Call(this Expression instance, string methodName, params Expression[] arguments)
        {
            return Expression.Call(instance, instance.Type.GetMethod(methodName), arguments);
        }
        #endregion
        #region 创建表示"大于"数值比较的 Expression
        /// <summary>
        /// 创建表示"大于"数值比较的 Expression
        /// </summary>
        /// <param name="left">左边</param>
        /// <param name="right">右边</param>
        /// <returns></returns>
        public static Expression GreaterThan(this Expression left, Expression right)
        {
            return Expression.GreaterThan(left, right);
        }
        #endregion
        #region 创建泛型委托Expression
        /// <summary>
        /// 创建泛型委托Expression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="body"></param>
        /// <param name="parameters">表示命名的参数表达式</param>
        /// <returns></returns>
        public static Expression<T> ToLambda<T>(this Expression body, params ParameterExpression[] parameters)
        {
            return Expression.Lambda<T>(body, parameters);
        }
        #endregion
        #region 创建lambda表达式：param=>true
        /// <summary>
        /// 创建lambda表达式：param=>true
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <example>机关函数应用True时：单个AND有效，多个AND有效；单个OR无效，多个OR无效；混应时写在AND后的OR有效</example>
        /// <returns></returns>
        public static Expression<Func<T, bool>> True<T>() { return param => true; }
        #endregion
        #region 创建lambda表达式：param=>false
        /// <summary>
        /// 创建lambda表达式：param=>false
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <example>机关函数应用False时：单个AND无效，多个AND无效；单个OR有效，多个OR有效；混应时写在OR后面的AND有效</example>
        /// <returns></returns>
        public static Expression<Func<T, bool>> False<T>() { return param => false; }
        #endregion
        #region 与组合第一个操作数的计算结果为 true 时才计算第二个操作数的条件AND运算
        /// <summary>
        /// 与 组合第一个操作数的计算结果为 true 时才计算第二个操作数的条件AND运算
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="first">第一个</param>
        /// <param name="second">第二个</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.AndAlso);
        }
        #endregion
        #region 或组合仅第一个操作数的计算结果为 False 时才计算第二个操作数的条件OR运算
        /// <summary>
        /// 或组合仅第一个操作数的计算结果为 False 时才计算第二个操作数的条件OR运算
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.OrElse);
        }
        #endregion
        #region 组合
        /// <summary>
        /// 组合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="first">第一个</param>
        /// <param name="second">第二个</param>
        /// <param name="merge">合并</param>
        /// <returns></returns>
        public static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            var map = first.Parameters //第一个表达式的参数
                .Select((f, i) => new { f, s = second.Parameters[i] })
                .ToDictionary(p => p.s, p => p.f);
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);
            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }
        #endregion
        /// <summary>
        /// 参数重新绑定
        /// </summary>
        private class ParameterRebinder : ExpressionVisitor
        {
            #region 初始化一个新的实例
            readonly Dictionary<ParameterExpression, ParameterExpression> map;
            /// <summary>
            /// 初始化一个新的实例
            /// </summary>
            /// <param name="map"></param>
            ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
            {
                this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
            }
            #endregion 
            #region 取代参数
            /// <summary>
            /// 取代参数
            /// </summary>
            /// <param name="map">The map.</param>
            /// <param name="exp">The exp.</param>
            /// <returns>Expression</returns>
            public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
            {
                return new ParameterRebinder(map).Visit(exp);
            }
            #endregion
            #region 访问参数
            /// <summary>
            /// 访问参数
            /// </summary>
            /// <param name="p">命名的参数表达式</param>
            /// <returns></returns>
            protected override Expression VisitParameter(ParameterExpression p)
            {
                ParameterExpression replacement;

                if (map.TryGetValue(p, out replacement))
                {
                    p = replacement;
                }
                return base.VisitParameter(p);
            }
            #endregion
        }
    }
}
