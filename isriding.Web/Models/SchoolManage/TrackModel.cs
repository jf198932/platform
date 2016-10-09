using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace isriding.Web.Models.SchoolManage
{
    public class TrackModel
    {
        public TrackModel()
        {
            Search = new TrackSearchModel();
        }

        public int Id { get; set; }
        public DateTime? Created_at { get; set; }
        public DateTime? Updated_at { get; set; }
        public string Start_point { get; set; }
        public string End_point { get; set; }

        public DateTime? Start_time { get; set; }
        public DateTime? End_time { get; set; }
        public double? Payment { get; set; }
        public int? Pay_status { get; set; }
        public string Pay_method { get; set; }
        public string Pay_docno { get; set; }
        public string Remark { get; set; }
        public string Trade_no { get; set; }

        public int User_id { get; set; }
        public string User_Name { get; set; }
        public int? Bike_id { get; set; }
        public string Ble_name { get; set; }
        public int? Start_site_id { get; set; }
        public string Start_site_name { get; set; }
        public int? End_site_id { get; set; }
        public string End_site_name { get; set; }

        public string School_name { get; set; }

        public TrackSearchModel Search { get; set; }
    }

    public class TrackSearchModel
    {
        public TrackSearchModel()
        {
            PayStatusList = new List<SelectListItem> {
                new SelectListItem { Text = "--- 请选择 ---", Value = "0", Selected = true },
                new SelectListItem {Text = "使用中", Value = "1"},
                new SelectListItem {Text = "还车未支付", Value = "2"},
                new SelectListItem {Text = "已支付", Value = "3"}
            };
            SchoolList = new List<SelectListItem>();
        }
        
        [Display(Name = "用户名称")]
        public string User_Name { get; set; }

        [Display(Name = "车辆编号")]
        public string Ble_name { get; set; }

        [Display(Name = "开始桩点")]
        public string Start_site_name { get; set; }

        [Display(Name = "结束桩点")]
        public string End_site_name { get; set; }

        [Display(Name = "支付状态")]
        public int Pay_status { get; set; }

        [Display(Name = "学校")]
        public int School_id { get; set; }

        public List<SelectListItem> PayStatusList { get; set; }
        public List<SelectListItem> SchoolList { get; set; }
    }
}