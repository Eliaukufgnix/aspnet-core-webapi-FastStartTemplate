using SqlSugar;

namespace FastStart.Domain.Entity
{
    /// <summary>
    /// 通知公告表
    ///</summary>
    [SugarTable("sys_notice")]
    public class SysNotice
    {
        /// <summary>
        /// 公告ID
        ///</summary>
        [SugarColumn(ColumnName = "notice_id", IsPrimaryKey = true, IsIdentity = true)]
        public int NoticeId { get; set; }

        /// <summary>
        /// 公告标题
        ///</summary>
        [SugarColumn(ColumnName = "notice_title")]
        public string NoticeTitle { get; set; }

        /// <summary>
        /// 公告类型（1通知 2公告）
        ///</summary>
        [SugarColumn(ColumnName = "notice_type")]
        public string NoticeType { get; set; }

        /// <summary>
        /// 公告内容
        ///</summary>
        [SugarColumn(ColumnName = "notice_content")]
        public byte[] NoticeContent { get; set; }

        /// <summary>
        /// 公告状态（0正常 1关闭）
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