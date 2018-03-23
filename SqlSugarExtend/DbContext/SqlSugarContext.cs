#define DEBUG
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Data;

namespace App.Infrastructure
{
    public class SqlSugarContext : IDbContext
    {
        SqlSugarClient _db;
        public SqlSugarContext(string connstr, DbType dbType = DbType.Mssql)
        {
            var _dbType = string.Empty;
            switch (dbType)
            {
                case DbType.Mysql:
                    _dbType = SqlSugar.DbType.MySql;
                    break;
                case DbType.Mssql:
                default:
                    _dbType = SqlSugar.DbType.SqlServer;
                    break;
            }
            _db = new SqlSugarClient(new ConnectionConfig() { ConnectionString = connstr, DbType = _dbType });
#if DEBUG
            _db.Ado.IsEnableLogEvent = true;
            _db.Ado.LogEventStarting = (sql, pars) =>
            {
                System.Diagnostics.Debug.Write(sql);
            };
#endif

        }

        #region 单表操作
        public int Count<T>(Expression<Func<T, bool>> predicate) where T : class, new()
        {
            return _db.Queryable<T>().Where(predicate).Count();
        }

        public T Get<T, TValue>(Expression<Func<T, object>> pk, TValue value)
            where T : class, new()
        where TValue : struct
        {
            var filed = ExpressionUtils.GetProperty(pk);
            SugarParameter parameter = new SugarParameter(filed, value);
            return _db.Queryable<T>().Where(string.Concat(filed, "=@", filed), parameter).Single();
        }

        public T Get<T>(Expression<Func<T, bool>> predicate, params Sorting<T>[] sorts) where T : class, new()
        {
            ISugarQueryable<T> query = _db.Queryable<T>();
            if (sorts != null)
            {
                foreach (var sort in sorts)
                {
                    query = query.OrderBy(sort.Parameter, sort.Direction == SortType.Asc ? OrderByType.Asc : OrderByType.Desc);
                }
            }
            return query.Single(predicate);
        }

        public IEnumerable<T> GetList<T>(Expression<Func<T, bool>> predicate, params Sorting<T>[] sorts) where T : class, new()
        {
            ISugarQueryable<T> query = _db.Queryable<T>().Where(predicate);
            if (sorts != null)
            {
                foreach (var sort in sorts)
                {
                    query = query.OrderBy(sort.Parameter, sort.Direction == SortType.Asc ? OrderByType.Asc : OrderByType.Desc);
                }
            }
            return query.ToList();
        }

        public IEnumerable<T> GetPageList<T>(Expression<Func<T, bool>> predicate, int page, int resultsPerPage, Sorting<T>[] sorts, ref int total) where T : class, new()
        {
            ISugarQueryable<T> query = _db.Queryable<T>().Where(predicate);
            if (sorts != null)
            {
                foreach (var sort in sorts)
                {
                    query = query.OrderBy(sort.Parameter, sort.Direction == SortType.Asc ? OrderByType.Asc : OrderByType.Desc);
                }
            }
            return query.ToPageList(page, resultsPerPage, ref total);
        }

        public int Insert<T>(T entity, Expression<Func<T, object>> pk = null, bool isIdentity = true) where T : class, new()
        {
            IInsertable<T> insertObj = _db.Insertable(entity);
            if (pk != null)
            {
                insertObj = insertObj.IgnoreColumns(pk);
            }
            if (isIdentity)
            {
                return insertObj.ExecuteReturnIdentity();
            }
            return insertObj.ExecuteCommand();
        }

        public int Update<T>(T entity, Expression<Func<T, object>> fileds) where T : class, new()
        {
            return _db.Updateable(entity).UpdateColumns(fileds).ExecuteCommand();
        }
        #endregion

        #region Ado操作
        public IEnumerable<TReturn> SqlQuery<TReturn>(string sql, IDictionary<string, object> dic = null) where TReturn : class, new()
        {
            return _db.Ado.SqlQuery<TReturn>(sql, dic);
        }

        public TReturn SqlQuerySingle<TReturn>(string sql, IDictionary<string, object> dic = null) where TReturn : class, new()
        {
            return _db.Ado.SqlQuerySingle<TReturn>(sql, dic);
        }

        public DataSet GetDataSet(string sql, IDictionary<string, object> dic = null)
        {
            return _db.Ado.GetDataSetAll(sql, dic);
        }

        public DataTable GetDataTable(string sql, IDictionary<string, object> dic = null)
        {
            return _db.Ado.GetDataTable(sql, dic);
        }

        public int ExecuteSql(string sql, IDictionary<string, object> dic = null, bool isTran = false)
        {
            if (isTran)
            {
                var result = _db.Ado.UseTran(() =>
                 {
                     return _db.Ado.ExecuteCommand(sql, dic);
                 });
                return result.IsSuccess ? result.Data : 0;
            }
            return _db.Ado.ExecuteCommand(sql, dic);
        }


        public int GetInt(string sql, IDictionary<string, object> dic = null)
        {
            return _db.Ado.GetInt(sql, dic);
        }

        public string GetString(string sql, IDictionary<string, object> dic = null)
        {
            return _db.Ado.GetString(sql, dic);
        }
        #endregion

        ~SqlSugarContext()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (_db != null)
            {
                _db.Dispose();
            }
        }

     
    }
}
