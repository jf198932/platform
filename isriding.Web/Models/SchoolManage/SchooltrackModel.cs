using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace isriding.Web.Models.SchoolManage
{
    public class SchooltrackModel
    {
        public SchooltrackModel()
        {
            Search = new SchooltrackSearchModel();
        }

        public int Id { get; set; }
        public int SchoolId { get; set; }
        public string SchoolName { get; set; }
        public int? RentCount { get; set; }
        public int? ReadyCount { get; set; }
        public int? ErrorCount { get; set; }
        public string ErrorCause { get; set; }

        public SchooltrackSearchModel Search { get; set; }
    }

    public class SchooltrackSearchModel
    {
        public SchooltrackSearchModel()
        {
            SchoolList = new List<SelectListItem>();
        }

        [Display(Name = "学校")]
        public int School_id { get; set; }
        public List<SelectListItem> SchoolList { get; set; }
    }

    public class SchooltrackDetailModel
    {
        public SchooltrackDetailModel()
        {
        }

        public int Id { get; set; }
        public int SchoolId { get; set; }
        public string SchoolName { get; set; }
        public int BikeId { get; set; }
        public string BikeName { get; set; }
        public string Status { get; set; }
        public string ErrorCause { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string StartSite { get; set; }
        public string EndSite { get; set; }
        public string UserName { get; set; }
    }
}