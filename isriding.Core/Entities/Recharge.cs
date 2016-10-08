using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace isriding.Entities
{
    [Table("Recharge")]
    public class Recharge : Entity
    {
        public virtual DateTime? Created_at { get; set; }
        public virtual DateTime? Updated_at { get; set; }
        public virtual int? Recharge_count { get; set; }
        public virtual int? Recharge_sum { get; set; }


        public virtual int? User_id { get; set; }
        [ForeignKey("User_id")]
        public virtual User User { get; set; }
    }
}