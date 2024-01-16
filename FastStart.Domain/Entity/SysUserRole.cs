using SqlSugar;

namespace FastStart.Domain.Entity
{
    /// <summary>
    /// 用户和角色关联表
    ///</summary>
    [SugarTable("sys_user_role")]
    public class SysUserRole
    {
        /// <summary>
        /// 用户ID
        ///</summary>
        [SugarColumn(ColumnName = "user_id", IsPrimaryKey = true)]
        public long UserId { get; set; }

        /// <summary>
        /// 角色ID
        ///</summary>
        [SugarColumn(ColumnName = "role_id", IsPrimaryKey = true)]
        public long RoleId { get; set; }
    }
}