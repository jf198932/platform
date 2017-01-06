using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace isriding.Web.Models.SchoolManage
{
    public class BikemanageModel
    {
        public BikemanageModel()
        {
            Search = new BikemanageSearchModel();
        }

        public int Id { get; set; }

        public string BleName { get; set; }

        public DateTime? MaxRentTime { get; set; }

        public string BikeVer { get; set; }

        public string BleType { get; set; }

        public DateTime? StartTime { get; set; }
        public string RentTimeCnt { get; set; }
        public string RentCnt { get; set; }
        public string Payment { get; set; }
        public string BikeStatus { get; set; }
        public string SchoolName { get; set; }

        public int? SchoolId { get; set; }
        public int? BikeId { get; set; }

        public BikemanageSearchModel Search { get; set; }
    }

    public class BikemanageSearchModel
    {
        public BikemanageSearchModel()
        {
            SchoolList = new List<SelectListItem>();
            BikeStatusList = new List<SelectListItem> {
                new SelectListItem { Text = "--- 请选择 ---", Value = "" },
                new SelectListItem {Text = "待租", Value = "待租", Selected = true},
                new SelectListItem {Text = "出租中", Value = "出租中"}
            };
        }

        //[Display(Name = "城市")]
        //public int School_id { get; set; }

        [Display(Name = "学校")]
        public int SchoolId { get; set; }
        [Display(Name = "车牌号")]
        public string Ble_name { get; set; }
        [Display(Name = "车辆状态")]
        public int Bstatus { get; set; }
        //[Display(Name = "停车港")]
        //public int Name { get; set; }

        public List<SelectListItem> BikeStatusList { get; set; }
        public List<SelectListItem> SchoolList { get; set; }
    }
    public class BikemanageDetailModel
    {
        public BikemanageDetailModel()
        {
        }

        public int Id { get; set; }
        public string PayDocno { get; set; }

        public int BikeId { get; set; }
        public string BikeName { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string StartSite { get; set; }
        public string EndSite { get; set; }
        public string RentTimeCnt { get; set; }
        public string payment { get; set; }
    }
}