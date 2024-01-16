using SqlSugar;

namespace FastStart.Domain.Entity
{
    /// <summary>
    /// 用户信息表
    ///</summary>
    [SugarTable("sys_user")]
    public class SysUser
    {
        /// <summary>
        /// 用户ID
        ///</summary>
        [SugarColumn(ColumnName = "user_id", IsPrimaryKey = true, IsIdentity = true)]
        public long UserId { get; set; }

        /// <summary>
        /// 部门ID
        ///</summary>
        [SugarColumn(ColumnName = "dept_id")]
        public long? DeptId { get; set; }

        /// <summary>
        /// 用户账号
        ///</summary>
        [SugarColumn(ColumnName = "user_name")]
        public string UserName { get; set; }

        /// <summary>
        /// 用户昵称
        ///</summary>
        [SugarColumn(ColumnName = "nick_name")]
        public string NickName { get; set; }

        /// <summary>
        /// 用户类型（00系统用户）
        /// 默认值: 00
        ///</summary>
        [SugarColumn(ColumnName = "user_type")]
        public string UserType { get; set; }

        /// <summary>
        /// 用户邮箱
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "email")]
        public string Email { get; set; }

        /// <summary>
        /// 手机号码
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "phonenumber")]
        public string Phonenumber { get; set; }

        /// <summary>
        /// 用户性别（0男 1女 2未知）
        /// 默认值: 0
        ///</summary>
        [SugarColumn(ColumnName = "sex")]
        public string Sex { get; set; }

        /// <summary>
        /// 头像地址
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "avatar")]
        public string Avatar { get; set; }

        /// <summary>
        /// 密码
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "password")]
        public string Password { get; set; }

        /// <summary>
        /// 帐号状态（0正常 1停用）
        /// 默认值: 0
        ///</summary>
        [SugarColumn(ColumnName = "status")]
        public string Status { get; set; }

        /// <summary>
        /// 删除标志（0代表存在 2代表删除）
        /// 默认值: 0
        ///</summary>
        [SugarColumn(ColumnName = "del_flag")]
        public string DelFlag { get; set; }

        /// <summary>
        /// 最后登录IP
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "login_ip")]
        public string LoginIp { get; set; }

        /// <summary>
        /// 最后登录时间
        ///</summary>
        [SugarColumn(ColumnName = "login_date")]
        public DateTime? LoginDate { get; set; }

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