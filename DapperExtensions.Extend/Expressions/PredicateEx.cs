using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Dapper;
using DapperExtensions.Extend.Contexts;

namespace DapperExtensions.Extend.Expressions
{
    internal static class PredicateEx
    {
        /// <summary>
        /// 表达式转成谓词组
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static IPredicate ToPredicateGroup<TEntity>(this Expression<Func<TEntity, bool>> expression) where TEntity : class
        {
            if (expression == null)
            {
                return null;
            }
            var pg = QueryBuilder<TEntity>.FromExpression(expression);
            return pg;
        }
        /// <summary>
        /// 谓词组转换成字典
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="group"></param>
        /// <returns></returns>
        public static IDictionary<string, object> ToPropertyParam<TEntity>(this IPredicate groups) where TEntity : class
        {
            var dics = new Dictionary<string, object>();
            switch (groups)
            {
                case PredicateGroup pg:
                    foreach (var p in pg.Predicates)
                    {
                        if (p is FieldPredicate<TEntity> f) dics.Add(f.PropertyName, f.Value);
                    }
                    break;
                case FieldPredicate<TEntity> f:
                    dics.Add(f.PropertyName, f.Value);
                    break;
            }

            return dics;
        }

        /// <summary>
        /// 字段转成字典
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="filed"></param>
        /// <returns></returns>
        public static IDictionary<string, object> ToPropertyParam<TEntity>(this DbFiled<TEntity> filed) where TEntity : class
        {
            var property = ExpressionUtils.GetProperty(filed.Name);
            return new Dictionary<string, object> { { property, filed.Value } };
        }
        /// <summary>
        /// 字段组转成字典
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="fileds"></param>
        /// <returns></returns>
        public static IDictionary<string, object> ToPropertyParam<TEntity>(this DbFiled<TEntity>[] fileds) where TEntity : class
        {
            IDictionary<string, object> param = new Dictionary<string, object>();
            foreach (var filed in fileds)
            {
                var property = ExpressionUtils.GetProperty(filed.Name);
                if (!param.ContainsKey(property))
                {
                    param.Add(property, filed.Value);
                }
            }
            return param;
        }
        /// <summary>
        /// 字典转成动态参数
        /// </summary>
        /// <param name="dics"></param>
        /// <returns></returns>
        public static DynamicParameters ToDynamicParameters(this IDictionary<string, object> dics)
        {
            var param = new DynamicParameters();
            dics.AsList().ForEach(x => param.Add(x.Key, x.Value));
            return param;
        }
    }
}
