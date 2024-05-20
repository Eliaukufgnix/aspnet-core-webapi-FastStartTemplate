namespace FastStart.Domain.Models
{
    public class SysUserDTO
    {
        public string? UserName { get; set; }
        public string? NickName { get; set; }
        public string? Email { get; set; }
        public string? Phonenumber { get; set; }
        public string? Sex { get; set; }
        public string? LoginIp { get; set; }
        public string? LoginDate { get; set; }
        public string? CreateBy { get; set; }
        public string? CreateTime { get; set; }
        public string? UpdateBy { get; set; }
        public string? UpdateTime { get; set; }
        private int? _pageIndex;

        public int pageIndex
        {
            get { return _pageIndex ?? 1; }
            set { _pageIndex = value; }
        }

        private int? _pageSize;

        public int pageSize
        {
            get { return _pageSize ?? 10; }
            set { _pageSize = value; }
        }
    }
}