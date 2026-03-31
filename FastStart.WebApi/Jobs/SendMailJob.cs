using Quartz;
using Serilog;

namespace FastStart.WebApi.Jobs
{
    /// <summary>
    ///
    /// </summary>
    public class SendMailJob : IJob
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Quartz");
            Log.Information("Quartz");
            return Task.CompletedTask;
        }
    }
}