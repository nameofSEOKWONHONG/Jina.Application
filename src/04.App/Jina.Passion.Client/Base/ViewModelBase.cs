using System.Collections.Generic;

namespace Jina.Passion.Client.Base
{
    public abstract class ViewModelBase
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
    }

    public abstract class ViewModelBase<TDto> : ViewModelBase where TDto : class
    {
        public List<TDto> Items { get; set; }
        public TDto SelectedItem { get; set; }
        public IEnumerable<TDto> SelectedItems { get; set; }

        public ViewModelBase()
        {
        }
    }
}