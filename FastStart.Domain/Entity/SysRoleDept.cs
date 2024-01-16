using SqlSugar;

namespace FastStart.Domain.Entity
{
    /// <summary>
    /// 角色和部门关联表
    ///</summary>
    [SugarTable("sys_role_dept")]
    public class SysRoleDept
    {
        /// <summary>
        /// 角色ID
        ///</summary>
        [SugarColumn(ColumnName = "role_id", IsPrimaryKey = true)]
        public long RoleId { get; set; }

        /// <summary>
        /// 部门ID
        ///</summary>
        [SugarColumn(ColumnName = "dept_id", IsPrimaryKey = true)]
        public long DeptId { get; set; }
    }
}