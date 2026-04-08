namespace FastStart.Domain.Models
{
    public class BasePageDTO
    {
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