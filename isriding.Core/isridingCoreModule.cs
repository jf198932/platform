using System.Reflection;
using Abp.Modules;

namespace isriding
{
    public class isridingCoreModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
