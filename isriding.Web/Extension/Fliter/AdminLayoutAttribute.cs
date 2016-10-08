using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using isriding.Web.Models.Common;

namespace isriding.Web.Extension.Fliter
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class AdminLayoutAttribute : ActionFilterAttribute
    {
        //private readonly IRepository<Module> _moduleRepository;
        //private readonly IRepository<UserRole> _userRoleRepository;
        //private readonly IRepository<RoleModulePermission> _roleModulePermissionRepository;

        public AdminLayoutAttribute()
        {
            //_moduleRepository = (((IIocResolver)IocManager.Instance).ResolveAsDisposable<IRepository<Module>>()).Object;
            //_userRoleRepository = (IocResolverExtensions.ResolveAsDisposable<IRepository<UserRole>>(((IIocResolver)IocManager.Instance))).Object;
            //_roleModulePermissionRepository = (IocResolverExtensions.ResolveAsDisposable<IRepository<RoleModulePermission>>(((IIocResolver)IocManager.Instance))).Object;
        }

        //在Action执行之前　乱了点，其实只是判断Cookie用户名密码正不正确而已而已。
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //var cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            //var user = HttpContext.Current.Session["CurrentUser"] as MemberLoginInfo;
            var session = HttpContext.Current.Session["currentUser"] as BackLoginModel;

            if (session == null)
            {
                var controller = filterContext.RouteData.Values["controller"].ToString();
                var action = filterContext.RouteData.Values["action"].ToString();
                var url = "";
                if (!string.IsNullOrWhiteSpace(controller) && !string.IsNullOrWhiteSpace(action))
                {
                    url = filterContext.HttpContext.Request.RawUrl;//获取虚拟路径
                    //url = string.Format("{2}/{0}/{1}", controller, action, path);
                }

                filterContext.Result = new RedirectResult($"{"~/Home/Login"}?returnUrl={HttpUtility.UrlEncode(url)}");
            }
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            //var cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            var session = HttpContext.Current.Session["currentUser"] as BackLoginModel;
            if (session != null)
            {
                //var ticket = FormsAuthentication.Decrypt(cookie.Value);
                //var currentUser = JsonSerializationHelper.DeserializeWithType<BackLoginModel>(ticket.UserData);
                ((ViewResult)filterContext.Result).ViewBag.LoginName = session.LoginName;
                var controller = filterContext.RouteData.Values["controller"].ToString().ToLower();
                //var action = filterContext.RouteData.Values["action"].ToString().ToLower();

                var permission = session.Buttons;

                if (permission != null && permission.Count > 0)
                {
                    var buttonList = permission.Where(p => p.Controller.ToLower() == controller).ToList();
                    ((ViewResult) filterContext.Result).ViewBag.ButtonList = buttonList;
                }
            }
        }
    }
}