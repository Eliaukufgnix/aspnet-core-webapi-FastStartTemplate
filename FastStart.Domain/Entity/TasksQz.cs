using SqlSugar;

namespace FastStart.Domain.Entity
{
    /// <summary>
    /// 任务计划表
    ///</summary>
    [SugarTable("tasks_quartz")]
    public class TasksQz
    {
        /// <summary>
        /// 任务id
        ///</summary>
        [SugarColumn(ColumnName = "quartz_id", IsPrimaryKey = true)]
        public long Id { get; set; }

        /// <summary>
        /// 任务名称
        ///</summary>
        [SugarColumn(ColumnName = "quartz_name")]
        public string Name { get; set; }

        /// <summary>
        /// 任务分组
        ///</summary>
        [SugarColumn(ColumnName = "quartz_job_group")]
        public string JobGroup { get; set; }

        /// <summary>
        /// 任务运行时间表达式
        ///</summary>
        [SugarColumn(ColumnName = "quartz_cron")]
        public string Cron { get; set; }

        /// <summary>
        /// 任务所在DLL对应的程序集名称
        ///</summary>
        [SugarColumn(ColumnName = "quartz_assembly_name")]
        public string AssemblyName { get; set; }

        /// <summary>
        /// 任务所在类
        ///</summary>
        [SugarColumn(ColumnName = "quartz_class_name")]
        public string ClassName { get; set; }

        /// <summary>
        /// 任务描述
        ///</summary>
        [SugarColumn(ColumnName = "quartz_remark")]
        public string Remark { get; set; }

        /// <summary>
        /// 执行次数
        ///</summary>
        [SugarColumn(ColumnName = "quartz_run_times")]
        public int RunTimes { get; set; }

        /// <summary>
        /// 开始时间
        ///</summary>
        [SugarColumn(ColumnName = "quartz_begin_time")]
        public DateTime? BeginTime { get; set; }

        /// <summary>
        /// 结束时间
        ///</summary>
        [SugarColumn(ColumnName = "quartz_end_time")]
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 触发器类型（0、simple 1、cron）
        ///</summary>
        [SugarColumn(ColumnName = "quartz_trigger_type")]
        public int TriggerType { get; set; }

        /// <summary>
        /// 执行间隔时间, 秒为单位
        ///</summary>
        [SugarColumn(ColumnName = "quartz_interval_second")]
        public int IntervalSecond { get; set; }

        /// <summary>
        /// 是否启动
        ///</summary>
        [SugarColumn(ColumnName = "quartz_is_start")]
        public string IsStart { get; set; }

        /// <summary>
        /// 执行传参
        ///</summary>
        [SugarColumn(ColumnName = "quartz_job_params")]
        public string JobParams { get; set; }

        /// <summary>
        /// 是否删除
        ///</summary>
        [SugarColumn(ColumnName = "quartz_is_deleted")]
        public string IsDeleted { get; set; }

        /// <summary>
        /// 创建时间
        ///</summary>
        [SugarColumn(ColumnName = "quartz_create_time")]
        public DateTime CreateTime { get; set; }
    }
}