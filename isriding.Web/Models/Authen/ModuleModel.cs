using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace isriding.Web.Models.Authen
{
    public class ModuleModel
    {
        public ModuleModel()
        {
            Enabled = true;
            IsMenu = true;
            ParentModuleItems = new List<SelectListItem>();
            SchoolList = new List<SelectListItem>();
            Search = new ModuleSearchModel();
        }

        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string ParentName { get; set; }
        public string Name { get; set; }
        public string LinkUrl { get; set; }
        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Icon { get; set; }
        public string Code { get; set; }
        public int OrderSort { get; set; }
        public string Description { get; set; }
        public bool IsMenu { get; set; }
        public bool Enabled { get; set; }
        public int? School_id { get; set; }
        public string SchoolName { get; set; }
        public List<SelectListItem> SchoolList { get; set; }

        public ModuleSearchModel Search { get; set; }

        public List<SelectListItem> ParentModuleItems { get; set; }
    }

    public class ModuleSearchModel
    {
        public ModuleSearchModel()
        {
            EnabledList = new List<SelectListItem> {
                new SelectListItem { Text = "--- 请选择 ---", Value = "-1", Selected = true },
                new SelectListItem {Text = "禁用", Value = "0"},
                new SelectListItem {Text = "启用", Value = "1"}
            };

            IsMenuList = new List<SelectListItem> {
                new SelectListItem { Text = "--- 请选择 ---", Value = "-1", Selected = true },
                new SelectListItem {Text = "否", Value = "0"},
                new SelectListItem {Text = "是", Value = "1"}
            };
            SchoolList = new List<SelectListItem>();
        }
        [Display(Name = "菜单(模块)名称")]
        public string Name { get; set; }
        [Display(Name = "编码")]
        public string Code { get; set; }
        [Display(Name = "菜单")]
        public bool IsMenu { get; set; }
        [Display(Name = "启用")]
        public bool Enabled { get; set; }
        [Display(Name = "校园")]
        public int School_id { get; set; }

        public List<SelectListItem> IsMenuList { get; set; }
        public List<SelectListItem> EnabledList { get; set; }
        public List<SelectListItem> SchoolList { get; set; }
    }
}