using System.Collections.Generic;

namespace isriding.Web.Models.Common
{
    public class SideBarMenuModel
    {
        public SideBarMenuModel()
        {
            this.ChildMenus = new List<SideBarMenuModel>();
        }
        public int Id { get; set; }

        public string Name { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }

        public string Icon { get; set; }

        public bool Selected { get; set; }

        public List<SideBarMenuModel> ChildMenus { get; set; }
    }
}