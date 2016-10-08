using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace isriding.Entities.Authen
{
    /// <summary>
    /// 用户
    /// </summary>
    [Table("BackUser")]
    public class BackUser : Entity
    {
        public BackUser()
        {
            //this.UserRole = new List<UserRole>();
        }

        public virtual string LoginName { get; set; }
        public virtual string LoginPwd { get; set; }
        public virtual string FullName { get; set; }
        public virtual string Email { get; set; }
        public virtual string Phone { get; set; }
        public virtual bool Enabled { get; set; }
        public virtual int PwdErrorCount { get; set; }
        public virtual int LoginCount { get; set; }
        public virtual DateTime? RegisterTime { get; set; }
        public virtual DateTime? LastLoginTime { get; set; }
        public virtual int? CreateId { get; set; }
        public virtual string CreateBy { get; set; }
        public virtual DateTime? CreateTime { get; set; }
        public virtual int? ModifyId { get; set; }
        public virtual string ModifyBy { get; set; }
        public virtual DateTime? ModifyTime { get; set; }

        public virtual int? School_id { get; set; }
        //[ForeignKey("School_id")]
        //public virtual School School { get; set; }

        //public virtual ICollection<UserRole> UserRole { get; set; }
    }
}