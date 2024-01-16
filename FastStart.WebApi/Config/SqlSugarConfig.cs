using Serilog;
using SqlSugar;

namespace FastStart.WebApi.Config
{
    /// <summary>
    /// SqlSugar配置
    /// </summary>
    public static class SqlSugarConfig
    {
        public static SqlSugarClient AddSqlSugarModule(IConfiguration configuration, string connName)
        {
            using var log = new LoggerConfiguration().WriteTo.Console().CreateLogger();
            // Scoped 用 SqlSugarClient
            return new SqlSugarClient(new ConnectionConfig()
            {
                DbType = DbType.MySql,
                ConnectionString = configuration.GetConnectionString(connName),
                IsAutoCloseConnection = true,
            },
            db =>
            {
                // 单例参数配置，所有上下文生效
                db.Aop.OnLogExecuting = (sql, pars) =>
                {
                    // 打印sql
                    Console.WriteLine(sql + "\r\n" +db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
                    // 记录sql，使用 ForContext 添加 "SqlLog" 属性
                    Log.ForContext("SqlLog", true).Information(sql + "\r\n" + db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
                };
            });
        }
    }
}
