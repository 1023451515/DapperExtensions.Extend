using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DapperExtensions.Extend.Contexts;

namespace DapperExtensions.Extend.Expressions
{
    internal static class SortingEx
    {
        /// <summary>
        /// 排序组转成DapperExtensions的ISort集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sorts"></param>
        /// <returns></returns>
        public static IList<ISort> ToSortable<T>(this IEnumerable<Sorting<T>> sorts) where T : class
        {
            var sortList = new List<ISort>();
            if (sorts != null)
            {
                var enumerable = sorts as IList<Sorting<T>> ?? sorts.ToList();
                if (!enumerable.Any())
                {
                    return sortList;
                }
                foreach (var sort in enumerable)
                {
                    var sortProperty = ReflectionHelper.GetProperty(sort.Parameter);
                    sortList.Add(new Sort
                    {
                        Ascending = sort.Direction != SortType.Desc,
                        PropertyName = sortProperty.Name
                    });
                }
                return sortList;
            }
        }
    }

}
