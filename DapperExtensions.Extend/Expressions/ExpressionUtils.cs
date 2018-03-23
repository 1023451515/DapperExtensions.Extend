using System;
using System.Linq.Expressions;
using System.Reflection;

namespace DapperExtensions.Extend.Expressions
{
    internal static class ExpressionUtils
    {
        /// <summary>
        /// 判断表达式是否是常量表达式
        /// </summary>
        /// <typeparam name="T">表达式参数类型</typeparam>
        /// <typeparam name="TResult">表达式返回类型</typeparam>
        /// <param name="expr">要判断的表达式</param>
        /// <param name="value">常量</param>
        /// <returns>判断结果</returns>
        public static bool IsConstant<T, TResult>(Expression<Func<T, TResult>> expr, TResult value)
        {
            if (!(expr.Body is ConstantExpression constant))
                return false;
            return constant.Value.Equals(value);
        }

        /// <summary>
        /// 获取表达式的成员属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static string GetProperty<T>(Expression<Func<T, object>> expression)
        {
            if (expression == null) { return null; }
            var propertyInfo = ReflectionHelper.GetProperty(expression) as PropertyInfo;
            return propertyInfo == null ? null : propertyInfo.Name;
        }
    }
}
