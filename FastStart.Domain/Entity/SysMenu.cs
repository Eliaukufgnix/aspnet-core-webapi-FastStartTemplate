using SqlSugar;

namespace FastStart.Domain.Entity
{
    /// <summary>
    /// 菜单权限表
    ///</summary>
    [SugarTable("sys_menu")]
    public class SysMenu
    {
        /// <summary>
        /// 菜单ID
        ///</summary>
        [SugarColumn(ColumnName = "menu_id", IsPrimaryKey = true, IsIdentity = true)]
        public long MenuId { get; set; }

        /// <summary>
        /// 菜单名称
        ///</summary>
        [SugarColumn(ColumnName = "menu_name")]
        public string MenuName { get; set; }

        /// <summary>
        /// 父菜单ID
        /// 默认值: 0
        ///</summary>
        [SugarColumn(ColumnName = "parent_id")]
        public long? ParentId { get; set; }

        /// <summary>
        /// 显示顺序
        /// 默认值: 0
        ///</summary>
        [SugarColumn(ColumnName = "order_num")]
        public int? OrderNum { get; set; }

        /// <summary>
        /// 路由地址
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "path")]
        public string Path { get; set; }

        /// <summary>
        /// 组件路径
        ///</summary>
        [SugarColumn(ColumnName = "component")]
        public string Component { get; set; }

        /// <summary>
        /// 路由参数
        ///</summary>
        [SugarColumn(ColumnName = "query")]
        public string Query { get; set; }

        /// <summary>
        /// 是否为外链（0是 1否）
        /// 默认值: 1
        ///</summary>
        [SugarColumn(ColumnName = "is_frame")]
        public int? IsFrame { get; set; }

        /// <summary>
        /// 是否缓存（0缓存 1不缓存）
        /// 默认值: 0
        ///</summary>
        [SugarColumn(ColumnName = "is_cache")]
        public int? IsCache { get; set; }

        /// <summary>
        /// 菜单类型（M目录 C菜单 F按钮）
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "menu_type")]
        public string MenuType { get; set; }

        /// <summary>
        /// 菜单状态（0显示 1隐藏）
        /// 默认值: 0
        ///</summary>
        [SugarColumn(ColumnName = "visible")]
        public string Visible { get; set; }

        /// <summary>
        /// 菜单状态（0正常 1停用）
        /// 默认值: 0
        ///</summary>
        [SugarColumn(ColumnName = "status")]
        public string Status { get; set; }

        /// <summary>
        /// 权限标识
        ///</summary>
        [SugarColumn(ColumnName = "perms")]
        public string Perms { get; set; }

        /// <summary>
        /// 菜单图标
        /// 默认值: #
        ///</summary>
        [SugarColumn(ColumnName = "icon")]
        public string Icon { get; set; }

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
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "remark")]
        public string Remark { get; set; }
    }
}