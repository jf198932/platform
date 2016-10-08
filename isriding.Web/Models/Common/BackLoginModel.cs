using System;
using System.Collections.Generic;
using Abp.AutoMapper;
using isriding.Entities.Authen;
using isriding.Web.Models.Authen;

namespace isriding.Web.Models.Common
{
    [AutoMapFrom(typeof(BackUser))]
    public class BackLoginModel
    {
        public BackLoginModel()
        {
            Menus = new List<SideBarMenuModel>();
            Buttons = new List<PermissionButtonModel>();
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
        public int? School_id { get; set; }

        public List<SideBarMenuModel> Menus { get; set; }

        public List<PermissionButtonModel> Buttons { get; set; }
    }
}