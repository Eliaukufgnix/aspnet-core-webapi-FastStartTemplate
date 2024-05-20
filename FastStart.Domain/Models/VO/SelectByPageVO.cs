namespace FastStart.Domain.Models
{
    public class SelectByPageVO<T>
    {
        public SelectByPageVO(List<T> list, int total)
        {
            this.list = list;
            this.total = total;
        }

        public List<T> list { get; set; }
        public int total { get; set; }
    }
}