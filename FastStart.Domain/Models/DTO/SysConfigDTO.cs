namespace FastStart.Domain.Models
{
    public class SysConfigDTO : BasePageDTO
    {
        public string? ConfigName { get; set; }
        public string? ConfigKey { get; set; }
        public string? ConfigValue { get; set; }
        public string? ConfigType { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public string? UpdateBy { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string? Remark { get; set; }
    }
}
