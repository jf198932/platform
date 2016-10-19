using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace isriding.Entities
{
    [Table("Sitemonitor")]
    public class Sitemonitor : Entity
    {
        public virtual DateTime? Created_at { get; set; }
        public virtual DateTime? Updated_at { get; set; }
        [MaxLength(32)]
        public virtual string Name { get; set; }
        public virtual int? Bikesite_id { get; set; }
        public virtual int? Status { get; set; }
        public virtual bool Enabled { get; set; }

        [ForeignKey("Bikesite_id")]
        public virtual Bikesite Bikesite { get; set; }
    }
}