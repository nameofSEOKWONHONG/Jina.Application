using AntDesign.ProLayout;
using eXtensionSharp;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.SignalR.Client;

namespace Jina.Passion.Client.Layout.ViewModels
{
    public class NotificationViewModel
    {
        public NoticeIcon NoticeIcon { get; set; }
        public int Count { get; set; }
        public List<NoticeIconData> FavoriteMenus { get; set; } = new();
        private HubConnection _hubConnection;
        private NavigationManager _navigation;

        public NotificationViewModel(NavigationManager navigationManager)
        {
            _navigation = navigationManager;
        }

        public async Task InitializeAsync()
        {
            var uri = _navigation.ToAbsoluteUri("/messageHub");
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(uri)
                .Build();

            _hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                var encodedMsg = $"{user}: {message}";
                // message process
                this.AddFavoriteMenu(new NoticeIconData()
                {
                    Title = user,
                    Description = message,
                    Avatar = "T",
                    Key = Guid.NewGuid().ToString("N"),
                    Read = false,
                    Datetime = DateTime.Now,
                });

                if (OnChange.xIsNotEmpty())
                {
                    OnChange();
                }
            });

            await _hubConnection.StartAsync();
        }

        public void AddFavoriteMenu(NoticeIconData data)
        {
            FavoriteMenus.Add(data);
            Count = FavoriteMenus.Count;
            if (OnChange.xIsNotEmpty())
            {
                OnChange();
            }
        }

        public void ClearFavorite()
        {
            if (FavoriteMenus.Count <= 0)
            {
                Count = 0;
                if (OnChange.xIsNotEmpty())
                {
                    OnChange();
                }
                return;
            }

            Count -= FavoriteMenus.Count;
            FavoriteMenus.Clear();
            if (OnChange.xIsNotEmpty())
            {
                OnChange();
            }
        }

        public Action OnChange { get; set; }
    }
}