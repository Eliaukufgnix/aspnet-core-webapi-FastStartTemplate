namespace FastStart.Domain.Models
{
    public class SysMenuVO
    {
        public string path { get; set; }
        public string component { get; set; }
        public string redirect { get; set; }
        public string name { get; set; }
        public Meta meta { get; set; }
        public List<SysMenuVO> children { get; set; }

        public SysMenuVO()
        {
            meta = new Meta();
        }
    }

    public class Meta
    {
        public string title { get; set; }
        public string icon { get; set; }
        public bool hidden { get; set; }
    }
}