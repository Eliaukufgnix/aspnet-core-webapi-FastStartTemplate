using SqlSugar;

namespace FastStart.Domain.Entity
{
    /// <summary>
    /// 参数配置表
    ///</summary>
    [SugarTable("sys_config")]
    public class SysConfig
    {
        /// <summary>
        /// 参数主键
        ///</summary>
        [SugarColumn(ColumnName = "config_id", IsPrimaryKey = true, IsIdentity = true)]
        public int ConfigId { get; set; }

        /// <summary>
        /// 参数名称
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "config_name")]
        public string ConfigName { get; set; }

        /// <summary>
        /// 参数键名
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "config_key")]
        public string ConfigKey { get; set; }

        /// <summary>
        /// 参数键值
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "config_value")]
        public string ConfigValue { get; set; }

        /// <summary>
        /// 系统内置（Y是 N否）
        /// 默认值: N
        ///</summary>
        [SugarColumn(ColumnName = "config_type")]
        public string ConfigType { get; set; }

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
        /// 备注
        ///</summary>
        [SugarColumn(ColumnName = "remark")]
        public string Remark { get; set; }
    }
}