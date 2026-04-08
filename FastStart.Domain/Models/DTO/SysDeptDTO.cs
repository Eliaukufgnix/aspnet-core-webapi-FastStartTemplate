namespace FastStart.Domain.Models
{
    public class SysDeptDTO : BasePageDTO
    {
        public long? ParentId { get; set; }
        public string? Ancestors { get; set; }
        public string? DeptName { get; set; }
        public int? OrderNum { get; set; }
        public string? Leader { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Status { get; set; }
        public string? DelFlag { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public string? UpdateBy { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}