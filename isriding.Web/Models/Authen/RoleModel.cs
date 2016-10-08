using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace isriding.Web.Models.Authen
{
    public class RoleModel
    {
        public RoleModel()
        {
            Search = new RoleSearchModel();
            //SchoolList = new List<SelectListItem>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int OrderSort { get; set; }
        public bool Enabled { get; set; }
        public int? CreateId { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public int? ModifyId { get; set; }
        public string ModifyBy { get; set; }
        public DateTime? ModifyTime { get; set; }
        public int? School_id { get; set; }
        public string SchoolName { get; set; }
        //public List<SelectListItem> SchoolList { get; set; }

        public RoleSearchModel Search { get; set; }
    }

    public class RoleSearchModel
    {
        public RoleSearchModel()
        {
            EnabledList = new List<SelectListItem> {
                new SelectListItem { Text = "--- 请选择 ---", Value = "-1", Selected = true },
                new SelectListItem {Text = "禁用", Value = "0"},
                new SelectListItem {Text = "启用", Value = "1"}
            };
            SchoolList = new List<SelectListItem>();
        }
        [Display(Name = "角色名称")]
        public string Name { get; set; }
        [Display(Name = "启用")]
        public bool Enabled { get; set; }
        [Display(Name = "校园")]
        public int School_id { get; set; }
        public List<SelectListItem> SchoolList { get; set; }

        public List<SelectListItem> EnabledList { get; set; }
    }
}