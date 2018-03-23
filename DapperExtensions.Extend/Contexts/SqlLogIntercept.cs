using System.Collections.Concurrent;
using System.Collections.Generic;
using Castle.DynamicProxy;
using DapperExtensions.Extend.Contexts;

namespace DapperExtensions.Extend
{
    public class SqlLogInterceptor : IInterceptor
    {
        SqlBuilder _builder;
        public SqlLogInterceptor(string dbType = DbType.MySql)
        {
            _builder = new SqlBuilder(SqlFactory.GetDapperConfiguration(dbType));
        }

        public void Intercept(IInvocation invocation)
        {
            var method = invocation.Method.Name;
            if (!method.StartsWith("get_"))
            {
                var args = invocation.Arguments;
                var entityType = invocation.GenericArguments[0];
                var sql = string.Empty;
                IDictionary<string, object> parameters = new Dictionary<string, object>();
                switch (method)
                {
                    case "GetPage":
                        sql = _builder.GetPaging(entityType, args[0] as IPredicate, args[1] as IList<ISort>, int.Parse(args[2].ToString()), int.Parse(args[3].ToString()), out parameters);
                        break;
                    case "GetList":
                        sql = _builder.GetList(entityType, args[0] as IPredicate, args[1] as IList<ISort>, out parameters);
                        break;
                }
                WriteSqlToConsole(sql, parameters);

            }
            invocation.Proceed();
        }

        private void WriteSqlToConsole(string sql, IDictionary<string, object> parameters)
        {
            System.Diagnostics.Debug.Write(sql);
        }
    }
}
