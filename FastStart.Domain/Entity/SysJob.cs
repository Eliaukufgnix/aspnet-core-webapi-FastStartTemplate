using SqlSugar;

namespace FastStart.Domain.Entity
{
    /// <summary>
    /// 定时任务调度表
    ///</summary>
    [SugarTable("sys_job")]
    public class SysJob
    {
        /// <summary>
        /// 任务ID
        ///</summary>
        [SugarColumn(ColumnName = "job_id", IsPrimaryKey = true, IsIdentity = true)]
        public long JobId { get; set; }

        /// <summary>
        /// 任务名称
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "job_name", IsPrimaryKey = true)]
        public string JobName { get; set; }

        /// <summary>
        /// 任务组名
        /// 默认值: DEFAULT
        ///</summary>
        [SugarColumn(ColumnName = "job_group", IsPrimaryKey = true)]
        public string JobGroup { get; set; }

        /// <summary>
        /// 调用目标字符串
        ///</summary>
        [SugarColumn(ColumnName = "invoke_target")]
        public string InvokeTarget { get; set; }

        /// <summary>
        /// cron执行表达式
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "cron_expression")]
        public string CronExpression { get; set; }

        /// <summary>
        /// 计划执行错误策略（1立即执行 2执行一次 3放弃执行）
        /// 默认值: 3
        ///</summary>
        [SugarColumn(ColumnName = "misfire_policy")]
        public string MisfirePolicy { get; set; }

        /// <summary>
        /// 是否并发执行（0允许 1禁止）
        /// 默认值: 1
        ///</summary>
        [SugarColumn(ColumnName = "concurrent")]
        public string Concurrent { get; set; }

        /// <summary>
        /// 状态（0正常 1暂停）
        /// 默认值: 0
        ///</summary>
        [SugarColumn(ColumnName = "status")]
        public string Status { get; set; }

        /// <summary>
        /// 创建者
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "create_by")]
        public string CreateBy { get; set; }

        /// <summary>
        /// 创建时间
        ///</summary>
        [SugarColumn(ColumnName = "create_time")]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 更新者
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "update_by")]
        public string UpdateBy { get; set; }

        /// <summary>
        /// 更新时间
        ///</summary>
        [SugarColumn(ColumnName = "update_time")]
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 备注信息
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "remark")]
        public string Remark { get; set; }
    }
}