using FastStart.Domain;
using FastStart.Domain.Entity;
using FastStart.Quartz;
using Microsoft.AspNetCore.Mvc;

namespace FastStart.WebApi.Controllers
{
    /// <summary>
    /// Quartz定时调度
    /// </summary>
    [Route("dev-api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Quartz")]
    public class QuartzController : ControllerBase
    {
        private readonly ISchedulerCenter _center;

        public QuartzController(ISchedulerCenter center)
        {
            _center = center;
        }

        private TasksQz test = new TasksQz
        {
            Id = 999999,
            Name = "test_sendmail",
            JobGroup = "Group1",
            AssemblyName = "FastStart.Quartz",
            ClassName = "SendMailJob",
            TriggerType = 1,
            Cron = "0/5 * * * * ? "
        };

        /// <summary>
        /// 开启任务调度
        /// </summary>
        /// <returns></returns>
        [HttpGet("Start")]
        public async Task<ResultModel<string>> Start()
        {
            ResultModel<string> resultModel = await _center.StartScheduleAsync();
            return resultModel;
        }

        /// <summary>
        /// 停止任务调度
        /// </summary>
        /// <returns></returns>
        [HttpGet("Stop")]
        public async Task<ResultModel<string>> Stop()
        {
            ResultModel<string> resultModel = await _center.StopScheduleAsync();
            return resultModel;
        }

        /// <summary>
        /// 添加一个任务调度
        /// </summary>
        /// <returns></returns>
        [HttpGet("AddJob")]
        public async Task<ResultModel<string>> AddJob()
        {
            ResultModel<string> resultModel = await _center.AddScheduleJobAsync(test);
            return resultModel;
        }

        /// <summary>
        /// 停止一个任务调度
        /// </summary>
        /// <returns></returns>
        [HttpGet("StopJob")]
        public async Task<ResultModel<string>> StopJob()
        {
            ResultModel<string> resultModel = await _center.StopScheduleJobAsync(test);
            return resultModel;
        }

        /// <summary>
        /// 恢复一个任务调度
        /// </summary>
        /// <returns></returns>
        [HttpGet("ResumeJob")]
        public async Task<ResultModel<string>> ResumeJob()
        {
            ResultModel<string> resultModel = await _center.ResumeJob(test);
            return resultModel;
        }
    }
}