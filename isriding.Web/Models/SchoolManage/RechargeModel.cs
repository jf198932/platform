using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace isriding.Web.Models.SchoolManage
{
    public class RechargeModel
    {
        public RechargeModel()
        {
            Search = new RechargeSearchModel();
        }
        public int Id { get; set; }

        public DateTime? Created_at { get; set; }
        public DateTime? Updated_at { get; set; }
        /// <summary>
        /// 预充
        /// </summary>
        public double? Recharge_count { get; set; }
        /// <summary>
        /// 失效
        /// </summary>
        public int? Recharge_sum { get; set; }
        /// <summary>
        /// 押金
        /// </summary>
        public double? Deposit { get; set; }


        public int? User_id { get; set; }

        public string User_name { get; set; }
        public string Phone { get; set; }

        public string School_name { get; set; }

        public RechargeSearchModel Search { get; set; }
    }

    public class RechargeSearchModel
    {
        public RechargeSearchModel()
        {
            SchoolList = new List<SelectListItem>();
        }
        [Display(Name = "会员名称")]
        public string User_name { get; set; }
        [Display(Name = "押金")]
        public double? Deposit { get; set; }
        [Display(Name = "预充值")]
        public double? Recharge_count { get; set; }
        [Display(Name = "电话号码")]
        public string Phone { get; set; }

        [Display(Name = "学校")]
        public int? School_id { get; set; }

        public List<SelectListItem> SchoolList { get; set; }
    }
}