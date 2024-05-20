using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;

namespace FastStart.Quartz
{
    /// <summary>
    /// 自定义job工厂
    /// </summary>
    public class ZeroJobFactory : IJobFactory
    {
        /// <summary>
        /// 注入反射获取依赖对象
        /// </summary>
        private readonly IServiceProvider _serviceProvider;

        public ZeroJobFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 实现接口Job
        /// </summary>
        /// <param name="bundle"></param>
        /// <param name="scheduler"></param>
        /// <returns></returns>
        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            try
            {
                //从Quartz.net的源码实现net core注入这一块能够发现，job实例是通过AddTransient加入容器中的
                //还有自定义的JobFactory也需要单例注入，我觉的是因为如果不单例注入会导致Quartz使用默认的SimpleJobFactory
                //从而导致这里的获取Job实例出问题。
                var serviceScope = _serviceProvider.CreateScope();
                var job = serviceScope.ServiceProvider.GetService(bundle.JobDetail.JobType) as IJob;
                return job;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 允许工厂释放Job
        /// </summary>
        /// <param name="job"></param>
        public void ReturnJob(IJob job)
        {
            var disposable = job as IDisposable;
            disposable?.Dispose();
        }
    }
}