using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace isriding.Entities
{
    [Table("Log")]
    public class Log : Entity
    {
        public virtual DateTime? Created_at { get; set; }
        public virtual DateTime? Updated_at { get; set; }
        /// <summary>
        /// 3.异常离开，4.用户报警，5 报警车辆
        /// </summary>
        public virtual int? Type { get; set; }
        public virtual DateTime? Op_Time { get; set; }


        public virtual int? Bikesite_id { get; set; }
        public virtual int? Bike_id { get; set; }
        [ForeignKey("Bikesite_id")]
        public virtual Bikesite Bikesite { get; set; }
        [ForeignKey("Bike_id")]
        public virtual Bike Bike { get; set; }
    }
}