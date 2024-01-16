using SqlSugar;

namespace FastStart.Domain.Entity
{
    /// <summary>
    /// 系统访问记录
    ///</summary>
    [SugarTable("sys_logininfor")]
    public class SysLogininfor
    {
        /// <summary>
        /// 访问ID
        ///</summary>
        [SugarColumn(ColumnName = "info_id", IsPrimaryKey = true, IsIdentity = true)]
        public long InfoId { get; set; }

        /// <summary>
        /// 用户账号
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "user_name")]
        public string UserName { get; set; }

        /// <summary>
        /// 登录IP地址
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "ipaddr")]
        public string Ipaddr { get; set; }

        /// <summary>
        /// 登录地点
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "login_location")]
        public string LoginLocation { get; set; }

        /// <summary>
        /// 浏览器类型
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "browser")]
        public string Browser { get; set; }

        /// <summary>
        /// 操作系统
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "os")]
        public string Os { get; set; }

        /// <summary>
        /// 登录状态（0成功 1失败）
        /// 默认值: 0
        ///</summary>
        [SugarColumn(ColumnName = "status")]
        public string Status { get; set; }

        /// <summary>
        /// 提示消息
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "msg")]
        public string Msg { get; set; }

        /// <summary>
        /// 访问时间
        ///</summary>
        [SugarColumn(ColumnName = "login_time")]
        public DateTime? LoginTime { get; set; }
    }
}