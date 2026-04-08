namespace FastStart.Domain.Models
{
    public class SysPostDTO : BasePageDTO
    {
        public string? PostCode { get; set; }
        public string? PostName { get; set; }
        public int? PostSort { get; set; }
        public string? Status { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public string? UpdateBy { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string? Remark { get; set; }
    }
}