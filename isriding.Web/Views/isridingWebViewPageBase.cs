using Abp.Web.Mvc.Views;

namespace isriding.Web.Views
{
    public abstract class isridingWebViewPageBase : isridingWebViewPageBase<dynamic>
    {

    }

    public abstract class isridingWebViewPageBase<TModel> : AbpWebViewPage<TModel>
    {
        protected isridingWebViewPageBase()
        {
            LocalizationSourceName = isridingConsts.LocalizationSourceName;
        }
    }
}