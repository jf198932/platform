using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace isriding.Web.Models.School
{
    public class SchoolModel
    {
        public SchoolModel()
        {
            Created_at = DateTime.Now;
            Updated_at = DateTime.Now;
        }
        public int Id { get; set; }
        public DateTime? Created_at { get; set; }
        public DateTime? Updated_at { get; set; }
        [Required(ErrorMessage ="校园名称不能为空")]
        public string Name { get; set; }
        [Required(ErrorMessage = "区号不能为空")]
        public string Areacode { get; set; }
        public string Gps_point { get; set; }
        public int? Site_count { get; set; }
        public int? Bike_count { get; set; }
        public int? Time_charge { get; set; }
        public DateTime? Refresh_date { get; set; }
        //[Required(ErrorMessage = "租户名称不能为空")]
        //[Remote("CheckTenancyNameExists", "Tenancy", ErrorMessage = "租户名称不能重复")]
        //public string TenancyName { get; set; }
    }
}