namespace FastStart.Domain.Models
{
    public class SysDictDTO : BasePageDTO
    {
        public string? DictName { get; set; }
        public string? DictType { get; set; }
        public string? Status { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public string? UpdateBy { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string? Remark { get; set; }
    }
}