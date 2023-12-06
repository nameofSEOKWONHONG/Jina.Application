namespace Jina.Passion.Client.Pages.Account.ViewModels
{
    public class MenuRoleViewModel
    {
    }

    public class MenuRoleTemplate
    {
        public string RoleName { get; set; }
        public List<MenuTemplate> Menus { get; set; }
    }

    public class MenuTemplate
    {
        public int Id { get; set; }
        public string MenuName { get; set; }
        public bool Read { get; set; }
        public bool Write { get; set; }
        public bool View { get; set; }
        public bool Export { get; set; }
        public bool Import { get; set; }
    }
}