using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace isriding.Entities
{
    /// <summary>
    /// 充值退款记录
    /// </summary>
    [Table("Recharge_detail")]
    public class Recharge_detail : Entity
    {
        public virtual DateTime? Created_at { get; set; }
        public virtual DateTime? Updated_at { get; set; }

        public virtual int? User_id { get; set; }
        /// <summary>
        /// 充值，退款金额
        /// </summary>
        public virtual double? Recharge_amount { get; set; }
        /// <summary>
        /// 1.充值，2.退款
        /// </summary>
        public virtual int? Type { get; set; }
        /// <summary>
        /// 1.押金, 2,预充值
        /// </summary>
        public virtual int? Recharge_type { get; set; }
        /// <summary>
        /// 1.支付宝 2.微信 3.银联
        /// </summary>
        public virtual int? Recharge_method { get; set; }
        /// <summary>
        /// 充值内部编号
        /// </summary>
        public virtual string recharge_docno { get; set; }
        /// <summary>
        /// 支付宝/微信/银联充值单号
        /// </summary>
        public virtual string doc_no { get; set; }
        /// <summary>
        /// 0正常 1 退款申请  2 退款成功
        /// </summary>
        public virtual int? Status { get; set; }

        public virtual string source_recharge_docno { get; set; }

        public virtual string source_doc_no { get; set; }

        [ForeignKey("User_id")]
        public virtual User User { get; set; }
    }
}