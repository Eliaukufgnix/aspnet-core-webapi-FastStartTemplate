using SqlSugar;

namespace FastStart.Domain.Entity
{
    /// <summary>
    /// 部门表
    ///</summary>
    [SugarTable("sys_dept")]
    public class SysDept
    {
        /// <summary>
        /// 部门id
        ///</summary>
        [SugarColumn(ColumnName = "dept_id", IsPrimaryKey = true, IsIdentity = true)]
        public long DeptId { get; set; }

        /// <summary>
        /// 父部门id
        /// 默认值: 0
        ///</summary>
        [SugarColumn(ColumnName = "parent_id")]
        public long? ParentId { get; set; }

        /// <summary>
        /// 祖级列表
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "ancestors")]
        public string Ancestors { get; set; }

        /// <summary>
        /// 部门名称
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "dept_name")]
        public string DeptName { get; set; }

        /// <summary>
        /// 显示顺序
        /// 默认值: 0
        ///</summary>
        [SugarColumn(ColumnName = "order_num")]
        public int? OrderNum { get; set; }

        /// <summary>
        /// 负责人
        ///</summary>
        [SugarColumn(ColumnName = "leader")]
        public string Leader { get; set; }

        /// <summary>
        /// 联系电话
        ///</summary>
        [SugarColumn(ColumnName = "phone")]
        public string Phone { get; set; }

        /// <summary>
        /// 邮箱
        ///</summary>
        [SugarColumn(ColumnName = "email")]
        public string Email { get; set; }

        /// <summary>
        /// 部门状态（0正常 1停用）
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
    }
}