using SqlSugar;

namespace FastStart.Domain.Entity
{
    /// <summary>
    /// 定时任务调度日志表
    ///</summary>
    [SugarTable("sys_job_log")]
    public class SysJobLog
    {
        /// <summary>
        /// 任务日志ID
        ///</summary>
        [SugarColumn(ColumnName = "job_log_id", IsPrimaryKey = true, IsIdentity = true)]
        public long JobLogId { get; set; }

        /// <summary>
        /// 任务名称
        ///</summary>
        [SugarColumn(ColumnName = "job_name")]
        public string JobName { get; set; }

        /// <summary>
        /// 任务组名
        ///</summary>
        [SugarColumn(ColumnName = "job_group")]
        public string JobGroup { get; set; }

        /// <summary>
        /// 调用目标字符串
        ///</summary>
        [SugarColumn(ColumnName = "invoke_target")]
        public string InvokeTarget { get; set; }

        /// <summary>
        /// 日志信息
        ///</summary>
        [SugarColumn(ColumnName = "job_message")]
        public string JobMessage { get; set; }

        /// <summary>
        /// 执行状态（0正常 1失败）
        /// 默认值: 0
        ///</summary>
        [SugarColumn(ColumnName = "status")]
        public string Status { get; set; }

        /// <summary>
        /// 异常信息
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "exception_info")]
        public string ExceptionInfo { get; set; }

        /// <summary>
        /// 创建时间
        ///</summary>
        [SugarColumn(ColumnName = "create_time")]
        public DateTime? CreateTime { get; set; }
    }
}