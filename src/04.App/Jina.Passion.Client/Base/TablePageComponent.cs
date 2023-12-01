using AntDesign;
using AntDesign.TableModels;
using eXtensionSharp;
using Jina.Domain.Example;
using Jina.Domain.SharedKernel.Abstract;
using Jina.Passion.Client.Base.Abstract;
using Jina.Passion.Client.Client.Base;

namespace Jina.Passion.Client.Base
{
    public abstract class TablePageComponent<TOption, TResult> : PageComponentBase<TOption, TResult>
        where TOption : DlgOptionsBase
    {
        protected int PageNo { get; set; }
        protected int PageSize { get; set; }
        protected int TotalPages { get; set; }

        protected string DefaultPagingPosition = "bottomRight";

        public virtual async Task OnSearch<T>(QueryModel<T> query, Func<int, int, string, string, Task<IPaginatedResult>> callback)
        {
            if (callback.xIsEmpty()) return;

            this.
                Loading = true;

            await Task.Delay(this.Interval);

            IPaginatedResult result = null;
            if (query.SortModel.xIsNotEmpty())
            {
                var sort = query.SortModel.FirstOrDefault(m => m.Sort.xIsNotEmpty());
                if (sort.xIsNotEmpty())
                {
                    if (sort.Sort == "ascend")
                    {
                        result = await callback(query.PageIndex, query.PageSize, sort!.FieldName, "ASC");
                    }
                    else
                    {
                        result = await callback(query.PageIndex, query.PageSize, sort!.FieldName, "DESC");
                    }
                }
                else
                {
                    result = await callback(query.PageIndex, query.PageSize, string.Empty, string.Empty);
                }
            }
            else
            {
                result = await callback(query.PageIndex, query.PageSize, string.Empty, string.Empty);
            }

            if (result.Succeeded)
            {
                this.TotalPages = result.TotalCount;
            }

            this.Loading = false;
        }
    }
}