using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using isriding.Web.Models.Common;

namespace isriding.Web.Extension.Fliter
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class AdminPermissionAttribute : AuthorizeAttribute
    {
        private readonly PermissionCustomMode _customMode;

        public AdminPermissionAttribute(PermissionCustomMode customMode)
        {
            _customMode = customMode;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //权限拦截是否忽略
            if (_customMode == PermissionCustomMode.Ignore)
            {
                return;
            }
            var area = filterContext.RouteData.DataTokens.ContainsKey("area") ? filterContext.RouteData.DataTokens["area"].ToString() : string.Empty;
            var controller = filterContext.RouteData.Values["controller"].ToString();
            var action = filterContext.RouteData.Values["action"].ToString();
            var url = "";
            if (!string.IsNullOrWhiteSpace(controller) && !string.IsNullOrWhiteSpace(action))
            {
                url = filterContext.HttpContext.Request.RawUrl;//获取虚拟路径
                                                               //url = string.Format("{2}/{0}/{1}", controller, action, path);
            }

            ////验证用户是否登录
            //var cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            var session = HttpContext.Current.Session["currentUser"] as BackLoginModel;
            if (session == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Login", returnUrl = url }));
                return;
            }
            //var ticket = FormsAuthentication.Decrypt(cookie.Value);
            //if (ticket == null)
            //{
            //    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Login", returnUrl = url }));
            //    return;
            //}
            //var currentUser = JsonSerializationHelper.DeserializeWithType<BackLoginModel>(ticket.UserData);

            
            //if (currentUser == null)
            //{
            //    //跳转到登录页面
            //    //filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { area = "Common", controller = "Login", action = "Index" }));
            //    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Login", returnUrl = url }));
            //}
            //else
            //{
                // 权限拦截与验证
                //var area = filterContext.RouteData.DataTokens.ContainsKey("area") ? filterContext.RouteData.DataTokens["area"].ToString() : string.Empty;
                int allowedCount = 0;
                this.IsAllowed(session.Menus, area, controller, action, ref allowedCount);

                if (allowedCount == 0)
                {
                    //filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { area = "Common", controller = "Error", action = "Page400" }));
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Error", action = "Page400" }));
                }
            //}
            
        }

        public void IsAllowed(List<SideBarMenuModel> treeMenuList, string area, string controller, string action, ref int count)
        {
            //var roleIdList = user.UserRoleList.Select(t => t.RoleId);
            if (!string.IsNullOrWhiteSpace(controller) && !string.IsNullOrWhiteSpace(action))
            {
                //var url = string.Format(@"/{0}/{1}", controller, action);
                foreach (var item in treeMenuList)
                {
                    if (item.ChildMenus != null && item.ChildMenus.Count > 0)
                    {
                        IsAllowed(item.ChildMenus, area, controller, action, ref count);
                    }
                    if (item.Action == action && item.Controller == controller)
                        count++;
                }
            }
        }
    }

    public enum PermissionCustomMode
    {
        Enforce,
        Ignore
    }
}