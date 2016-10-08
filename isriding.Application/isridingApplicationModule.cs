using System.Reflection;
using Abp.Modules;

namespace isriding
{
    [DependsOn(typeof(isridingCoreModule))]
    public class isridingApplicationModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
