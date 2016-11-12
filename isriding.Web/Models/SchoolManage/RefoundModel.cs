using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace isriding.Web.Models.SchoolManage
{
    public class RefoundModel
    {
        public RefoundModel()
        {
            Search = new RefoundSearchModel();
            RefoundStatus = new List<SelectListItem> {
                new SelectListItem { Text = "--- 请选择 ---", Value = "0", Selected = true },
                new SelectListItem {Text = "申请中", Value = "1"},
                new SelectListItem {Text = "审核不通过", Value = "2"},
                new SelectListItem {Text = "退款中", Value = "3"},
                new SelectListItem {Text = "退款成功", Value = "4"}
            };
        }
        public int Id { get; set; }
        public DateTime? Created_at { get; set; }
        public DateTime? Updated_at { get; set; }
        /// <summary>
        /// 申请退款金额
        /// </summary>
        public double? Refound_amount { get; set; }
        /// <summary>
        /// 1.申请中, 2.审核不通过，3.退款中，4.退款成功
        /// </summary>
        public int? Refound_status { get; set; }

        public int? User_id { get; set; }

        public string User_name { get; set; }

        public string School_name { get; set; }

        public RefoundSearchModel Search { get; set; }

        public List<SelectListItem> RefoundStatus { get; set; }
    }

    public class RefoundSearchModel
    {
        public RefoundSearchModel()
        {
            SchoolList = new List<SelectListItem>();
            RefoundStatus = new List<SelectListItem> {
                new SelectListItem { Text = "--- 请选择 ---", Value = "0", Selected = true },
                new SelectListItem {Text = "申请中", Value = "1"},
                new SelectListItem {Text = "审核不通过", Value = "2"},
                new SelectListItem {Text = "退款中", Value = "3"},
                new SelectListItem {Text = "退款成功", Value = "4"}
            };
        }
        [Display(Name = "会员名称")]
        public string User_name { get; set; }
        [Display(Name = "退款状态")]
        public int? Refound_status { get; set; }
        [Display(Name = "退款金额")]
        public double? Refound_amount { get; set; }

        [Display(Name = "学校")]
        public int? School_id { get; set; }

        public List<SelectListItem> SchoolList { get; set; }
        public List<SelectListItem> RefoundStatus { get; set; }
    }
}