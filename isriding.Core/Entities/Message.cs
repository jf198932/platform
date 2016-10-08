using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace isriding.Entities
{
    [Table("Message")]
    public class Message : Entity
    {
        public virtual DateTime? Created_at { get; set; }
        public virtual DateTime? Updated_at { get; set; }
        [MaxLength(1000)]
        public virtual string Message_info { get; set; }
        public virtual DateTime? Send_Time { get; set; }


        public virtual int? Bikesite_id { get; set; }
        public virtual int? User_id { get; set; }
        [ForeignKey("Bikesite_id")]
        public virtual Bikesite Bikesite { get; set; }
        [ForeignKey("User_id")]
        public virtual User User { get; set; }
    }
}