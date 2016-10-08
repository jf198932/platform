using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace isriding.Entities.Authen
{
    /// <summary>
    /// 模块
    /// </summary>
    [Table("Module")]
    public class Module : Entity
    {
        public Module()
        {
            //this.ChildModule = new List<Module>();
            //this.ModulePermission = new List<ModulePermission>();
            //this.RoleModulePermission = new List<RoleModulePermission>();
        }

        public virtual int? ParentId { get; set; }
        public virtual string Name { get; set; }
        public virtual string LinkUrl { get; set; }
        public virtual string Area { get; set; }
        public virtual string Controller { get; set; }
        public virtual string Action { get; set; }
        public virtual string Icon { get; set; }
        public virtual string Code { get; set; }
        public virtual int OrderSort { get; set; }
        public virtual string Description { get; set; }
        public virtual bool IsMenu { get; set; }
        public virtual bool Enabled { get; set; }
        public virtual int? CreateId { get; set; }
        public virtual string CreateBy { get; set; }
        public virtual DateTime? CreateTime { get; set; }
        public virtual int? ModifyId { get; set; }
        public virtual string ModifyBy { get; set; }
        public virtual DateTime? ModifyTime { get; set; }

        public virtual int? School_id { get; set; }
        //[ForeignKey("School_id")]
        //public virtual School School { get; set; }

        [ForeignKey("ParentId")]
        public virtual Module ParentModule { get; set; }
        //public virtual ICollection<Module> ChildModule { get; set; }
        //public virtual ICollection<ModulePermission> ModulePermission { get; set; }
        //public virtual ICollection<RoleModulePermission> RoleModulePermission { get; set; }
    }
}