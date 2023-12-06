using AntDesign;
using AntDesign.TableModels;
using eXtensionSharp;
using Jina.Domain.Account;
using Jina.Domain.Example;
using Jina.Domain.SharedKernel.Abstract;
using Jina.Passion.Client.Base.Abstract;
using Jina.Passion.Client.Client.Base;
using Jina.Passion.Client.Pages.Account.Contents;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Jina.Passion.Client.Base
{
    public class ViewPaginateion
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int Total { get; set; }

        public string DefaultPagingPosition = "bottomRight";
    }

    public abstract class TablePageComponent<TDto, TOption, TResult> : PageComponentBase<TOption, TResult>
    {
        protected readonly ViewPaginateion Paginateion = new ViewPaginateion();

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
                            result = await callback(this.Paginateion.PageIndex, this.Paginateion.PageSize, sort!.FieldName, "ASC");
                        }
                        else
                        {
                            result = await callback(this.Paginateion.PageIndex, this.Paginateion.PageSize, sort!.FieldName, "DESC");
                        }
                    }
                    else
                    {
                        result = await callback(this.Paginateion.PageIndex, this.Paginateion.PageSize, string.Empty, string.Empty);
                    }
                }
                else
                {
                    result = await callback(this.Paginateion.PageIndex, this.Paginateion.PageSize, string.Empty, string.Empty);
                }
            }
            else
            {
                result = await callback(this.Paginateion.PageIndex, this.Paginateion.PageSize, string.Empty, string.Empty);
            }

            if (result.Succeeded)
            {
                this.Paginateion.Total = result.TotalCount;
            }

            this.Loading = false;
        }

        public virtual async Task OnRemove(Func<Task<IResultBase<bool>>> callback)
        {
            this.Loading = true;
            var result = await callback();
            if (result.Succeeded)
            {
                await this.ShowMessageAsync("삭제 되었습니다.", ENUM_MESSAGE_TYPE.Success);
            }
            this.Loading = false;
        }

        public async Task AddItem(MouseEventArgs e)
        {
            await OnAddItem(null);
        }

        public virtual async Task OnAddItem(Func<Task<IResultBase>> callback)
        {
            this.Loading = true;

            var result = await callback();
            if (result.Succeeded)
            {
                await this.ShowMessageAsync("저장 되었습니다.", ENUM_MESSAGE_TYPE.Success);
            }

            this.Loading = false;
        }
    }

    public abstract class TablePageComponent<TDto, TViewModel, TOption, TResult> : TablePageComponent<TDto, TOption, TResult>
        where TDto : class
        where TViewModel : FeViewModelBase<TDto>
        where TOption : DlgOptionsBase
    {
        [Inject]
        public TViewModel ViewModel { get; set; }

        public async Task RemoveRange(MouseEventArgs e)
        {
            await OnRemoveRange(null);
        }

        public virtual async Task OnRemoveRange(Func<Task<IResultBase<bool>>> callback)
        {
            if (this.ViewModel.SelectedItems.xIsEmpty())
            {
                await this.ShowMessageAsync("선택된 내역이 없습니다.", ENUM_MESSAGE_TYPE.Error);
                return;
            }
            this.Loading = true;

            var result = await callback();
            if (result.Succeeded)
            {
                await this.ShowMessageAsync("삭제 되었습니다.", ENUM_MESSAGE_TYPE.Success);
            }

            this.Loading = false;
        }

        public virtual void OnSelectChanged(RowData<TDto> item)
        {
            this.ViewModel.SelectedItem = item.Data;
            Console.WriteLine(this.ViewModel.SelectedItem.xToJson());
        }
    }
}