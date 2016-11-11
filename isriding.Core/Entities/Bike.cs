using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace isriding.Entities
{
    /// <summary>
    /// 自行车，智能锁
    /// </summary>
    [Table("Bike")]
    public class Bike : Entity
    {
        public Bike()
        {
            //Logs = new List<Log>();
            //Tracks = new List<Track>();
        }
        public virtual DateTime? Created_at { get; set; }
        public virtual DateTime? Updated_at { get; set; }
        /// <summary>
        /// 蓝牙设备的名称，固件中设定的名字，由MAC地址加特征字符构成
        /// </summary>
        [MaxLength(32)]
        public virtual string Ble_serial { get; set; }
        /// <summary>
        /// 显示在追踪器或者智能锁上的序列号
        /// </summary>
        [MaxLength(32)]
        public virtual string Ble_name { get; set; }
        /// <summary>
        /// 蓝牙设备类型，1追踪器，2智能锁, 3.蓝牙锁,4.密码锁
        /// </summary>
        public virtual int? Ble_type { get; set; }
        /// <summary>
        /// 如果是智能锁的话，表示锁状态。1锁闭，2锁开，3异常
        /// </summary>
        public virtual int? Lock_status { get; set; }
        /// <summary>
        /// 如果是智能锁的话，表示车辆租用状态。1可租用，0出租中。
        /// </summary>
        public virtual int? Bike_status { get; set; }
        /// <summary>
        /// 如果是追踪器的话，表示虚拟锁定状态。0初始，1锁定,2解锁,3 异常，4推送,5 报警
        /// </summary>
        public virtual int? Vlock_status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual int? Insite_status { get; set; }
        /// <summary>
        /// 当前所在的位置的GPS
        /// </summary>
        [MaxLength(100)]
        public virtual string Position { get; set; }
        /// <summary>
        /// 电池电量
        /// </summary>
        public virtual int? Battery { get; set; }
        /// <summary>
        /// 车辆照片
        /// </summary>
        public virtual string Bike_img { get; set; }
        /// <summary>
        /// 机械锁密码
        /// </summary>
        public virtual string Lock_pwd { get; set; }
        /// <summary>
        /// 车辆租用类型, 1.可租用 2. 不可租用
        /// </summary>
        public virtual int? rent_type { get; set; }
        /// <summary>
        /// 所属用户
        /// </summary>
        public virtual int? User_id { get; set; }
        /// <summary>
        /// 所属学校
        /// </summary>
        public virtual int? School_id { get; set; }
        /// <summary>
        /// 当前所在的停车港
        /// </summary>
        public virtual int? Bikesite_id { get; set; }
        [ForeignKey("User_id")]
        public virtual User User { get; set; }
        [ForeignKey("School_id")]
        public virtual School School { get; set; }
        [ForeignKey("Bikesite_id")]
        public virtual Bikesite Bikesite { get; set; }


        //public virtual ICollection<Log> Logs { get; set; }
        //public virtual ICollection<Track> Tracks { get; set; }
    }
}