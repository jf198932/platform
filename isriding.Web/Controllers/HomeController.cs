using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Domain.Uow;
using Abp.UI;
using AutoMapper;
using isriding.Entities.Authen;
using isriding.Helper;
using isriding.Web.Extension.Fliter;
using isriding.Web.Models;
using isriding.Web.Models.Authen;
using isriding.Web.Models.Common;
using Abp.Extensions;
using Abp.Runtime.Caching;
using isriding.Authen.BackUser;
using isriding.Authen.Module;
using isriding.Authen.RoleModulePermission;
using isriding.Authen.UserRole;
using isriding.School;

namespace isriding.Web.Controllers
{
    public class HomeController : isridingControllerBase
    {
        private readonly IBackUserReadRepository _backUserReadRepository;
        private readonly IModuleReadRepository _moduleUserReadRepository;
        private readonly IUserRoleReadRepository _userRoleReadRepository;
        private readonly ISchoolReadRepository _schoolReadRepository;
        private readonly IRoleModulePermissionReadRepository _roleModulePermissionReadRepository;
        private readonly ICacheManager _cacheManager;

        public HomeController(IBackUserReadRepository backUserReadRepository,
            IModuleReadRepository moduleUserReadRepository,
            IUserRoleReadRepository userRoleReadRepository,
            ISchoolReadRepository schoolReadRepository,
            IRoleModulePermissionReadRepository roleModulePermissionReadRepository,
            ICacheManager cacheManager)
        {
            _backUserReadRepository = backUserReadRepository;
            _moduleUserReadRepository = moduleUserReadRepository;
            _userRoleReadRepository = userRoleReadRepository;
            _schoolReadRepository = schoolReadRepository;
            _roleModulePermissionReadRepository = roleModulePermissionReadRepository;
            _cacheManager = cacheManager;
        }

        public ActionResult Index()
        {
            return RedirectToAction("List");
            //return View("~/App/Main/views/layout/layout.cshtml"); //Layout of the angular application.
        }
        [AdminLayout]
        public ActionResult List()
        {
            return View();
        }

        public ActionResult Login(string returnUrl)
        {
            var model = new LoginModel();
            ViewBag.returnUrl = returnUrl;
            return View(model);
        }

        [HttpPost, UnitOfWork, ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var now = DateTime.Now.ToLocalTime();
                var despwd = DESProvider.EncryptString(model.Password);
                
                var user = await _backUserReadRepository.FirstOrDefaultAsync(u =>
                                u.LoginName.ToLower() == model.UserNameOrEmail.ToLower()
                                && u.LoginPwd == despwd);
                if (user == null)
                    throw new UserFriendlyException("用户名或者密码错误");

                Mapper.Initialize(t=> { t.CreateMap<BackUser, BackLoginModel>(); });
                var currentUser = Mapper.Map<BackLoginModel>(user);

                var userRole = await _userRoleReadRepository.GetAllListAsync(ur => ur.UserId == currentUser.Id);

                var roleIds = userRole.Select(t => t.RoleId);

                
                //currentUser.RoleNames = roleNames.ToList();

                var moduleIdList = _roleModulePermissionReadRepository.GetAll().Where(t => roleIds.Contains(t.RoleId))
                        .Select(t => t.ModuleId)
                        .Distinct()
                        .ToList();

                var buttons = _roleModulePermissionReadRepository.GetAll()
                    .Where(t => roleIds.Contains(t.RoleId) && (t.PermissionId != null || t.PermissionId > 0))
                    .Select(
                        t =>
                            new PermissionButtonModel
                            {
                                Action = t.Module == null ? "" : t.Module.Action,
                                Controller = t.Module == null ? "" : t.Module.Controller,
                                Code = t.Permission == null ? "" : t.Permission.Code,
                                Name = t.Permission == null ? "" : t.Permission.Name,
                                Icon = t.Permission == null ? "" : t.Permission.Icon
                            });
                currentUser.Buttons.AddRange(buttons);

                //Logger.Info(System.Web.Helpers.Json.Encode(buttons));

                var moduleList = _moduleUserReadRepository.GetAll().ToList();
                //菜单列表
                SortMenuForTree(null, moduleIdList, moduleList, currentUser.Menus);


                var roleNamestr = userRole.Select(t => t.Role.Name).ToList();
                var schoolIdstr = _schoolReadRepository.GetAll().Where(t => roleNamestr.Contains(t.Name) || roleNamestr.Contains("admin")).Select(t => t.Id).ToList();
                //var schoolIds = _cacheManager.GetCache("schoolIds");
                
                Session["currentUser"] = currentUser;
                Session["SchoolIds"] = schoolIdstr;
                if (model.RememberMe)
                {
                    Session.Timeout = 43200;//30天
                    //await schoolIds.SetAsync(user.Id.ToString(), schoolIdstr, new TimeSpan(720, 0, 0));
                }
                else
                {
                    Session.Timeout = 720;  //12小时
                    //await schoolIds.SetAsync(user.Id.ToString(), schoolIdstr, new TimeSpan(12, 0, 0));
                }
                ////将用户名保存到票据中
                //var ticket = new FormsAuthenticationTicket(
                //    1,
                //    user.LoginName,
                //    now,
                //    //now.Add(_expirationTimeSpan),
                //    now.AddDays(365),
                //    model.RememberMe,
                //    //JsonSerializationHelper.SerializeWithType(currentUser),
                //    user.Id.ToString(),
                //    FormsAuthentication.FormsCookiePath
                //);
                ////加密
                //var encryptedTicket = FormsAuthentication.Encrypt(ticket);

                //使用Cookie
                //var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
                //{
                //    HttpOnly = true,
                //    Secure = FormsAuthentication.RequireSSL,
                //    Path = FormsAuthentication.FormsCookiePath,

                //};
                //if (ticket.IsPersistent)
                //{
                //    cookie.Expires = ticket.Expiration;
                //}
                //if (FormsAuthentication.CookieDomain != null)
                //{
                //    cookie.Domain = FormsAuthentication.CookieDomain;
                //}
                //// 将加密后的票据保存到Cookie发送到客户端
                //HttpContext.Response.Cookies.Add(cookie);
            }
            if (!returnUrl.IsNullOrEmpty() && Url.IsLocalUrl(returnUrl))
                return Json(returnUrl);
            else
                return Json("List");
        }

        public ActionResult LoginOut()
        {
            //var cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            //if (cookie != null)
            //{
            //    cookie.Expires = DateTime.Now.AddDays(-1);
            //    Response.Cookies.Add(cookie);
            //}
            //FormsAuthentication.SignOut();
            Session["currentUser"] = null;

            return RedirectToAction("Login");
        }

        public ActionResult SendRestPwdEmail()
        {
            return Json("");
        }

        /// <summary>
        /// 菜单节点
        /// </summary>
        /// <param name="parentId">父节点</param>
        /// <param name="moduleIdList"></param>
        /// <param name="allModule"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public void SortMenuForTree(int? parentId, List<int> moduleIdList, List<Module> allModule, List<SideBarMenuModel> model)
        {
            var modules = allModule.Where(m => m.ParentId == parentId && m.IsMenu && moduleIdList.Contains(m.Id)).OrderBy(m => m.OrderSort);
            foreach (var p in modules)
            {
                var menu = new SideBarMenuModel
                {
                    Name = p.Name,
                    Controller = p.Controller,
                    Action = p.Action,
                    Icon = p.Icon
                };
                SortMenuForTree(p.Id, moduleIdList, allModule, menu.ChildMenus);
                model.Add(menu);
            }
        }
    }
}