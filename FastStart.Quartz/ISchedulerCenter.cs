using FastStart.Domain;
using FastStart.Domain.Entity;

namespace FastStart.Quartz
{
    /// <summary>
    /// 服务调度接口
    /// </summary>
    public interface ISchedulerCenter
    {
        /// <summary>
        /// 开启任务调度
        /// </summary>
        /// <returns></returns>
        Task<ResultModel<string>> StartScheduleAsync();

        /// <summary>
        /// 停止任务调度
        /// </summary>
        /// <returns></returns>
        Task<ResultModel<string>> StopScheduleAsync();

        /// <summary>
        /// 添加一个任务
        /// </summary>
        /// <param name="sysSchedule"></param>
        /// <returns></returns>
        Task<ResultModel<string>> AddScheduleJobAsync(TasksQz sysSchedule);

        /// <summary>
        /// 暂停一个指定的计划任务
        /// </summary>
        /// <param name="sysSchedule"></param>
        /// <returns></returns>
        Task<ResultModel<string>> StopScheduleJobAsync(TasksQz sysSchedule);

        /// <summary>
        /// 恢复指定的计划任务
        /// </summary>
        /// <param name="sysSchedule"></param>
        /// <returns></returns>
        Task<ResultModel<string>> ResumeJob(TasksQz sysSchedule);
    }
}