using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace isriding.Entities
{
    [Table("User")]
    public class User : Entity
    {
        public User()
        {
            //Bikes = new List<Bike>();
            //Creditss = new List<Credit>();
            //Messages = new List<Message>();
            //Recharges = new List<Recharge>();
            //Refounds = new List<Refound>();
        }

        public virtual DateTime? Created_at { get; set; }
        public virtual DateTime? Updated_at { get; set; }
        [MaxLength(32)]
        public virtual string Phone { get; set; }
        [MaxLength(32)]
        public virtual string Name { get; set; }
        [MaxLength(32)]
        public virtual string Nickname { get; set; }
        [MaxLength(32)]
        public virtual string Weixacc { get; set; }
        [MaxLength(32)]
        public virtual string Email { get; set; }
        public virtual int? Certification { get; set; }
        public virtual int? Textmsg { get; set; }
        public virtual DateTime? Textmsg_time { get; set; }
        [MaxLength(100)]
        public virtual string Remember_token { get; set; }
        public virtual int? Credits { get; set; }
        public virtual int? Balance { get; set; }

        public virtual string Img { get; set; }

        public virtual string HeadImg { get; set; }
        /// <summary>
        /// 身份类型 0：游客  1：在校学生  2：教职工
        /// </summary>
        public virtual int? User_type { get; set; }
        public virtual int? Device_os { get; set; }
        public virtual string Device_id { get; set; }

        public virtual int? School_id { get; set; }
        [ForeignKey("School_id")]
        public virtual School School { get; set; }


        //public virtual ICollection<Bike> Bikes { get; set; }
        //public virtual ICollection<Credit> Creditss { get; set; }
        //public virtual ICollection<Message> Messages { get; set; }
        //public virtual ICollection<Recharge> Recharges { get; set; }
        //public virtual ICollection<Refound> Refounds { get; set; }
    }
}