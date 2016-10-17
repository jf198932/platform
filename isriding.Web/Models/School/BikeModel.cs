using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace isriding.Web.Models.School
{
    public class BikeModel
    {
        public BikeModel()
        {
            Created_at = DateTime.Now;
            Updated_at = DateTime.Now;
            
            TypeList = new List<SelectListItem>();
            LockStatusList = new List<SelectListItem>();
            BikeStatusList = new List<SelectListItem>();
            VlockStatusList = new List<SelectListItem>();
            InsiteStatusList = new List<SelectListItem>();
            UserList = new List<SelectListItem>();
            BikesiteList = new List<SelectListItem>();
            SchoolList = new List<SelectListItem>();
            Search = new BikeSearchModel();
        }

        public int Id { get; set; }
        public DateTime? Created_at { get; set; }
        public DateTime? Updated_at { get; set; }
        [Required]
        public string Ble_serial { get; set; }
        [Required]
        public string Ble_name { get; set; }
        [Required]
        public int? Ble_type { get; set; }
        public int? Lock_status { get; set; }
        public int? Bike_status { get; set; }
        public int? Vlock_status { get; set; }
        public int? Insite_status { get; set; }
        public string Position { get; set; }
        public int? Battery { get; set; }
        public string Bike_img { get; set; }
        public int? User_id { get; set; }
        public string User_name { get; set; }
        public int? Bikesite_id { get; set; }
        public string Bikesite_name { get; set; }
        public string Phone { get; set; }
        public string Lock_pwd { get; set; }
        public int? Rent_type { get; set; }

        public int? School_id { get; set; }
        public string School_name { get; set; }

        public DateTime? AlarmTime { get; set; }
        public string AlarmBikesiteName { get; set; }

        public BikeSearchModel Search { get; set; }

        public List<SelectListItem> TypeList { get; set; }
        public List<SelectListItem> LockStatusList { get; set; }
        public List<SelectListItem> BikeStatusList { get; set; }
        public List<SelectListItem> VlockStatusList { get; set; }
        public List<SelectListItem> InsiteStatusList { get; set; }
        public List<SelectListItem> UserList { get; set; }
        public List<SelectListItem> BikesiteList { get; set; }
        public List<SelectListItem> SchoolList { get; set; }
    }

    public class BikeSearchModel
    {
        public BikeSearchModel()
        {
            VlockStatusList = new List<SelectListItem> {
                new SelectListItem { Text = "--- 请选择 ---", Value = "-1", Selected = true },
                new SelectListItem {Text = "初始", Value = "0"},
                new SelectListItem {Text = "锁闭", Value = "1"},
                new SelectListItem {Text = "锁开", Value = "2"},
                new SelectListItem {Text = "异常", Value = "3"},
                new SelectListItem {Text = "异常(已推送)", Value = "4"},
                new SelectListItem {Text = "报警", Value = "5"}
            };
            TypeList = new List<SelectListItem>() {
                new SelectListItem { Text = "--- 请选择 ---", Value = "0", Selected = true},
                new SelectListItem {Text = "追踪器", Value = "1"},
                new SelectListItem {Text = "智能锁", Value = "2"},
                new SelectListItem {Text = "蓝牙锁", Value = "3"},
                new SelectListItem {Text = "机械锁", Value = "4"}
            };
            SchoolList = new List<SelectListItem>();
        }
        [Display(Name = "类型")]
        public int? Ble_type { get; set; }

        [Display(Name = "名称")]
        public string Ble_name { get; set; }

        [Display(Name = "序列号")]
        public string Ble_serial { get; set; }

        [Display(Name = "锁状态")]
        public int? Vlock_status { get; set; }

        [Display(Name = "学校")]
        public int? School_id { get; set; }


        public List<SelectListItem> VlockStatusList { get; set; }

        public List<SelectListItem> TypeList { get; set; }

        public List<SelectListItem> SchoolList { get; set; }
    }
}