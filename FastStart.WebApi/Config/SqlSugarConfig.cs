using Serilog;
using SqlSugar;

namespace FastStart.WebApi.Config
{
    /// <summary>
    /// SqlSugar配置
    /// </summary>
    public static class SqlSugarConfig
    {
        /// <summary>
        /// 添加SqlSugar模块
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="connName"></param>
        /// <returns></returns>
        public static SqlSugarClient AddSqlSugarModule(IConfiguration configuration, string connName)
        {
            using var log = new LoggerConfiguration().WriteTo.Console().CreateLogger();
            // Scoped 用 SqlSugarClient
            return new SqlSugarClient(new ConnectionConfig()
            {
                DbType = DbType.SqlServer,
                ConnectionString = configuration.GetConnectionString(connName),
                IsAutoCloseConnection = true,
            },
            db =>
            {
                // 单例参数配置，所有上下文生效
                db.Aop.OnLogExecuting = (sql, pars) =>
                {
                    // 打印sql
                    Console.WriteLine(sql + "\r\n" + db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
                };
                // sql执行完毕后
                db.Aop.OnLogExecuted = (sql, pars) =>
                {
                    // sql执行时间
                    Console.WriteLine("\r\ntime:" + db.Ado.SqlExecutionTime.ToString());
                    // 记录sql执行时间，使用 ForContext 添加 "SqlLog" 属性
                    Log.ForContext("SqlLog", true).Information("\r\n" + sql + "\r\n" + db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)) + "\r\ntime:" + db.Ado.SqlExecutionTime.ToString());
                };
            });
        }
    }
}