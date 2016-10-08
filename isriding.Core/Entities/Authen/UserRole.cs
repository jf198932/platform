using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace isriding.Entities.Authen
{
    /// <summary>
    /// 用户-角色
    /// </summary>
    [Table("UserRole")]
    public class UserRole : Entity
    {
        public virtual int UserId { get; set; }
        public virtual int RoleId { get; set; }

        public virtual int? CreateId { get; set; }
        public virtual string CreateBy { get; set; }
        public virtual DateTime? CreateTime { get; set; }
        public virtual int? ModifyId { get; set; }
        public virtual string ModifyBy { get; set; }
        public virtual DateTime? ModifyTime { get; set; }

        [ForeignKey("UserId")]
        public virtual BackUser BackUser { get; set; }
        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }
    }
}