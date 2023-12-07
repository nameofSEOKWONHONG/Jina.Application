using AntDesign.ProLayout;
using eXtensionSharp;

namespace Jina.Passion.Client.Layout.ViewModels
{
    public class NotificationViewModel
    {
        public NoticeIcon NoticeIcon { get; set; }
        public int Count { get; set; }
        public List<NoticeIconData> FavoriteMenus { get; set; } = new();

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
            if (FavoriteMenus.Count <= 0) { return; }

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