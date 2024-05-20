using FastStart.Domain;
using FastStart.Domain.Entity;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System.Collections.Specialized;
using System.Reflection;

namespace FastStart.Quartz
{
    /// <summary>
    /// 任务调度管理中心
    /// </summary>
    public class SchedulerCenterServer : ISchedulerCenter
    {
        private Task<IScheduler> _scheduler;
        private readonly IJobFactory _iocjobFactory;

        /// <summary>
        ///
        /// </summary>
        /// <param name="jobFactory"></param>
        public SchedulerCenterServer(IJobFactory jobFactory)
        {
            _iocjobFactory = jobFactory;// 不要在构造函数中调用异步方法，而是在首次使用时初始化//_scheduler = GetSchedulerAsync();
        }

        private async Task<IScheduler> GetSchedulerAsync()
        {
            // 从Factory中获取Scheduler实例
            var collection = new NameValueCollection
            {
                { "quartz.serializer.type", "binary" },
            };
            // 实例化工厂
            ISchedulerFactory factory = new StdSchedulerFactory(collection);
            var scheduler = await factory.GetScheduler();
            scheduler.JobFactory = _iocjobFactory;
            return scheduler;
        }

        private async Task<IScheduler> GetOrCreateSchedulerAsync()
        {
            if (_scheduler == null || _scheduler.IsCompleted)
            {
                _scheduler = GetSchedulerAsync();
            }
            return await _scheduler;
        }

        /// <summary>
        /// 开启任务调度
        /// </summary>
        /// <returns></returns>
        public async Task<ResultModel<string>> StartScheduleAsync()
        {
            try
            {
                var scheduler = await GetOrCreateSchedulerAsync();
                if (!scheduler.IsStarted)
                {
                    //等待任务运行完成
                    await scheduler.Start();
                    await Console.Out.WriteLineAsync("任务调度开启！");
                    return ResultModel<string>.Success("任务调度开启成功");
                }
                else
                {
                    return ResultModel<string>.Fail("任务调度已经开启");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 停止任务调度
        /// </summary>
        /// <returns></returns>
        public async Task<ResultModel<string>> StopScheduleAsync()
        {
            try
            {
                var scheduler = await GetOrCreateSchedulerAsync();
                if (!scheduler.IsShutdown)
                {
                    //等待任务运行完成
                    await scheduler.Shutdown();
                    await Console.Out.WriteLineAsync("任务调度停止！");
                    return ResultModel<string>.Success("任务调度停止成功");
                }
                else
                {
                    return ResultModel<string>.Fail("任务调度已经停止");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 添加一个计划任务（映射程序集指定IJob实现类）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tasksQz"></param>
        /// <returns></returns>
        public async Task<ResultModel<string>> AddScheduleJobAsync(TasksQz tasksQz)
        {
            try
            {
                var scheduler = await GetOrCreateSchedulerAsync();
                if (tasksQz != null)
                {
                    JobKey jobKey = new JobKey(tasksQz.Id.ToString(), tasksQz.JobGroup);
                    if (await scheduler.CheckExists(jobKey))
                    {
                        return ResultModel<string>.Fail($"该任务计划已经在执行:【{tasksQz.Name}】,请勿重复启动！");
                    }

                    #region 设置开始时间和结束时间

                    if (tasksQz.BeginTime == null)
                    {
                        tasksQz.BeginTime = DateTime.Now;
                    }
                    DateTimeOffset starRunTime = DateBuilder.NextGivenSecondDate(tasksQz.BeginTime, 1);//设置开始时间
                    if (tasksQz.EndTime == null)
                    {
                        tasksQz.EndTime = DateTime.MaxValue.AddDays(-1);
                    }
                    DateTimeOffset endRunTime = DateBuilder.NextGivenSecondDate(tasksQz.EndTime, 1);//设置暂停时间

                    #endregion 设置开始时间和结束时间

                    #region 通过反射获取程序集类型和类

                    Assembly assembly = Assembly.Load(new AssemblyName(tasksQz.AssemblyName));
                    Type jobType = assembly.GetType(tasksQz.AssemblyName + "." + tasksQz.ClassName);

                    #endregion 通过反射获取程序集类型和类

                    //判断任务调度是否开启
                    if (!scheduler.IsStarted)
                    {
                        await StartScheduleAsync();
                    }

                    //传入反射出来的执行程序集
                    IJobDetail job = new JobDetailImpl(tasksQz.Id.ToString(), tasksQz.JobGroup, jobType);
                    job.JobDataMap.Add("JobParam", tasksQz.JobParams);
                    ITrigger trigger;

                    #region 泛型传递

                    //IJobDetail job = JobBuilder.Create<T>()
                    //    .WithIdentity(sysSchedule.Name, sysSchedule.JobGroup)
                    //    .Build();

                    #endregion 泛型传递

                    if (tasksQz.Cron != null && CronExpression.IsValidExpression(tasksQz.Cron) && tasksQz.TriggerType > 0)
                    {
                        trigger = CreateCronTrigger(tasksQz);
                    }
                    else
                    {
                        trigger = CreateSimpleTrigger(tasksQz);
                    }
                    // 告诉Quartz使用我们的触发器来安排作业
                    await scheduler.ScheduleJob(job, trigger);
                    //await Task.Delay(TimeSpan.FromSeconds(120));
                    //await Console.Out.WriteLineAsync("关闭了调度器！");
                    //await _scheduler.Result.Shutdown();
                    return ResultModel<string>.Success($"启动任务:【{tasksQz.Name}】成功");
                }
                else
                {
                    return ResultModel<string>.Fail($"任务计划不存在:【{tasksQz.Name}】");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 暂停一个指定的计划任务
        /// </summary>
        /// <returns></returns>
        public async Task<ResultModel<string>> StopScheduleJobAsync(TasksQz sysSchedule)
        {
            try
            {
                var scheduler = await GetOrCreateSchedulerAsync();
                JobKey jobKey = new JobKey(sysSchedule.Id.ToString(), sysSchedule.JobGroup);
                if (!await scheduler.CheckExists(jobKey))
                {
                    return ResultModel<string>.Fail($"未找到要暂停的任务:【{sysSchedule.Name}】");
                }
                else
                {
                    await scheduler.PauseJob(jobKey);
                    return ResultModel<string>.Success($"暂停任务:【{sysSchedule.Name}】成功");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 恢复指定的计划任务
        /// </summary>
        /// <param name="sysSchedule"></param>
        /// <returns></returns>
        public async Task<ResultModel<string>> ResumeJob(TasksQz sysSchedule)
        {
            try
            {
                var scheduler = await GetOrCreateSchedulerAsync();
                JobKey jobKey = new JobKey(sysSchedule.Id.ToString(), sysSchedule.JobGroup);
                if (!await scheduler.CheckExists(jobKey))
                {
                    return ResultModel<string>.Fail($"未找到要重新的任务:【{sysSchedule.Name}】,请先选择添加计划！");
                }
                await scheduler.ResumeJob(jobKey);
                return ResultModel<string>.Success($"恢复计划任务:【{sysSchedule.Name}】成功");
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region 创建触发器帮助方法

        /// <summary>
        /// 创建SimpleTrigger触发器（简单触发器）
        /// </summary>
        /// <param name="sysSchedule"></param>
        /// <param name="starRunTime"></param>
        /// <param name="endRunTime"></param>
        /// <returns></returns>
        private ITrigger CreateSimpleTrigger(TasksQz sysSchedule)
        {
            if (sysSchedule.RunTimes > 0)
            {
                ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity(sysSchedule.Id.ToString(), sysSchedule.JobGroup)
                .StartAt(sysSchedule.BeginTime.Value)
                .EndAt(sysSchedule.EndTime.Value)
                .WithSimpleSchedule(x =>
                x.WithIntervalInSeconds(sysSchedule.IntervalSecond)
                .WithRepeatCount(sysSchedule.RunTimes)).ForJob(sysSchedule.Id.ToString(), sysSchedule.JobGroup).Build();
                return trigger;
            }
            else
            {
                ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity(sysSchedule.Id.ToString(), sysSchedule.JobGroup)
                .StartAt(sysSchedule.BeginTime.Value)
                .EndAt(sysSchedule.EndTime.Value)
                .WithSimpleSchedule(x =>
                x.WithIntervalInSeconds(sysSchedule.IntervalSecond)
                .RepeatForever()).ForJob(sysSchedule.Id.ToString(), sysSchedule.JobGroup).Build();
                return trigger;
            }
            // 触发作业立即运行，然后每10秒重复一次，无限循环
        }

        /// <summary>
        /// 创建类型Cron的触发器
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        private ITrigger CreateCronTrigger(TasksQz sysSchedule)
        {
            // 作业触发器
            return TriggerBuilder.Create()
                   .WithIdentity(sysSchedule.Id.ToString(), sysSchedule.JobGroup)
                   .StartAt(sysSchedule.BeginTime.Value)//开始时间
                   .EndAt(sysSchedule.EndTime.Value)//结束数据
                   .WithCronSchedule(sysSchedule.Cron)//指定cron表达式
                   .ForJob(sysSchedule.Id.ToString(), sysSchedule.JobGroup)//作业名称
                   .Build();
        }

        #endregion 创建触发器帮助方法
    }
}