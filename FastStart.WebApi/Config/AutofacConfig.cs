using Autofac;
using FastStart.Quartz;
using FastStart.Service.impl;
using Quartz.Spi;
using SqlSugar;
using System.Reflection;

namespace FastStart.WebApi.Config
{
    /// <summary>
    /// Autofac配置信息
    /// </summary>
    public class AutofacConfig : Autofac.Module
    {
        /// <summary>
        /// Autofac加载
        /// </summary>
        /// <param name="builder"></param>
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
                return SqlSugarConfig.AddSqlSugarModule(c.Resolve<IConfiguration>(), "SqlServer");
            }).As<ISqlSugarClient>().InstancePerLifetimeScope();
            //自定义Job工厂
            // services.AddSingleton<IJobFactory, ZeroJobFactory>();
            builder.RegisterType<ZeroJobFactory>().As<IJobFactory>().SingleInstance();
            //任务调度控制中心
            // services.AddSingleton<ISchedulerCenter, SchedulerCenterServer>();
            builder.RegisterType<SchedulerCenterServer>().As<ISchedulerCenter>().SingleInstance();
            //Jobs,将组件好的Job放在这里，生命周期为瞬时的
            // services.AddTransient<SendMailJob>();
            builder.RegisterType<SendMailJob>().AsSelf().InstancePerDependency();
        }
    }
}