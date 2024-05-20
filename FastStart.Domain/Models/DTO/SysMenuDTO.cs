namespace FastStart.Domain.Models
{
    public class SysMenuDTO
    {
        public long MenuId { get; set; }
        public long? ParentId { get; set; }
        public string? MenuName { get; set; }
        public string? Path { get; set; }
        public string? Component { get; set; }
        public string? Redirect { get; set; }
        public string? Query { get; set; }
        public string? Visible { get; set; }
        public string? Status { get; set; }
        public string? Perms { get; set; }
        public int? IsFrame { get; set; }
        public int? IsCache { get; set; }
        public string? MenuType { get; set; }
        public string? Icon { get; set; }
        public int? OrderNum { get; set; }
        public DateTime? CreateTime { get; set; }
    }
}