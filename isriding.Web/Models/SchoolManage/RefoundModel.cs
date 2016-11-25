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
            StatusList = new List<SelectListItem> {
                new SelectListItem { Text = "--- 请选择 ---", Value = "-1", Selected = true },
                new SelectListItem {Text = "正常", Value = "0"},
                new SelectListItem {Text = "申请退款", Value = "1"},
                new SelectListItem {Text = "退款成功", Value = "2"}
            };
            Typelist = new List<SelectListItem> {
                new SelectListItem { Text = "--- 请选择 ---", Value = "0"},
                new SelectListItem {Text = "充值记录", Value = "1", Selected = true },
                new SelectListItem {Text = "退款记录", Value = "2"}
            };
            Recharge_typelist = new List<SelectListItem> {
                new SelectListItem { Text = "--- 请选择 ---", Value = "0"},
                new SelectListItem {Text = "押金", Value = "1", Selected = true },
                new SelectListItem {Text = "预充值", Value = "2"}
            };
            Recharge_methodlist = new List<SelectListItem> {
                new SelectListItem { Text = "--- 请选择 ---", Value = "0" },
                new SelectListItem {Text = "支付宝", Value = "1", Selected = true},
                new SelectListItem {Text = "微信", Value = "2"},
                new SelectListItem {Text = "银联", Value = "3"}
            };
        }
        public int Id { get; set; }
        public DateTime? Created_at { get; set; }
        public DateTime? Updated_at { get; set; }
        public int? User_id { get; set; }

        public double? Recharge_amount { get; set; }
        /// <summary>
        /// 1.充值，2.退款
        /// </summary>
        public int? Type { get; set; }
        /// <summary>
        /// 1.押金, 2,预充值
        /// </summary>
        public int? Recharge_type { get; set; }
        /// <summary>
        /// 1.支付宝 2.微信 3.银联
        /// </summary>
        public int? Recharge_method { get; set; }
        /// <summary>
        /// 充值内部编号
        /// </summary>
        public string recharge_docno { get; set; }
        /// <summary>
        /// 支付宝/微信/银联充值单号
        /// </summary>
        public string doc_no { get; set; }
        /// <summary>
        /// 0正常 1：申请退款  2退款成功
        /// </summary>
        public int? Status { get; set; }

        public string User_name { get; set; }

        public string School_name { get; set; }

        public RefoundSearchModel Search { get; set; }
        
        public List<SelectListItem> StatusList { get; set; }
        public List<SelectListItem> Typelist { get; set; }
        public List<SelectListItem> Recharge_typelist { get; set; }
        public List<SelectListItem> Recharge_methodlist { get; set; }
    }

    public class RefoundSearchModel
    {
        public RefoundSearchModel()
        {
            SchoolList = new List<SelectListItem>();
            StatusList = new List<SelectListItem> {
                new SelectListItem { Text = "--- 请选择 ---", Value = "-1", Selected = true },
                new SelectListItem {Text = "正常", Value = "0"},
                new SelectListItem {Text = "申请退款", Value = "1"},
                new SelectListItem {Text = "退款成功", Value = "2"}
            };
            Typelist = new List<SelectListItem> {
                new SelectListItem { Text = "--- 请选择 ---", Value = "0"},
                new SelectListItem {Text = "充值记录", Value = "1", Selected = true },
                new SelectListItem {Text = "退款记录", Value = "2"}
            };
            Recharge_typelist = new List<SelectListItem> {
                new SelectListItem { Text = "--- 请选择 ---", Value = "0"},
                new SelectListItem {Text = "押金", Value = "1", Selected = true },
                new SelectListItem {Text = "预充值", Value = "2"}
            };
            Recharge_methodlist = new List<SelectListItem> {
                new SelectListItem { Text = "--- 请选择 ---", Value = "0", Selected = true },
                new SelectListItem {Text = "支付宝", Value = "1"},
                new SelectListItem {Text = "微信", Value = "2"},
                new SelectListItem {Text = "银联", Value = "3"}
            };
        }
        [Display(Name = "会员名称")]
        public string User_name { get; set; }
        [Display(Name = "退款金额")]
        public double? Recharge_amount { get; set; }

        [Display(Name = "学校")]
        public int? School_id { get; set; }

        /// <summary>
        /// 1.充值，2.退款
        /// </summary>
        [Display(Name = "订单类型")]
        public int? Type { get; set; }
        /// <summary>
        /// 1.押金, 2,预充值
        /// </summary>
        [Display(Name = "金额类型")]
        public int? Recharge_type { get; set; }
        /// <summary>
        /// 1.支付宝 2.微信 3.银联
        /// </summary>
        [Display(Name = "支付方式")]
        public int? Recharge_method { get; set; }
        /// <summary>
        /// 0正常 1：申请退款  2退款成功
        /// </summary>
        [Display(Name = "订单状态")]
        public int? Status { get; set; }
        [Display(Name = "开始时间")]
        public string StartDate { get; set; }
        [Display(Name = "结束时间")]
        public string EndDate { get; set; }

        public List<SelectListItem> SchoolList { get; set; }
        public List<SelectListItem> StatusList { get; set; }
        public List<SelectListItem> Typelist { get; set; }
        public List<SelectListItem> Recharge_typelist { get; set; }
        public List<SelectListItem> Recharge_methodlist { get; set; }
    }
}