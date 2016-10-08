using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace isriding.Entities
{
    [Table("School")]
    public class School : Entity
    {
        public School()
        {
            //Bikes = new List<Bike>();
            //Bikesites = new List<Bikesite>();
            //Users = new List<User>();
            //BackUsers = new List<BackUser>();
            //Modules = new List<Module>();
            //Permissions = new List<Permission>();
            //Roles = new List<Role>();
        }

        public virtual DateTime? Created_at { get; set; }
        public virtual DateTime? Updated_at { get; set; }
        [MaxLength(32)]
        public virtual string TenancyName { get; set; }
        [MaxLength(32)]
        public virtual string Name { get; set; }
        [MaxLength(32)]
        public virtual string Areacode { get; set; }
        [MaxLength(100)]
        public virtual string Gps_point { get; set; }
        public virtual int? Site_count { get; set; }
        public virtual int? Bike_count { get; set; }
        public virtual int? Time_charge { get; set; }
        public virtual DateTime? Refresh_date { get; set; }

        //public virtual ICollection<Bike> Bikes { get; set; } 
        //public virtual ICollection<Bikesite> Bikesites { get; set; }
        //public virtual ICollection<User> Users { get; set; }

        //public virtual ICollection<BackUser> BackUsers { get; set; }
        //public virtual ICollection<Module> Modules { get; set; }
        //public virtual ICollection<Permission> Permissions { get; set; }
        //public virtual ICollection<Role> Roles { get; set; }
    }
}