using SqlSugar;

namespace FastStart.Domain.Entity
{
    /// <summary>
    /// 岗位信息表
    ///</summary>
    [SugarTable("sys_post")]
    public class SysPost
    {
        /// <summary>
        /// 岗位ID
        ///</summary>
        [SugarColumn(ColumnName = "post_id", IsPrimaryKey = true, IsIdentity = true)]
        public long PostId { get; set; }

        /// <summary>
        /// 岗位编码
        ///</summary>
        [SugarColumn(ColumnName = "post_code")]
        public string PostCode { get; set; }

        /// <summary>
        /// 岗位名称
        ///</summary>
        [SugarColumn(ColumnName = "post_name")]
        public string PostName { get; set; }

        /// <summary>
        /// 显示顺序
        ///</summary>
        [SugarColumn(ColumnName = "post_sort")]
        public int PostSort { get; set; }

        /// <summary>
        /// 状态（0正常 1停用）
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