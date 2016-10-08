using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace isriding.Entities
{
    [Table("Bike")]
    public class Bike : Entity
    {
        public Bike()
        {
            //Logs = new List<Log>();
            //Tracks = new List<Track>();
        }
        public virtual DateTime? Created_at { get; set; }
        public virtual DateTime? Updated_at { get; set; }
        [MaxLength(32)]
        public virtual string Ble_serial { get; set; }
        [MaxLength(32)]
        public virtual string Ble_name { get; set; }
        public virtual int? Ble_type { get; set; }
        public virtual int? Lock_status { get; set; }
        public virtual int? Bike_status { get; set; }
        public virtual int? Vlock_status { get; set; }
        public virtual int? Insite_status { get; set; }
        [MaxLength(32)]
        public virtual string Position { get; set; }
        public virtual int? Battery { get; set; }

        public virtual string Bike_img { get; set; }
        public virtual string Lock_pwd { get; set; }
        public virtual int? rent_type { get; set; }

        public virtual int? User_id { get; set; }
        public virtual int? School_id { get; set; }
        public virtual int? Bikesite_id { get; set; }
        [ForeignKey("User_id")]
        public virtual User User { get; set; }
        [ForeignKey("School_id")]
        public virtual School School { get; set; }
        [ForeignKey("Bikesite_id")]
        public virtual Bikesite Bikesite { get; set; }


        //public virtual ICollection<Log> Logs { get; set; }
        //public virtual ICollection<Track> Tracks { get; set; }
    }
}