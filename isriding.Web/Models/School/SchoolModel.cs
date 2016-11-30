using System;
using System.ComponentModel.DataAnnotations;

namespace isriding.Web.Models.School
{
    public class SchoolModel
    {
        public SchoolModel()
        {
            Created_at = DateTime.Now;
            Updated_at = DateTime.Now;
            Bike_count = 0;
            Site_count = 0;
            Time_charge = 1;
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
        /// <summary>
        /// 公共自行车单价，单位为“RMB分/minute"，每分钟几分钱
        /// </summary>
        public int? Time_charge { get; set; }
        public DateTime? Refresh_date { get; set; }
        /// <summary>
        /// 免费时间（分钟）
        /// </summary>
        public int? Free_time { get; set; }
        /// <summary>
        /// 押金
        /// </summary>
        public double? Deposit { get; set; }
        //[Required(ErrorMessage = "租户名称不能为空")]
        //[Remote("CheckTenancyNameExists", "Tenancy", ErrorMessage = "租户名称不能重复")]
        //public string TenancyName { get; set; }
    }
}