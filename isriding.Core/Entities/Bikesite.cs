using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace isriding.Entities
{
    [Table("Bikesite")]
    public class Bikesite : Entity
    {
        public Bikesite()
        {
            //Bikes = new List<Bike>();
            //Messages = new List<Message>();
            //Sitebeacons = new List<Sitebeacon>();
            //Sitemonitors = new List<Sitemonitor>();
            //TracksStart = new List<Track>();
            //TracksEnd = new List<Track>();
            //Logs = new List<Log>();
        }

        public virtual DateTime? Created_at { get; set; }
        public virtual DateTime? Updated_at { get; set; }
        [MaxLength(32)]
        public virtual string Name { get; set; }
        public virtual int? Type { get; set; }
        [MaxLength(500)]
        public virtual string Description { get; set; }
        public virtual int? Rent_charge { get; set; }
        public virtual int? Return_charge { get; set; }
        [MaxLength(100)]
        public virtual string Gps_point { get; set; }
        public virtual int? Radius { get; set; }
        public virtual int? Bike_count { get; set; }
        public virtual int? Available_count { get; set; }

        public virtual int? School_id { get; set; }
        [ForeignKey("School_id")]
        public virtual School School { get; set; }


        //public virtual ICollection<Bike> Bikes { get; set; } 
        //public virtual ICollection<Message> Messages { get; set; }
        //public virtual ICollection<Sitebeacon> Sitebeacons { get; set; }
        //public virtual ICollection<Sitemonitor> Sitemonitors { get; set; }
        //public virtual ICollection<Track> TracksStart { get; set; }
        //public virtual ICollection<Track> TracksEnd { get; set; }
        //public virtual ICollection<Log> Logs { get; set; }
    }
}