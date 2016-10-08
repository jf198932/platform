using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace isriding.Entities
{
    [Table("Credit")]
    public class Credit : Entity
    {
        public virtual DateTime? Created_at { get; set; }
        public virtual DateTime? Updated_at { get; set; }
        public virtual int? Credits_count { get; set; }
        public virtual int? Credits_sum { get; set; }


        public virtual int? User_id { get; set; }
        [ForeignKey("User_id")]
        public virtual User User { get; set; }
    }
}