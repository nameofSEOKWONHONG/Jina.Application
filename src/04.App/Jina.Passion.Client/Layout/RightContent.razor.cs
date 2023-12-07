using AntDesign;
using AntDesign.ProLayout;
using eXtensionSharp;
using Jina.Passion.Client.Layout.ViewModels;
using Microsoft.AspNetCore.Components;

namespace Jina.Passion.Client.Layout
{
    public partial class RightContent
    {
        private List<NoticeIconData> _notifications = new();
        private List<NoticeIconData> _messages = new();
        private List<NoticeIconData> _events = new();
        private int _count = 0;

        [Inject] protected NavigationManager NavigationManager { get; set; }
        [Inject] protected MessageService MessageService { get; set; }
        [Inject] protected NotificationViewModel ViewModel { get; set; }

        private List<AutoCompleteDataItem<string>> DefaultOptions { get; set; } = new List<AutoCompleteDataItem<string>>
        {
            new AutoCompleteDataItem<string>
            {
                Label = "umi ui",
                Value = "umi ui"
            },
            new AutoCompleteDataItem<string>
            {
                Label = "Pro Table",
                Value = "Pro Table"
            },
            new AutoCompleteDataItem<string>
            {
                Label = "Pro Layout",
                Value = "Pro Layout"
            }
        };

        public AvatarMenuItem[] AvatarMenuItems { get; set; } = new AvatarMenuItem[]
        {
            new() { Key = "center", IconType = "user", Option = "个人中心"},
            new() { Key = "setting", IconType = "setting", Option = "个人设置"},
            new() { IsDivider = true },
            new() { Key = "logout", IconType = "logout", Option = "退出登录"}
        };

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            SetClassMap();
            ViewModel.OnChange = () =>
            {
                this.InvokeStateHasChangedAsync();
            };
            //_currentUser = await UserService.GetCurrentUserAsync();
            //var notices = await ProjectService.GetNoticesAsync();
            //_notifications = notices.Where(x => x.Type == "notification").Cast<NoticeIconData>().ToArray();
            //_messages = notices.Where(x => x.Type == "message").Cast<NoticeIconData>().ToArray();
            //_events = notices.Where(x => x.Type == "event").Cast<NoticeIconData>().ToArray();
            //_count = notices.Length;
        }

        protected void SetClassMap()
        {
            ClassMapper
                .Clear()
                .Add("right");
        }

        public void HandleSelectUser(MenuItem item)
        {
            switch (item.Key)
            {
                case "center":
                    NavigationManager.NavigateTo("/account/center");
                    break;

                case "setting":
                    NavigationManager.NavigateTo("/account/settings");
                    break;

                case "logout":
                    NavigationManager.NavigateTo("/user/login");
                    break;
            }
        }

        public void HandleSelectLang(MenuItem item)
        {
        }

        public async Task HandleClear(string key)
        {
            switch (key)
            {
                case "notification":
                    ViewModel.ClearFavorite();
                    break;

                case "message":
                    _messages.Clear();
                    break;

                case "event":
                    _events.Clear();
                    break;
            }
            await MessageService.Success($"清空了{key}");
        }

        public async Task HandleViewMore(string key)
        {
            await MessageService.Info("Click on view more");
        }

        public void HandleItemClick(string e)
        {
            Console.WriteLine(e);
            NavigationManager.NavigateTo(e);
        }
    }
}