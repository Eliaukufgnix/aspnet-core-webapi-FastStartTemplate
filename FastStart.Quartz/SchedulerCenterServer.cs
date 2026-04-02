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
        private readonly Lazy<Task<IScheduler>> _schedulerLazy;
        private readonly IJobFactory _iocjobFactory;

        /// <summary>
        ///
        /// </summary>
        /// <param name="jobFactory"></param>
        public SchedulerCenterServer(IJobFactory jobFactory)
        {
            _iocjobFactory = jobFactory;// 不要在构造函数中调用异步方法，而是在首次使用时初始化//_scheduler = GetSchedulerAsync();
            _schedulerLazy = new Lazy<Task<IScheduler>>(CreateSchedulerAsync);
        }

        private async Task<IScheduler> CreateSchedulerAsync()
        {
            var collection = new NameValueCollection
            {
                { "quartz.serializer.type", "binary" },
            };
            var factory = new StdSchedulerFactory(collection);
            var scheduler = await factory.GetScheduler();
            scheduler.JobFactory = _iocjobFactory;
            return scheduler;
        }

        private Task<IScheduler> GetSchedulerAsync() => _schedulerLazy.Value;

        /// <summary>
        /// 开启任务调度
        /// </summary>
        /// <returns></returns>
        public async Task<ResultModel<string>> StartScheduleAsync()
        {
            try
            {
                var scheduler = await GetSchedulerAsync();
                if (scheduler.IsStarted)
                {
                    return ResultModel<string>.Fail("任务调度已经开启");
                }
                //等待任务运行完成
                await scheduler.Start();
                await Console.Out.WriteLineAsync("任务调度开启！");
                return ResultModel<string>.Success("任务调度开启成功");
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
                var scheduler = await GetSchedulerAsync();
                if (scheduler.IsShutdown)
                {
                    return ResultModel<string>.Fail("任务调度已经停止");
                }
                //等待任务运行完成
                await scheduler.Shutdown();
                await Console.Out.WriteLineAsync("任务调度停止！");
                return ResultModel<string>.Success("任务调度停止成功");
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
            if (tasksQz == null)
            {
                return ResultModel<string>.Fail("任务计划不能为空");
            }
            // 确保必要的属性非空
            if (string.IsNullOrWhiteSpace(tasksQz.JobGroup) ||
                string.IsNullOrWhiteSpace(tasksQz.Name) ||
                string.IsNullOrWhiteSpace(tasksQz.AssemblyName) ||
                string.IsNullOrWhiteSpace(tasksQz.ClassName))
            {
                return ResultModel<string>.Fail("任务计划信息不完整，缺少 JobGroup/Name/AssemblyName/ClassName");
            }
            var scheduler = await GetSchedulerAsync();
            JobKey jobKey = new JobKey(tasksQz.Id.ToString(), tasksQz.JobGroup);
            if (await scheduler.CheckExists(jobKey))
            {
                return ResultModel<string>.Fail($"该任务计划已经在执行:【{tasksQz.Name}】,请勿重复启动！");
            }

            // 设置开始和结束时间默认值
            DateTime beginTime = tasksQz.BeginTime ?? DateTime.Now;
            DateTime endTime = tasksQz.EndTime ?? DateTime.MaxValue.AddDays(-1);

            // 反射获取作业类型
            Type? jobType = GetJobType(tasksQz.AssemblyName, tasksQz.ClassName);
            if (jobType == null)
            {
                return ResultModel<string>.Fail($"无法加载作业类型：{tasksQz.AssemblyName}.{tasksQz.ClassName}");
            }

            // 判断任务调度是否开启（如果未开启则启动）
            if (!scheduler.IsStarted)
            {
                await StartScheduleAsync();
            }

            IJobDetail job = new JobDetailImpl(tasksQz.Id.ToString(), tasksQz.JobGroup, jobType);
            if (tasksQz.JobParams != null)
            {
                job.JobDataMap.Add("JobParam", tasksQz.JobParams);
            }

            ITrigger trigger;
            if (!string.IsNullOrEmpty(tasksQz.Cron) && CronExpression.IsValidExpression(tasksQz.Cron) && tasksQz.TriggerType > 0)
            {
                trigger = CreateCronTrigger(tasksQz.Id.ToString(), tasksQz.JobGroup, tasksQz.Cron, beginTime, endTime);
            }
            else
            {
                trigger = CreateSimpleTrigger(tasksQz.Id.ToString(), tasksQz.JobGroup, tasksQz.IntervalSecond, tasksQz.RunTimes, beginTime, endTime);
            }

            await scheduler.ScheduleJob(job, trigger);
            return ResultModel<string>.Success($"启动任务:【{tasksQz.Name}】成功");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="className"></param>
        /// <returns></returns>
        private static Type? GetJobType(string assemblyName, string className)
        {
            try
            {
                Assembly assembly = Assembly.Load(new AssemblyName(assemblyName));
                string fullName = $"{assemblyName}.{className}";
                return assembly.GetType(fullName);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 暂停一个指定的计划任务
        /// </summary>
        public async Task<ResultModel<string>> StopScheduleJobAsync(TasksQz sysSchedule)
        {
            if (sysSchedule == null) return ResultModel<string>.Fail("任务计划不能为空");
            if (string.IsNullOrWhiteSpace(sysSchedule.JobGroup)) return ResultModel<string>.Fail("JobGroup 不能为空");

            var scheduler = await GetSchedulerAsync();
            JobKey jobKey = new JobKey(sysSchedule.Id.ToString(), sysSchedule.JobGroup);
            if (!await scheduler.CheckExists(jobKey))
            {
                return ResultModel<string>.Fail($"未找到要暂停的任务:【{sysSchedule.Name}】");
            }
            await scheduler.PauseJob(jobKey);
            return ResultModel<string>.Success($"暂停任务:【{sysSchedule.Name}】成功");
        }

        /// <summary>
        /// 恢复指定的计划任务
        /// </summary>
        public async Task<ResultModel<string>> ResumeJob(TasksQz sysSchedule)
        {
            if (sysSchedule == null) return ResultModel<string>.Fail("任务计划不能为空");
            if (string.IsNullOrWhiteSpace(sysSchedule.JobGroup)) return ResultModel<string>.Fail("JobGroup 不能为空");

            var scheduler = await GetSchedulerAsync();
            JobKey jobKey = new JobKey(sysSchedule.Id.ToString(), sysSchedule.JobGroup);
            if (!await scheduler.CheckExists(jobKey))
            {
                return ResultModel<string>.Fail($"未找到要重新的任务:【{sysSchedule.Name}】,请先选择添加计划！");
            }
            await scheduler.ResumeJob(jobKey);
            return ResultModel<string>.Success($"恢复计划任务:【{sysSchedule.Name}】成功");
        }

        #region 创建触发器帮助方法

        private ITrigger CreateSimpleTrigger(string jobId, string jobGroup, int intervalSecond, int runTimes, DateTime beginTime, DateTime endTime)
        {
            var builder = TriggerBuilder.Create()
                .WithIdentity(jobId, jobGroup)
                .StartAt(beginTime)
                .EndAt(endTime);

            if (runTimes > 0)
            {
                builder.WithSimpleSchedule(x => x.WithIntervalInSeconds(intervalSecond).WithRepeatCount(runTimes));
            }
            else
            {
                builder.WithSimpleSchedule(x => x.WithIntervalInSeconds(intervalSecond).RepeatForever());
            }

            return builder.ForJob(jobId, jobGroup).Build();
        }

        private ITrigger CreateCronTrigger(string jobId, string jobGroup, string cron, DateTime beginTime, DateTime endTime)
        {
            return TriggerBuilder.Create()
                .WithIdentity(jobId, jobGroup)
                .StartAt(beginTime)
                .EndAt(endTime)
                .WithCronSchedule(cron)
                .ForJob(jobId, jobGroup)
                .Build();
        }

        #endregion 创建触发器帮助方法
    }
}