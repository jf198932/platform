using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace isriding.Web.Models.SchoolManage
{
    public class TroubleModel
    {
        public TroubleModel()
        {
            Search = new TroubleSearchModel();

            VerifyStatusList = new List<SelectListItem> {
                new SelectListItem { Text = "--- 请选择 ---", Value = "0", Selected = true },
                new SelectListItem {Text = "待核实", Value = "1"},
                new SelectListItem {Text = "已经核实", Value = "2"},
                new SelectListItem {Text = "非属实(虚报)", Value = "3"}
            };

            DealStatusList = new List<SelectListItem> {
                new SelectListItem { Text = "--- 请选择 ---", Value = "0", Selected = true },
                new SelectListItem {Text = "用户提交", Value = "1"},
                new SelectListItem {Text = "客服处理中", Value = "2"},
                new SelectListItem {Text = "客服已经处理", Value = "3"}
            };
        }

        public int Id { get; set; }
        public DateTime? create_time { get; set; }
        public DateTime? update_time { get; set; }
        /// <summary>
        /// 反馈用户
        /// </summary>
        public int? create_by { get; set; }
        /// <summary>
        /// 最后修改人
        /// </summary>
        public int? update_by { get; set; }
        /// <summary>
        /// 车辆编号
        /// </summary>
        public string bike_number { get; set; }
        /// <summary>
        /// 车辆损坏(坐垫损坏、链条损坏、踏脚损坏、龙头损坏、轮胎损坏、其他损坏)
        /// </summary>
        public string trouble1 { get; set; }
        /// <summary>
        /// 用车故障(车锁故障、还车故障)
        /// </summary>
        public string trouble2 { get; set; }
        /// <summary>
        /// 违规用车(举报违停、上私锁、不锁车、私卸车牌、而已损坏、疑似偷车)
        /// </summary>
        public string trouble3 { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string comments { get; set; }
        /// <summary>
        /// 照片地址
        /// </summary>
        public string img_url { get; set; }
        /// <summary>
        /// 核实情况，1.待核实，2.已经核实，3.非属实(虚报)
        /// </summary>
        public int? verify_status { get; set; }
        /// <summary>
        /// 客服处理情况, 1.用户提交, 2.客服处理中，3.客服已经处理
        /// </summary>
        public int? deal_status { get; set; }

        public string username { get; set; }
        public string schoolname { get; set; }

        public string phone { get; set; }


        public TroubleSearchModel Search { get; set; }

        public List<SelectListItem> VerifyStatusList { get; set; }
        public List<SelectListItem> DealStatusList { get; set; }
    }

    public class TroubleSearchModel
    {
        public TroubleSearchModel()
        {
            VerifyStatusList = new List<SelectListItem> {
                new SelectListItem { Text = "--- 请选择 ---", Value = "0", Selected = true },
                new SelectListItem {Text = "待核实", Value = "1"},
                new SelectListItem {Text = "已经核实", Value = "2"},
                new SelectListItem {Text = "非属实(虚报)", Value = "3"}
            };

            DealStatusList = new List<SelectListItem> {
                new SelectListItem { Text = "--- 请选择 ---", Value = "0", Selected = true },
                new SelectListItem {Text = "用户提交", Value = "1"},
                new SelectListItem {Text = "客服处理中", Value = "2"},
                new SelectListItem {Text = "客服已经处理", Value = "3"}
            };
            SchoolList = new List<SelectListItem>();
        }

        [Display(Name = "核实情况")]
        public int verify_status { get; set; }

        [Display(Name = "处理情况")]
        public string deal_status { get; set; }

        [Display(Name = "学校")]
        public int School_id { get; set; }

        [Display(Name = "开始时间")]
        public string StartDate { get; set; }
        [Display(Name = "结束时间")]
        public string EndDate { get; set; }

        public List<SelectListItem> VerifyStatusList { get; set; }
        public List<SelectListItem> DealStatusList { get; set; }
        public List<SelectListItem> SchoolList { get; set; }
    }
}