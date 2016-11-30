using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace isriding.Entities
{
    /// <summary>
    /// 退款
    /// </summary>
    [Table("Refound")]
    public class Refound : Entity
    {
        public virtual DateTime? Created_at { get; set; }
        public virtual DateTime? Updated_at { get; set; }
        /// <summary>
        /// 申请退款金额
        /// </summary>
        public virtual double? Refound_amount { get; set; }
        /// <summary>
        /// 1.申请中, 2.审核不通过，3.退款中，4.退款成功
        /// </summary>
        public virtual int? Refound_status { get; set; }

        public virtual int? User_id { get; set; }
        [ForeignKey("User_id")]
        public virtual User User { get; set; }
    }
}