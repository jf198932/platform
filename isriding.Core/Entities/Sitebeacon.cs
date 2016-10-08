using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace isriding.Entities
{
    [Table("Sitebeacon")]
    public class Sitebeacon : Entity
    {
        public virtual DateTime? Created_at { get; set; }
        public virtual DateTime? Updated_at { get; set; }
        [MaxLength(32)]
        public virtual string Name { get; set; }
        [MaxLength(100)]
        public virtual string Gps_point { get; set; }
        public virtual int? Tx_power { get; set; }
        public virtual int? Battery { get; set; }


        public virtual int? Bikesite_id { get; set; }
        [ForeignKey("Bikesite_id")]
        public virtual Bikesite Bikesite { get; set; }
    }
}