using System.Collections.Generic;

namespace Jina.Passion.Client.Base
{
    public abstract class FeViewModelBase
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
    }

    public abstract class FeViewModelBase<Dto> : FeViewModelBase where Dto : class
    {
        public List<Dto> Items { get; set; }
        public Dto SelectedItem { get; set; }
        public IEnumerable<Dto> SelectedItems { get; set; }

        public FeViewModelBase()
        {
        }
    }
}