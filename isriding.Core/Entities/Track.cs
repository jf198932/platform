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
        [MaxLength(100)]
        public virtual string Start_point { get; set; }
        [MaxLength(100)]
        public virtual string End_point { get; set; }

        public virtual DateTime? Start_time { get; set; }
        public virtual DateTime? End_time { get; set; }
        public virtual double? Payment { get; set; }
        public virtual int? Pay_status { get; set; }
        public virtual string Pay_method { get; set; }
        public virtual string Pay_docno { get; set; }
        public virtual string Remark { get; set; }
        public virtual string Trade_no { get; set; }

        public virtual int User_id { get; set; }
        public virtual int? Bike_id { get; set; }
        public virtual int? Start_site_id { get; set; }
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