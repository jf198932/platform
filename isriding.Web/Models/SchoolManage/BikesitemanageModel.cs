using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace isriding.Web.Models.SchoolManage
{
    public class BikesitemanageModel
    {
        public BikesitemanageModel()
        {
            Search = new BikesitemanageSearchModel();
        }

        public int Id { get; set; }
        public DateTime? Created_at { get; set; }
        public DateTime? Updated_at { get; set; }
        public string Name { get; set; }
        public int? Type { get; set; }

        public string Type_name { get; set; }
        public string Description { get; set; }
        public int? Rent_charge { get; set; }
        public int? Return_charge { get; set; }
        public string Gps_point { get; set; }
        public int? Radius { get; set; }
        public int? Bike_count { get; set; }
        public int? Available_count { get; set; }
        public int? School_id { get; set; }
        public string School { get; set; }

        public BikesitemanageSearchModel Search { get; set; }
    }

    public class BikesitemanageSearchModel
    {
        public BikesitemanageSearchModel()
        {
            SchoolList = new List<SelectListItem>();
            BikesiteList = new List<SelectListItem>();
        }

        [Display(Name = "学校")]
        public int School_id { get; set; }

        [Display(Name = "停车港")]
        public int Name { get; set; }

        public List<SelectListItem> BikesiteList { get; set; }
        public List<SelectListItem> SchoolList { get; set; }
    }
}