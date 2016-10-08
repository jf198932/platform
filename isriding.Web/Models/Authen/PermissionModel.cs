using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using isriding.Web.Models.Common;

namespace isriding.Web.Models.Authen
{
    public class PermissionModel
    {
        public PermissionModel()
        {
            Search = new PermissionSearchModel();
            SchoolList = new List<SelectListItem>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int OrderSort { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }
        public int? School_id { get; set; }
        public string SchoolName { get; set; }

        public PermissionSearchModel Search { get; set; }
        public List<SelectListItem> SchoolList { get; set; }
    }

    public class PermissionSearchModel
    {
        public PermissionSearchModel()
        {
            EnabledList = new List<SelectListItem> {
                new SelectListItem { Text = "--- 请选择 ---", Value = "-1", Selected = true },
                new SelectListItem {Text = "禁用", Value = "0"},
                new SelectListItem {Text = "启用", Value = "1"}
            };
            SchoolList = new List<SelectListItem>();
        }
        [Display(Name = "按钮名称")]
        public string Name { get; set; }
        [Display(Name = "编码")]
        public string Code { get; set; }
        [Display(Name = "启用")]
        public bool Enabled { get; set; }
        [Display(Name = "校园")]
        public int School_id { get; set; }
        public List<SelectListItem> SchoolList { get; set; } 

        public List<SelectListItem> EnabledList { get; set; }

    }

    public class ButtonModel
    {
        public ButtonModel()
        {
            ButtonList = new List<KeyValueModel>();
            SelectedButtonList = new List<int>();
        }

        public int ModuleId { get; set; }

        public string ModuleName { get; set; }

        public ICollection<KeyValueModel> ButtonList { get; set; }

        [KeyValue(DisplayProperty = "ButtonList")]
        public ICollection<int> SelectedButtonList { get; set; }
    }

    public class PermissionButtonModel
    {
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
    }
}