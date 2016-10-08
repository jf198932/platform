using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace isriding.Web.Models.School
{
    public class VersionUpdateModel
    {
        public VersionUpdateModel()
        {
            UpgradeList = new List<SelectListItem>
            {
                new SelectListItem { Text = "--- 请选择 ---", Value = "0", Selected = true},
                new SelectListItem {Text = "不可升级", Value = "1"},
                new SelectListItem {Text = "可升级", Value = "2"},
                new SelectListItem {Text = "强制升级", Value = "3"}
            };
            DeviceList = new List<SelectListItem>
            {
                new SelectListItem { Text = "--- 请选择 ---", Value = "0", Selected = true},
                new SelectListItem {Text = "iOS", Value = "1"},
                new SelectListItem {Text = "Android", Value = "2"},
            };
            Search = new VersionUpdateSearchModel();
        }
        public int Id { get; set; }
        public int device_os { get; set; }
        public int versionCode { get; set; }
        public string versionName { get; set; }
        public int upgrade { get; set; }
        public string versionUrl { get; set; }

        public VersionUpdateSearchModel Search { get; set; }

        public List<SelectListItem> UpgradeList { get; set; }
        public List<SelectListItem> DeviceList { get; set; }
    }

    public class VersionUpdateSearchModel
    {
        public VersionUpdateSearchModel()
        {
            UpgradeList = new List<SelectListItem>
            {
                new SelectListItem { Text = "--- 请选择 ---", Value = "0", Selected = true},
                new SelectListItem {Text = "不可升级", Value = "1"},
                new SelectListItem {Text = "可升级", Value = "2"},
                new SelectListItem {Text = "强制升级", Value = "3"}
            };
            DeviceList = new List<SelectListItem>
            {
                new SelectListItem { Text = "--- 请选择 ---", Value = "0", Selected = true},
                new SelectListItem {Text = "iOS", Value = "1"},
                new SelectListItem {Text = "Android", Value = "2"},
            };
        }
        [Display(Name = "设备类型")]
        public int device_os { get; set; }
        [Display(Name = "版本号")]
        public string versionCode { get; set; }
        [Display(Name = "版本名称")]
        public string versionName { get; set; }
        [Display(Name = "链接")]
        public string versionUrl { get; set; }
        [Display(Name = "状态")]
        public int? upgrade { get; set; }

        public List<SelectListItem> UpgradeList { get; set; }
        public List<SelectListItem> DeviceList { get; set; } 
    }
}