using Abp.Application.Services;

namespace isriding
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public abstract class isridingAppServiceBase : ApplicationService
    {
        protected isridingAppServiceBase()
        {
            LocalizationSourceName = isridingConsts.LocalizationSourceName;
        }
    }
}