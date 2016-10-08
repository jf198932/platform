using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace isriding.Entities
{
    [Table("VersionUpdate")]
    public class VersionUpdate : Entity
    {
        public virtual int device_os { get; set; }
        public virtual int versionCode { get; set; }
        [MaxLength(100)]
        public virtual string versionName { get; set; }
        public virtual int upgrade { get; set; }
        [MaxLength(100)]
        public virtual string versionUrl { get; set; }
    }
}