using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace isriding.Entities.Authen
{
    /// <summary>
    /// 角色-模块-权限
    /// </summary>
    [Table("RoleModulePermission")]
    public class RoleModulePermission : Entity
    {
        public virtual int RoleId { get; set; }
        public virtual int ModuleId { get; set; }
        public virtual int? PermissionId { get; set; }
        public virtual int? CreateId { get; set; }
        public virtual string CreateBy { get; set; }
        public virtual DateTime? CreateTime { get; set; }
        public virtual int? ModifyId { get; set; }
        public virtual string ModifyBy { get; set; }
        public virtual DateTime? ModifyTime { get; set; }
        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }
        [ForeignKey("ModuleId")]
        public virtual Module Module { get; set; }
        [ForeignKey("PermissionId")]
        public virtual Permission Permission { get; set; }
    }
}