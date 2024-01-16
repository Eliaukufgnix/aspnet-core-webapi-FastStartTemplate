using Autofac;
using FastStart.Service.impl;
using SqlSugar;
using System.Reflection;

namespace FastStart.WebApi.Config
{
    /// <summary>
    /// Autofac配置信息
    /// </summary>
    public class AutofacConfig : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //程序集目录
            var basePath = AppDomain.CurrentDomain.BaseDirectory;

            //仓储层服务层程序集路径
            var repositoryPath = Path.Combine(basePath, "FastStart.Repository.dll");
            var servicePath = Path.Combine(basePath, "FastStart.Service.dll");

            //加载程序集
            var repository = Assembly.LoadFrom(repositoryPath);
            var service = Assembly.LoadFrom(servicePath);

            //注入程序集
            builder.RegisterAssemblyTypes(repository, service).AsImplementedInterfaces();
            // 注入SysLoginService
            builder.RegisterType<LoginService>().AsSelf().InstancePerLifetimeScope();
            // 注入ISqlSugarClient
            builder.Register(c =>
            {
                return SqlSugarConfig.AddSqlSugarModule(c.Resolve<IConfiguration>(), "MySql");
            }).As<ISqlSugarClient>().SingleInstance();
        }
    }
}