using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace isriding.Web.Models.School
{
    public class UserModel
    {
        public UserModel()
        {
            Created_at = DateTime.Now;
            Updated_at = DateTime.Now;
            SchoolList = new List<SelectListItem>();
            CertificationList = new List<SelectListItem>();
            Search = new UserSearchModel();
        }

        public int Id { get; set; }
        public DateTime? Created_at { get; set; }
        public DateTime? Updated_at { get; set; }
        public string Phone { get; set; }
        public string Name { get; set; }
        public string Nickname { get; set; }
        public string Weixacc { get; set; }
        public string Email { get; set; }
        public int? Certification { get; set; }
        public int? Textmsg { get; set; }
        public DateTime? Textmsg_time { get; set; }
        public string Remember_token { get; set; }
        public int? Credits { get; set; }
        public int? Balance { get; set; }
        public int? School_id { get; set; }
        public string School_name { get; set; }

        public UserSearchModel Search { get; set; }

        public List<SelectListItem> SchoolList { get; set; }
        public List<SelectListItem> CertificationList { get; set; }
    }

    public class UserSearchModel
    {
        public UserSearchModel()
        {
            CertificationList = new List<SelectListItem>
            {
                new SelectListItem { Text = "--- 请选择 ---", Value = "0", Selected = true},
                new SelectListItem {Text = "未申请", Value = "1"},
                new SelectListItem {Text = "已申请", Value = "2"},
                new SelectListItem {Text = "已认证", Value = "3"},
                new SelectListItem {Text = "认证失败", Value = "4"}
            };
            SchoolList = new List<SelectListItem>();
        }
        [Display(Name = "手机号")]
        public string Phone { get; set; }
        [Display(Name = "用户名")]
        public string Name { get; set; }
        [Display(Name = "昵称")]
        public string Nickname { get; set; }
        [Display(Name = "认证状态")]
        public int? Certification { get; set; }
        [Display(Name = "学校")]
        public int School_id { get; set; }

        public List<SelectListItem> CertificationList { get; set; }
        public List<SelectListItem> SchoolList { get; set; }
    }
}