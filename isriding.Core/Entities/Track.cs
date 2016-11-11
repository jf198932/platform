using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace isriding.Entities
{
    [Table("Track")]
    public class Track : Entity
    {
        public virtual DateTime? Created_at { get; set; }
        public virtual DateTime? Updated_at { get; set; }
        /// <summary>
        /// 租车人起始GPS
        /// </summary>
        [MaxLength(100)]
        public virtual string Start_point { get; set; }
        /// <summary>
        /// 租车人结束GPS
        /// </summary>
        [MaxLength(100)]
        public virtual string End_point { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public virtual DateTime? Start_time { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public virtual DateTime? End_time { get; set; }
        /// <summary>
        /// 实际支付金额
        /// </summary>
        public virtual double? Payment { get; set; }
        /// <summary>
        /// 应该支付金额
        /// </summary>
        public virtual double? Should_pay { get; set; }
        /// <summary>
        /// 结算状态，1,待还车, 2. 还车待支付, 3. 支付完成
        /// </summary>
        public virtual int? Pay_status { get; set; }
        /// <summary>
        /// 1.支付宝 2.微信 3.银联
        /// </summary>
        public virtual string Pay_method { get; set; }
        /// <summary>
        /// 订单内部编号
        /// </summary>
        public virtual string Pay_docno { get; set; }
        /// <summary>
        /// 用户建议
        /// </summary>
        public virtual string Remark { get; set; }
        /// <summary>
        /// 支付宝/微信/银联产生的单号
        /// </summary>
        public virtual string Trade_no { get; set; }
        /// <summary>
        /// 所属用户
        /// </summary>
        public virtual int User_id { get; set; }
        /// <summary>
        /// 所属用户当前租用的自行车
        /// </summary>
        public virtual int? Bike_id { get; set; }
        /// <summary>
        /// 起始桩点GPS
        /// </summary>
        public virtual int? Start_site_id { get; set; }
        /// <summary>
        /// 结束桩点GPS
        /// </summary>
        public virtual int? End_site_id { get; set; }
        [ForeignKey("User_id")]
        public virtual User User { get; set; }
        [ForeignKey("Bike_id")]
        public virtual Bike Bike { get; set; }
        [ForeignKey("Start_site_id")]
        public virtual Bikesite Bikesitestart { get; set; }
        [ForeignKey("End_site_id")]
        public virtual Bikesite Bikesiteend { get; set; }
    }
}