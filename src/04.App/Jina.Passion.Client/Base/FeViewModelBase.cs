namespace Jina.Passion.Client.Base
{
    public abstract class FeViewModelBase
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public FeViewModelBase()
        {
        }
    }
}