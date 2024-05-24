using Quartz;

namespace FastStart.Quartz
{
    [DisallowConcurrentExecution]
    [PersistJobDataAfterExecution]
    public class SendMailJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            await Task.Run(() =>
            {
                Console.WriteLine("First Job");
                //job详情
                var jobDetails = context.JobDetail;
                //触发器的信息
                var trigger = context.Trigger;
                Console.WriteLine($"JobKey：{jobDetails.Key}，Group：{jobDetails.Key.Group}\r\n" +
                    $"Trigger：{trigger.Key}\r\n" +
                    $"RunTime：{context.JobRunTime}" +
                    $"ExecuteTime：{DateTime.Now}");
            });
        }
    }
}