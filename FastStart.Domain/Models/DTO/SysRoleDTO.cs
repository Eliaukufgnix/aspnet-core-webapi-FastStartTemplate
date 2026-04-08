namespace FastStart.Domain.Models.DTO
{
    public class SysRoleDTO : BasePageDTO
    {
        public long? RoleId { get; set; }
        public string? RoleName { get; set; }
        public string? RoleKey { get; set; }
        public int? RoleSort { get; set; }
        public string? DataScope { get; set; }
        public byte? MenuCheckStrictly { get; set; }
        public byte? DeptCheckStrictly { get; set; }
        public string? Status { get; set; }
        public string? DelFlag { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public string? UpdateBy { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string? Remark { get; set; }
    }
}