using SqlSugar;

namespace FastStart.Domain.Entity
{
    /// <summary>
    /// 角色和菜单关联表
    ///</summary>
    [SugarTable("sys_role_menu")]
    public class SysRoleMenu
    {
        /// <summary>
        /// 角色ID
        ///</summary>
        [SugarColumn(ColumnName = "role_id", IsPrimaryKey = true)]
        public long RoleId { get; set; }

        /// <summary>
        /// 菜单ID
        ///</summary>
        [SugarColumn(ColumnName = "menu_id", IsPrimaryKey = true)]
        public long MenuId { get; set; }
    }
}