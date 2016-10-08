using System.Data.Entity;
using System.Reflection;
using Abp.EntityFramework;
using Abp.Modules;
using isriding.EntityFramework;

namespace isriding
{
    [DependsOn(typeof(AbpEntityFrameworkModule), typeof(isridingCoreModule))]
    public class isridingDataModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = "Default";
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            Database.SetInitializer<isridingDbContext>(null);
        }
    }
}
