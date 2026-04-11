namespace FastStart.Domain.Models
{
    public class SysOperLogDTO : BasePageDTO
    {
        public string? Title { get; set; }
        public int? BusinessType { get; set; }
        public string? Method { get; set; }
        public string? RequestMethod { get; set; }
        public int? OperatorType { get; set; }
        public string? OperName { get; set; }
        public string? DeptName { get; set; }
        public string? OperUrl { get; set; }
        public string? OperIp { get; set; }
        public string? OperLocation { get; set; }
        public string? OperParam { get; set; }
        public string? JsonResult { get; set; }
        public int? Status { get; set; }
        public string? ErrorMsg { get; set; }
        public DateTime? OperTime { get; set; }
        public long? CostTime { get; set; }
    }
}