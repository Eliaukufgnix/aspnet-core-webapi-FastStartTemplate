using SqlSugar;

namespace FastStart.Domain.Entity
{
    /// <summary>
    /// 字典数据表
    ///</summary>
    [SugarTable("sys_dict_data")]
    public class SysDictData
    {
        /// <summary>
        /// 字典编码
        ///</summary>
        [SugarColumn(ColumnName = "dict_code", IsPrimaryKey = true, IsIdentity = true)]
        public long DictCode { get; set; }

        /// <summary>
        /// 字典排序
        /// 默认值: 0
        ///</summary>
        [SugarColumn(ColumnName = "dict_sort")]
        public int? DictSort { get; set; }

        /// <summary>
        /// 字典标签
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "dict_label")]
        public string DictLabel { get; set; }

        /// <summary>
        /// 字典键值
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "dict_value")]
        public string DictValue { get; set; }

        /// <summary>
        /// 字典类型
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "dict_type")]
        public string DictType { get; set; }

        /// <summary>
        /// 样式属性（其他样式扩展）
        ///</summary>
        [SugarColumn(ColumnName = "css_class")]
        public string CssClass { get; set; }

        /// <summary>
        /// 表格回显样式
        ///</summary>
        [SugarColumn(ColumnName = "list_class")]
        public string ListClass { get; set; }

        /// <summary>
        /// 是否默认（Y是 N否）
        /// 默认值: N
        ///</summary>
        [SugarColumn(ColumnName = "is_default")]
        public string IsDefault { get; set; }

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