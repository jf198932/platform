using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace isriding.Entities
{
    /// <summary>
    /// 自行车桩点
    /// </summary>
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
        /// <summary>
        /// 租车点名称，显示在地图上
        /// </summary>
        [MaxLength(32)]
        public virtual string Name { get; set; }
        /// <summary>
        /// 租车点类型，对应编号待定 1. 普通, 2.防盗,3.租车
        /// </summary>
        public virtual int? Type { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [MaxLength(500)]
        public virtual string Description { get; set; }
        /// <summary>
        /// 保留字段，设计为除租用费用外，租车离开本港时需要付费金额
        /// </summary>
        public virtual int? Rent_charge { get; set; }
        /// <summary>
        /// 保留字段，设计为除租用费用外，还车进入本港时需要付费的金额
        /// </summary>
        public virtual int? Return_charge { get; set; }
        /// <summary>
        /// 租车点的gps位置
        /// </summary>
        [MaxLength(100)]
        public virtual string Gps_point { get; set; }
        /// <summary>
        /// 以gps位置为中心的覆盖半径，单位为米
        /// </summary>
        public virtual int? Radius { get; set; }
        /// <summary>
        /// 公共自行车数量
        /// </summary>
        public virtual int? Bike_count { get; set; }
        /// <summary>
        /// 可租用的公共自行车数量
        /// </summary>
        public virtual int? Available_count { get; set; }
        /// <summary>
        /// 启用
        /// </summary>
        public virtual bool Enable { get; set; }
        /// <summary>
        /// 标识所属的学校
        /// </summary>
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