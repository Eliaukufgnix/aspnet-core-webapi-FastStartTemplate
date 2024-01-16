using SqlSugar;

namespace FastStart.Domain.Entity
{
    /// <summary>
    /// 字典类型表
    ///</summary>
    [SugarTable("sys_dict_type")]
    public class SysDictType
    {
        /// <summary>
        /// 字典主键
        ///</summary>
        [SugarColumn(ColumnName = "dict_id", IsPrimaryKey = true, IsIdentity = true)]
        public long DictId { get; set; }

        /// <summary>
        /// 字典名称
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "dict_name")]
        public string DictName { get; set; }

        /// <summary>
        /// 字典类型
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "dict_type")]
        public string DictType { get; set; }

        /// <summary>
        /// 状态（0正常 1停用）
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
        /// 备注
        ///</summary>
        [SugarColumn(ColumnName = "remark")]
        public string Remark { get; set; }
    }
}