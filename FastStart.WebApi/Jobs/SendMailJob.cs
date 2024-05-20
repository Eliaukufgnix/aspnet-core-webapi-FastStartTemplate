using Quartz;
using Serilog;

namespace FastStart.WebApi.Jobs
{
    public class SendMailJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Quartz");
            Log.Information("Quartz");
            return Task.CompletedTask;
        }
    }
}