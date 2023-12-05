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
        protected int PageIndex { get; set; } = 1;
        protected int PageSize { get; set; } = 10;
        protected int Total { get; set; }

        protected string DefaultPagingPosition = "bottomRight";

        protected virtual async Task OnTableChange(QueryModel query)
        {
            await OnSearch(query, null);
        }

        public virtual async Task OnSearch(QueryModel query, Func<int, int, string, string, Task<IPaginatedResult>> callback)
        {
            if (callback.xIsEmpty()) return;

            this.
                Loading = true;

            await Task.Delay(this.Interval);

            IPaginatedResult result = null;
            if (query.xIsNotEmpty())
            {
                if (query.SortModel.xIsNotEmpty())
                {
                    var sort = query.SortModel.FirstOrDefault(m => m.Sort.xIsNotEmpty());
                    if (sort.xIsNotEmpty())
                    {
                        if (sort.Sort == "ascend")
                        {
                            result = await callback(this.PageIndex, this.PageSize, sort!.FieldName, "ASC");
                        }
                        else
                        {
                            result = await callback(this.PageIndex, this.PageSize, sort!.FieldName, "DESC");
                        }
                    }
                    else
                    {
                        result = await callback(this.PageIndex, this.PageSize, string.Empty, string.Empty);
                    }
                }
                else
                {
                    result = await callback(this.PageIndex, this.PageSize, string.Empty, string.Empty);
                }
            }
            else
            {
                result = await callback(this.PageIndex, this.PageSize, string.Empty, string.Empty);
            }

            if (result.Succeeded)
            {
                this.Total = result.TotalCount;
            }

            this.Loading = false;
        }
    }
}