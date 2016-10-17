using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Web.Models;
using isriding.Entities.Authen;
using isriding.Helper;
using isriding.Web.Extension.Fliter;
using isriding.Web.Models.Common;

namespace isriding.Web.Controllers
{
    public class CommonController : isridingControllerBase
    {
        private readonly IRepository<Module> _moduleUserRepository;
        private readonly IRepository<UserRole> _userRoleRepository;
        private readonly IRepository<RoleModulePermission> _roleModulePermissionRepository;
        private readonly IRepository<BackUser> _backUserRepository; 

        public CommonController(IRepository<Module> moduleUserRepository, IRepository<UserRole> userRoleRepository, IRepository<RoleModulePermission> roleModulePermissionRepository, IRepository<BackUser> backUserRepository)
        {
            _moduleUserRepository = moduleUserRepository;
            _userRoleRepository = userRoleRepository;
            _roleModulePermissionRepository = roleModulePermissionRepository;
            _backUserRepository = backUserRepository;
        }

        [ChildActionOnly, UnitOfWork]
        public virtual ActionResult SidebarMenu()
        {
            var model = new List<SideBarMenuModel>();
            //var currentUser = Session["CurrentUser"] as BackUser;
            //var cookie = Request.Cookies[FormsAuthentication.FormsCookieName];

            //if (cookie == null)
            //{
            //    return PartialView(model);
            //}

            //var ticket = FormsAuthentication.Decrypt(cookie.Value);

            //if (ticket == null)
            //{
            //    return PartialView(model);
            //}

            //var currentUser = JsonSerializationHelper.DeserializeWithType<BackLoginModel>(ticket.UserData);

            var currentUser = Session["currentUser"] as BackLoginModel;

            if (currentUser == null)
                return PartialView(model);

            return PartialView(currentUser.Menus);
        }

        [AdminLayout]
        public ActionResult ChangePwd()
        {
            var model = new ChangePwdModel();
            return View(model);
        }

        [UnitOfWork, HttpPost]
        public virtual ActionResult ChangePwd(ChangePwdModel model)
        {
            if (model.NewLoginPwdConfirm == model.NewLoginPwd)
            {
                var currentUser = Session["currentUser"] as BackLoginModel;
                var user = _backUserRepository.Get(currentUser.Id);
                user.LoginPwd = DESProvider.EncryptString(model.NewLoginPwd);
                _backUserRepository.Update(user);
                return Json(new {result = true}, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new {result = false}, JsonRequestBehavior.AllowGet);
            }
        }

        [DontWrapResult]
        public ActionResult CheckPwd(string oldLoginPwd)
        {
            bool result = true;
            var currentUser = Session["currentUser"] as BackLoginModel;
            var temp = DESProvider.EncryptString(oldLoginPwd);
            if (currentUser == null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            if (currentUser.LoginPwd != temp)
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}