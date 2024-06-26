﻿using System.Runtime.InteropServices;
using AntDesign;
using eXtensionSharp;
using Jina.Passion.Client.Base;
using Jina.Passion.Client.Base.Abstract;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace Jina.Passion.Client.Client.Base
{
    /// <summary>
    /// 일반 페이지
    /// </summary>
    public abstract class PageComponentBase : ComponentBase
    {
        protected CurrentRole CurrentRole { get; }
        protected string CurrentUrl;
        protected List<string> BreadcrumbItems;
        protected bool Loading = false;
        protected readonly int Interval = 500;

        [Inject] protected NavigationManager NavigationManager { get; set; }
        [Inject] protected NotificationService NotificationService { get; set; }
        [Inject] protected IJSRuntime Js { get; set; }
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

            await this.OnRoleAsync();
            await this.OnSetupAsync();
            await this.OnLoadAsync();
        }

        /// <summary>
        /// 권한 설정
        /// </summary>
        /// <returns></returns>
        protected virtual Task OnRoleAsync()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// 기초 설정
        /// </summary>
        /// <returns></returns>
        protected virtual Task OnSetupAsync()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// 시작 데이터 로드
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
            await Js.InvokeVoidAsync(command);
        }

        protected async Task InvokeVoidAsync(string command, string args)
        {
            await Js.InvokeVoidAsync(command, args);
        }

        protected async Task<T> InvokeAsync<T>(string command, string args)
        {
            return await Js.InvokeAsync<T>(command, args);
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
    /// 모달 지원 페이지
    /// </summary>
    /// <typeparam name="TOption"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public abstract class PageComponentBase<TOption, TResult> : FeedbackComponent<TOption, TResult>
    {
        protected CurrentRole CurrentRole { get; }
        protected string CurrentUrl;
        protected List<string> BreadcrumbItems;
        [Obsolete("don't use", true)]
        protected bool Loading = false;
        protected readonly int Interval = 500;

        [Inject] protected NavigationManager NavigationManager { get; set; }
        [Inject] protected NotificationService NotificationService { get; set; }
        [Inject] protected ModalService ModalService { get; set; }

        [Inject] protected IConfirmService ConfirmService { get; set; }
        [Inject] protected ISpinService SpinService { get; set; }

        [Parameter]
        public EventCallback<string> OnTabChange { get; set; }

        protected override async Task OnInitializedAsync()
        {
            SpinService.Show();
            
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

            if (this.Options.xIsNotEmpty())
            {
                await this.OnDlgParameterSetupAsync();
            }
            await this.OnRoleAsync();
            await this.OnSetupAsync();
            await this.OnLoadAsync();
            
            this.SpinService.Close();
        }

        /// <summary>
        /// 페이지 다이얼로그 파라메터 유무를 체크하여 실행합니다. 파라메터에 대한 할당을 구현합니다.
        /// </summary>
        /// <returns></returns>
        protected virtual Task OnDlgParameterSetupAsync()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// 권한 설정을 구현합니다.
        /// </summary>
        /// <returns></returns>
        protected virtual Task OnRoleAsync()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// 페이지에 필요한 기초 데이터를 설정을 구현합니다.
        /// </summary>
        /// <returns></returns>
        protected virtual Task OnSetupAsync()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// 페이지 초기에 필요한 부수 로직을 구현합니다.
        /// 특수한 경우에만 재정의 합니다. 일반적으로는 호출과 동시에 OnSearch를 호출합니다.
        /// </summary>
        /// <returns></returns>
        protected virtual Task OnLoadAsync()
        {
            return Task.CompletedTask;
        }

        protected virtual async Task ShowMessageAsync(string message, ENUM_MESSAGE_TYPE type = ENUM_MESSAGE_TYPE.Info)
        {
            const int duration = 2;
            switch (type)
            {
                case ENUM_MESSAGE_TYPE.Info:
                    await this.NotificationService.Info(new NotificationConfig()
                    {
                        Duration = duration,
                        Message = "Information",
                        Description = message
                    });
                    break;

                case ENUM_MESSAGE_TYPE.Success:
                    await this.NotificationService.Success(new NotificationConfig()
                    {
                        Duration = duration,
                        Message = "Success",
                        Description = message
                    });
                    break;

                case ENUM_MESSAGE_TYPE.Warning:
                    await this.NotificationService.Warning(new NotificationConfig()
                    {
                        Duration = duration,
                        Message = "Warning",
                        Description = message
                    });
                    break;

                case ENUM_MESSAGE_TYPE.Error:
                    await this.NotificationService.Error(new NotificationConfig()
                    {
                        Duration = duration,
                        Message = "Error",
                        Description = message
                    });
                    break;
            }
        }

        protected async Task InvokeVoidAsync(string command)
        {
            await Js.InvokeVoidAsync(command);
        }

        protected async Task InvokeVoidAsync(string command, string args)
        {
            await Js.InvokeVoidAsync(command, args);
        }

        protected async Task<T> InvokeAsync<T>(string command, string args)
        {
            return await Js.InvokeAsync<T>(command, args);
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