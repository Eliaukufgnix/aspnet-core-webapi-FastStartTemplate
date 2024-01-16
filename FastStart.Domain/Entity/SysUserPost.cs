using SqlSugar;

namespace FastStart.Domain.Entity
{
    /// <summary>
    /// 用户与岗位关联表
    ///</summary>
    [SugarTable("sys_user_post")]
    public class SysUserPost
    {
        /// <summary>
        /// 用户ID
        ///</summary>
        [SugarColumn(ColumnName = "user_id", IsPrimaryKey = true)]
        public long UserId { get; set; }

        /// <summary>
        /// 岗位ID
        ///</summary>
        [SugarColumn(ColumnName = "post_id", IsPrimaryKey = true)]
        public long PostId { get; set; }
    }
}