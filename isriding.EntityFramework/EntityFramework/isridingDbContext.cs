using System;
using System.Data.Entity;
using Abp.EntityFramework;

namespace isriding.EntityFramework
{
    public class isridingDbContext : AbpDbContext
    {
        //TODO: Define an IDbSet for each Entity...
        public virtual IDbSet<Entities.User> Users { get; set; }
        public virtual IDbSet<Entities.Track> Tracks { get; set; }
        public virtual IDbSet<Entities.Sitebeacon> Sitebeacons { get; set; }
        public virtual IDbSet<Entities.School> Schools { get; set; }
        public virtual IDbSet<Entities.Refound> Refounds { get; set; }
        public virtual IDbSet<Entities.Recharge> Recharges { get; set; }
        public virtual IDbSet<Entities.Recharge_detail> Recharge_details { get; set; }
        public virtual IDbSet<Entities.Message> Messages { get; set; }
        public virtual IDbSet<Entities.Log> Logs { get; set; }
        public virtual IDbSet<Entities.Credit> Credits { get; set; }
        public virtual IDbSet<Entities.Bikesite> Bikesites { get; set; }
        public virtual IDbSet<Entities.Bike> Bikes { get; set; }
        public virtual IDbSet<Entities.Sitemonitor> Sitemonitors { get; set; }
        public virtual IDbSet<Entities.VersionUpdate> VersionUpdates { get; set; }
        public virtual IDbSet<Entities.Parameter> Parameters { get; set; }
        //权限
        public virtual IDbSet<Entities.Authen.BackUser> BackUsers { get; set; }
        public virtual IDbSet<Entities.Authen.Role> Roles { get; set; }
        public virtual IDbSet<Entities.Authen.Module> Modules { get; set; }
        public virtual IDbSet<Entities.Authen.Permission> Permissions { get; set; }
        public virtual IDbSet<Entities.Authen.UserRole> UserRoles { get; set; }
        public virtual IDbSet<Entities.Authen.ModulePermission> ModulePermissions { get; set; }
        public virtual IDbSet<Entities.Authen.RoleModulePermission> RoleModulePermissions { get; set; }
        //Example:
        //public virtual IDbSet<User> Users { get; set; }

        /* NOTE: 
         *   Setting "Default" to base class helps us when working migration commands on Package Manager Console.
         *   But it may cause problems when working Migrate.exe of EF. If you will apply migrations on command line, do not
         *   pass connection string name to base classes. ABP works either way.
         */
        public isridingDbContext()
            : base("Default")
        {

        }

        /* NOTE:
         *   This constructor is used by ABP to pass connection string defined in isridingDataModule.PreInitialize.
         *   Notice that, actually you will not directly create an instance of isridingDbContext since ABP automatically handles it.
         */
        public isridingDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }
    }

    public class ReadonlyisridingDbContext : isridingDbContext
    {

        public ReadonlyisridingDbContext() : base("Read")
        {
            
        }
        //public override int SaveChanges()
        //{
        //    // Throw if they try to call this
        //    throw new InvalidOperationException("This context is read-only.");
        //}
    }
}
