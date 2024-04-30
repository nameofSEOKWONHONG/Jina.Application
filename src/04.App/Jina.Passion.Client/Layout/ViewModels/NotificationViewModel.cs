using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AntDesign;
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
        private INotificationService _notificationService;

        public NotificationViewModel(NavigationManager navigationManager, INotificationService notificationService)
        {
            _navigation = navigationManager;
            _notificationService = notificationService;
        }

        public async Task InitializeAsync()
        {
            var uri = "https://localhost:7103/messageHub";
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(uri)
                .Build();

            _hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                var encodedMsg = $"{user}: {message}";
                var notiData = new NoticeIconData()
                {
                    Title = user,
                    Description = message,
                    Avatar = "T",
                    Key = Guid.NewGuid().ToString("N"),
                    Read = false,
                    Datetime = DateTime.Now,
                };
                // message process
                this.AddFavoriteMenu(notiData);
                return Task.CompletedTask;
            });

            await _hubConnection.StartAsync();
        }

        public void AddFavoriteMenu(NoticeIconData data)
        {
            FavoriteMenus.Add(data);
            Count = FavoriteMenus.Count;
            if (OnChange.xIsNotEmpty())
            {
                OnChange(data);
            }
        }

        public void ClearFavorite()
        {
            if (FavoriteMenus.Count <= 0)
            {
                Count = 0;
                if (OnChange.xIsNotEmpty())
                {
                    OnChange(null);
                }
                return;
            }

            Count -= FavoriteMenus.Count;
            FavoriteMenus.Clear();
            if (OnChange.xIsNotEmpty())
            {
                OnChange(null);
            }
        }

        public Action<NoticeIconData> OnChange { get; set; }
    }
}