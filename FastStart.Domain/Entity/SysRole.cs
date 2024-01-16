using SqlSugar;

namespace FastStart.Domain.Entity
{
    /// <summary>
    /// 角色信息表
    ///</summary>
    [SugarTable("sys_role")]
    public class SysRole
    {
        /// <summary>
        /// 角色ID
        ///</summary>
        [SugarColumn(ColumnName = "role_id", IsPrimaryKey = true, IsIdentity = true)]
        public long RoleId { get; set; }

        /// <summary>
        /// 角色名称
        ///</summary>
        [SugarColumn(ColumnName = "role_name")]
        public string RoleName { get; set; }

        /// <summary>
        /// 角色权限字符串
        ///</summary>
        [SugarColumn(ColumnName = "role_key")]
        public string RoleKey { get; set; }

        /// <summary>
        /// 显示顺序
        ///</summary>
        [SugarColumn(ColumnName = "role_sort")]
        public int RoleSort { get; set; }

        /// <summary>
        /// 数据范围（1：全部数据权限 2：自定数据权限 3：本部门数据权限 4：本部门及以下数据权限）
        /// 默认值: 1
        ///</summary>
        [SugarColumn(ColumnName = "data_scope")]
        public string DataScope { get; set; }

        /// <summary>
        /// 菜单树选择项是否关联显示
        /// 默认值: 1
        ///</summary>
        [SugarColumn(ColumnName = "menu_check_strictly")]
        public byte? MenuCheckStrictly { get; set; }

        /// <summary>
        /// 部门树选择项是否关联显示
        /// 默认值: 1
        ///</summary>
        [SugarColumn(ColumnName = "dept_check_strictly")]
        public byte? DeptCheckStrictly { get; set; }

        /// <summary>
        /// 角色状态（0正常 1停用）
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