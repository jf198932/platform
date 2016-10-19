using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace isriding.Web.Models.School
{
    public class BikesiteModel
    {
        public BikesiteModel()
        {
            Created_at = DateTime.Now;
            Updated_at = DateTime.Now;
            SchoolList = new List<SelectListItem>();
            TypeList = new List<SelectListItem>();

            Search = new BikesiteSearchModel();

            Enable = true;
        }
        public int Id { get; set; }
        public DateTime? Created_at { get; set; }
        public DateTime? Updated_at { get; set; }
        [Required(ErrorMessage = "桩点名称不能为空")]
        public string Name { get; set; }
        public int? Type { get; set; }
        [MaxLength(500,ErrorMessage ="不能超过500字")]
        public string Description { get; set; }
        public int? Rent_charge { get; set; }
        public int? Return_charge { get; set; }
        public string Gps_point { get; set; }
        public int? Radius { get; set; }
        public int? Bike_count { get; set; }
        public int? Available_count { get; set; }
        public bool Enable { get; set; }

        public int? School_id { get; set; }
        public string School_name { get; set; }

        public BikesiteSearchModel Search { get; set; }

        public List<SelectListItem> SchoolList { get; set; }
        public List<SelectListItem> TypeList { get; set; }
    }

    public class BikesiteSearchModel
    {
        public BikesiteSearchModel()
        {
            TypeList = new List<SelectListItem>{
                new SelectListItem { Text = "--- 请选择 ---", Value = "0", Selected = true},
                new SelectListItem {Text = "普通", Value = "1"},
                new SelectListItem {Text = "防盗", Value = "2"},
                new SelectListItem {Text = "租车", Value = "3"}
            };
            SchoolList = new List<SelectListItem>();
            EnableList = new List<SelectListItem>{
                new SelectListItem { Text = "--- 请选择 ---", Value = "-1"},
                new SelectListItem {Text = "否", Value = "0"},
                new SelectListItem {Text = "是", Value = "1", Selected = true}
            };
        }

        [Display(Name = "类型")]
        public int? Type { get; set; }

        [Display(Name = "名称")]
        public string Name { get; set; }

        [Display(Name = "学校")]
        public int School_id { get; set; }

        [Display(Name = "启用")]
        public int? Enable { get; set; }

        public List<SelectListItem> TypeList { get; set; }

        public List<SelectListItem> SchoolList { get; set; }

        public List<SelectListItem> EnableList { get; set; }
    }
}