using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace isriding.Web.Models.School
{
    public class SitemonitorModel
    {
        public SitemonitorModel()
        {
            Search = new SitemonitorSearchModel();

            BikesiteList = new List<SelectListItem>();
            StatusList = new List<SelectListItem>
            {
                new SelectListItem { Text = "--- 请选择 ---", Value = "-1", Selected = true },
                new SelectListItem {Text = "损坏", Value = "0"},
                new SelectListItem {Text = "正常", Value = "1"}
            };
            EnabledList = new List<SelectListItem>
            {
                new SelectListItem { Text = "--- 请选择 ---", Value = "-1", Selected = true },
                new SelectListItem {Text = "禁用", Value = "0"},
                new SelectListItem {Text = "启用", Value = "1"}
            };
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? Bikesite_id { get; set; }
        public string Bikesite_name { get; set; }
        public int? Status { get; set; }
        public bool Enabled { get; set; }

        public string School_name { get; set; }

        public List<SelectListItem> BikesiteList { get; set; }
        public List<SelectListItem> StatusList { get; set; }
        public List<SelectListItem> EnabledList { get; set; }

        public SitemonitorSearchModel Search { get; set; }
    }

    public class SitemonitorSearchModel
    {
        public SitemonitorSearchModel()
        {
            BikesiteList = new List<SelectListItem>();
            SchoolList = new List<SelectListItem>();
            StatusList = new List<SelectListItem>
            {
                new SelectListItem { Text = "--- 请选择 ---", Value = "-1", Selected = true },
                new SelectListItem {Text = "损坏", Value = "0"},
                new SelectListItem {Text = "正常", Value = "1"}
            };
            EnabledList = new List<SelectListItem>
            {
                new SelectListItem { Text = "--- 请选择 ---", Value = "-1"},
                new SelectListItem {Text = "禁用", Value = "0"},
                new SelectListItem {Text = "启用", Value = "1", Selected = true }
            };
        }
        [Display(Name = "学校")]
        public int? School_id { get; set; }
        [Display(Name = "名称")]
        public string Name { get; set; }
        [Display(Name = "桩点")]
        public int? Bikesite_id { get; set; }
        [Display(Name = "状态")]
        public int? Status { get; set; }
        [Display(Name = "启用")]
        public int? Enabled { get; set; }

        public List<SelectListItem> BikesiteList { get; set; }
        public List<SelectListItem> StatusList { get; set; }
        public List<SelectListItem> EnabledList { get; set; }
        public List<SelectListItem> SchoolList { get; set; } 
    }
}