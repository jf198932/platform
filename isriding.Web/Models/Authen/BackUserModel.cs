using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using isriding.Web.Models.Common;

namespace isriding.Web.Models.Authen
{
    public class BackUserModel
    {
        public BackUserModel()
        {
            RoleList = new List<KeyValueModel>();
            SelectedRoleList = new List<int>();
            SchoolList = new List<SelectListItem>();
            Enabled = true;
            Search = new BackUserSearchModel();
        }

        public int Id { get; set; }
        public string LoginName { get; set; }
        public string LoginPwd { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool Enabled { get; set; }
        public int PwdErrorCount { get; set; }
        public int LoginCount { get; set; }
        public DateTime? RegisterTime { get; set; }
        public DateTime? LastLoginTime { get; set; }

        public BackUserSearchModel Search { get; set; }

        public List<SelectListItem> SchoolList { get; set; }

        public ICollection<KeyValueModel> RoleList { get; set; }
        [KeyValue(DisplayProperty = "RoleList")]
        public ICollection<int> SelectedRoleList { get; set; }
    }

    public class BackUserSearchModel
    {
        public BackUserSearchModel()
        {
            EnabledList = new List<SelectListItem> {
                new SelectListItem { Text = "--- 请选择 ---", Value = "-1", Selected = true },
                new SelectListItem {Text = "禁用", Value = "0"},
                new SelectListItem {Text = "启用", Value = "1"}
            };
            SchoolList = new List<SelectListItem>();
        }
        [Display(Name = "启用")]
        public bool Enabled { get; set; }

        [Display(Name = "登录名")]
        public string LoginName { get; set; }

        [Display(Name = "全名")]
        public string FullName { get; set; }
        [Display(Name = "校园")]
        public int School_id { get; set; }

        public List<SelectListItem> SchoolList { get; set; }

        public List<SelectListItem> EnabledList { get; set; }
    }
}