using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DapperExtensions.Mapper;
using DapperExtensions.Sql;

namespace DapperExtensions.Extend.Contexts
{
    internal class SqlBuilder : SqlGeneratorImpl
    {
        /// <summary>
        /// DapperExtensions配置
        /// </summary>
        readonly IDapperExtensionsConfiguration _configuration;

        public SqlBuilder(IDapperExtensionsConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// 获取查询单条语句的SQL
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        public string SelectSingle<T>(string primaryKey) where T : class
        {
            var classMap = _configuration.GetMap<T>();
            var top = "";
            if (_configuration.Dialect is SqlServerDialect)
            {
                top = " Top 1 ";
            }
            var sql = new StringBuilder(string.Format("SELECT {2} {0} FROM {1}",
                BuildSelectColumns(classMap),
                GetTableName(classMap), top));
            sql.Append(" WHERE ");
            sql.AppendFormat(" {0}={1} ", _configuration.Dialect.ParameterPrefix + primaryKey, "@" + primaryKey);
            if (_configuration.Dialect is MySqlDialect)
            {
                sql.Append(" Limit 1; ");
            }
            return sql.ToString();
        }

        /// <summary>
        /// 生成添加语句SQL
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="pk"></param>
        /// <returns></returns>
        public string Insert<T>(T entity, string pk) where T : class
        {
            var classMap = _configuration.GetMap<T>();
            var columns = classMap.Properties;
            if (!string.IsNullOrEmpty(pk))
            {
                columns = columns.Where(x => x.ColumnName != pk).ToList();
            }
            if (!columns.Any())
            {
                throw new ArgumentException("No columns were mapped.");
            }
            var columnNames = columns.Select(p => GetColumnName(classMap, p, false));
            var parameters = columns.Select(p => _configuration.Dialect.ParameterPrefix + p.Name);
            var sql =
                $"INSERT INTO {GetTableName(classMap)} ({columnNames.AppendStrings()}) VALUES ({parameters.AppendStrings()})";
            return sql;
        }

        /// <summary>
        /// 生成更新语句SQL
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">where条件</param>
        /// <param name="where">where字段字典</param>
        /// <param name="set">set字段</param>
        /// <returns></returns>
        public string Update<T>(IPredicate predicate, IDictionary<string, object> where, IEnumerable<string> set) where T : class
        {
            var classMap = _configuration.GetMap<T>();
            if (predicate == null || where == null || set == null)
            {
                throw new ArgumentNullException(nameof(predicate) + "||" + nameof(where) + nameof(set));
            }
            var enumerable = set as string[] ?? set.ToArray();
            if (!enumerable.Any())
            {
                throw new ArgumentException("No columns.");
            }
            var setSql = enumerable.Select(p =>
                $"{GetColumnName(classMap, p, false)} = {Configuration.Dialect.ParameterPrefix}{p}");
            return
                $"UPDATE {GetTableName(classMap)} SET {setSql.AppendStrings()} WHERE {predicate.GetSql(this, @where)}";
        }

        /// <summary>
        /// 生成分页SQL
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="predicate"></param>
        /// <param name="sort"></param>
        /// <param name="page"></param>
        /// <param name="resultsPerPage"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string GetPaging(Type entityType, IPredicate predicate, IList<ISort> sort, int page, int resultsPerPage, out IDictionary<string, object> parameters)
        {
            var classMap = _configuration.GetMap(entityType);
            parameters = new Dictionary<string, object>();
            var generator = SqlFactory.GetSqlGenerator(_configuration);
            return generator.SelectPaged(classMap, predicate, sort, page, resultsPerPage, parameters);
        }

        /// <summary>
        /// 生成获取列表的SQL
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="predicate"></param>
        /// <param name="sort"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string GetList(Type entityType, IPredicate predicate, IList<ISort> sort, out IDictionary<string, object> parameters)
        {
            var classMap = _configuration.GetMap(entityType);
            parameters = new Dictionary<string, object>();
            var generator = SqlFactory.GetSqlGenerator(_configuration);
            return generator.Select(classMap, predicate, sort, parameters);
        }
    }
}
