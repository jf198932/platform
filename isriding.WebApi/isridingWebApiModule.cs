using System.Reflection;
using Abp.Application.Services;
using Abp.Configuration.Startup;
using Abp.Modules;
using Abp.WebApi;

namespace isriding
{
    [DependsOn(typeof(AbpWebApiModule), typeof(isridingApplicationModule))]
    public class isridingWebApiModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            Configuration.Modules.AbpWebApi().DynamicApiControllerBuilder
                .ForAll<IApplicationService>(typeof(isridingApplicationModule).Assembly, "app")
                .Build();
        }
    }
}
