﻿using AntDesign;
using AntDesign.TableModels;
using eXtensionSharp;
using Jina.Domain.SharedKernel.Abstract;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using static Jina.Passion.FE.Share.Consts.StorageConst;

namespace Jina.Passion.FE.Client.Base
{
    public abstract partial class TablePageComponentBase : PageComponentBase
    {
        protected int PageNo { get; set; }
        protected int PageSize { get; set; }
        protected int TotalPages { get; set; }

        protected virtual async Task OnSearch<T>(QueryModel<T> query, Func<int, int, string, string, Task<IPaginatedResult>> callback)
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

    public abstract partial class FormPageComponentBase : PageComponentBase
    {
        protected virtual async Task OnSearch(Func<Task<IResultBase>> callback)
        {
            if (callback.xIsEmpty()) return;

            this.Loading = true;

            var result = await callback();
            await this.StateChangedAsync();

            this.Loading = false;
        }
    }

    public abstract partial class PageComponentBase : ComponentBase
    {
        protected CurrentRole CurrentRole { get; }
        protected string CurrentUrl;
        protected List<string> BreadcrumbItems;
        protected bool Loading = false;
        protected readonly int Interval = 500;

        [Inject] protected NavigationManager NavigationManager { get; set; }
        [Inject] protected NotificationService NotificationService { get; set; }
        [Inject] protected IJSRuntime JsRuntime { get; set; }
        [Inject] protected IConfirmService ConfirmService { get; set; }

        [Parameter]
        public EventCallback<string> OnTabChange { get; set; }

        protected override async Task OnInitializedAsync()
        {
            BreadcrumbItems = new List<string>();
            CurrentUrl = NavigationManager.Uri;
            var myUrl = CurrentUrl.Replace(NavigationManager.BaseUri, "");
            BreadcrumbItems.Add("Home");
            var path = myUrl.Split('/');
            var count = 1;

            foreach (var link in path)
            {
                if (link == "") continue;
                count++;
                var lastLink = BreadcrumbItems.Last();
                BreadcrumbItems.Add(link);
            }

            await this.OnRoleSetupAsync();
            await this.OnBaseSetupAsync();
            await this.OnLoadAsync();
        }

        /// <summary>
        /// 권한 설정
        /// </summary>
        /// <returns></returns>
        protected virtual Task OnRoleSetupAsync()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// 기초 설정
        /// </summary>
        /// <returns></returns>
        protected virtual Task OnBaseSetupAsync()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// 데이터 로드
        /// </summary>
        /// <returns></returns>
        protected virtual Task OnLoadAsync()
        {
            return Task.CompletedTask;
        }

        protected virtual async Task ShowMessageAsync(string message, ENUM_MESSAGE_TYPE type = ENUM_MESSAGE_TYPE.Info)
        {
            switch (type)
            {
                case ENUM_MESSAGE_TYPE.Info:
                    await this.NotificationService.Info(new NotificationConfig()
                    {
                        Message = "Information",
                        Description = message
                    });
                    break;

                case ENUM_MESSAGE_TYPE.Success:
                    await this.NotificationService.Success(new NotificationConfig()
                    {
                        Message = "Success",
                        Description = message
                    });
                    break;

                case ENUM_MESSAGE_TYPE.Warning:
                    await this.NotificationService.Warning(new NotificationConfig()
                    {
                        Message = "Warning",
                        Description = message
                    });
                    break;

                case ENUM_MESSAGE_TYPE.Error:
                    await this.NotificationService.Error(new NotificationConfig()
                    {
                        Message = "Error",
                        Description = message
                    });
                    break;
            }
        }

        protected async Task InvokeVoidAsync(string command)
        {
            await JsRuntime.InvokeVoidAsync(command);
        }

        protected async Task InvokeVoidAsync(string command, string args)
        {
            await JsRuntime.InvokeVoidAsync(command, args);
        }

        protected async Task<T> InvokeAsync<T>(string command, string args)
        {
            return await JsRuntime.InvokeAsync<T>(command, args);
        }

        /// <summary>
        /// OnShouldRender를 재호출 하게 함.
        /// </summary>
        /// <returns></returns>
        protected async Task StateChangedAsync()
        {
            await this.InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// OnShouldRender를 재호출 하게 함.
        /// </summary>
        /// <returns></returns>
        protected void StateChanged()
        {
            StateHasChanged();
        }

        protected virtual async Task ShowConfirmAsync(string title, string message, ConfirmIcon icon = ConfirmIcon.Info)
        {
            await ConfirmService.Show(
                message,
                title,
                ConfirmButtons.OK,
                icon);
        }

        protected virtual async Task<bool> ShowConfirmOkCancelAsync(string title, string message, ConfirmIcon icon = ConfirmIcon.Info)
        {
            var result = await ConfirmService.Show(
                message,
                title,
                ConfirmButtons.OKCancel,
                icon);

            return result == ConfirmResult.OK || result == ConfirmResult.Yes;
        }

        protected async Task OnKeyUp(KeyboardEventArgs e)
        {
            if (e.Code == "Enter" || e.Code == "NumpadEnter")
            {
                await this.ShowMessageAsync(e.Code);
            }
            else if (e.AltKey == true && e.Code == "F1")
            {
                await this.ShowMessageAsync(e.Code);
            }
            else if (e.AltKey && e.Code == "F2")
            {
                await this.ShowMessageAsync(e.Code);
            }
        }
    }

    /// <summary>
    /// 메세지 타입
    /// </summary>
    public enum ENUM_MESSAGE_TYPE
    {
        Info,
        Success,
        Error,
        Warning
    }
}