using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace isriding.Entities
{
    /// <summary>
    /// 小桩
    /// </summary>
    [Table("Sitemonitor")]
    public class Sitemonitor : Entity
    {
        public virtual DateTime? Created_at { get; set; }
        public virtual DateTime? Updated_at { get; set; }
        /// <summary>
        /// 监控桩位编号
        /// </summary>
        [MaxLength(32)]
        public virtual string Name { get; set; }
        /// <summary>
        /// 停车港
        /// </summary>
        public virtual int? Bikesite_id { get; set; }
        /// <summary>
        /// 0 正常 ,1 非工作
        /// </summary>
        public virtual int? Status { get; set; }
        /// <summary>
        /// 启用
        /// </summary>
        public virtual bool Enabled { get; set; }

        [ForeignKey("Bikesite_id")]
        public virtual Bikesite Bikesite { get; set; }
    }
}