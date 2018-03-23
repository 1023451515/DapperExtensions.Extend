#define DEBUG

using Autofac;
using DapperExtensions.Extend;
using Autofac.Extras.DynamicProxy;
using DapperExtensions.Extend.Contexts;

namespace ApiDemo
{
    public class DapperContextModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterType<SqlLogInterceptor>().AsSelf();

            builder.Register<IDapperContext>(c => new DapperContext("db", DbType.MsSql))
                ;
#if DEBUG
                //.EnableInterfaceInterceptors()
                //.InterceptedBy(typeof(SqlLogInterceptor));
#endif

            builder.RegisterGeneric(typeof(RespositoryBase<>)).As(typeof(IRespositoryBase<>));
        }
    }
}