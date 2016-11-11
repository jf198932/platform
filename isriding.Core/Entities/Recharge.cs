using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace isriding.Entities
{
    /// <summary>
    /// 充值
    /// </summary>
    [Table("Recharge")]
    public class Recharge : Entity
    {
        public virtual DateTime? Created_at { get; set; }
        public virtual DateTime? Updated_at { get; set; }
        /// <summary>
        /// 预充
        /// </summary>
        public virtual double? Recharge_count { get; set; }
        /// <summary>
        /// 失效
        /// </summary>
        public virtual int? Recharge_sum { get; set; }
        /// <summary>
        /// 押金
        /// </summary>
        public virtual double? Deposit { get; set; }


        public virtual int? User_id { get; set; }
        [ForeignKey("User_id")]
        public virtual User User { get; set; }
    }
}