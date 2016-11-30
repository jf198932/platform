using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Abp.Domain.Uow;
using Abp.Json;
using isriding.Authen.Module;
using isriding.Authen.RoleModulePermission;
using isriding.Authen.UserRole;
using isriding.Web.Models.Authen;
using isriding.Web.Models.Common;

namespace isriding.Web.Extension.Fliter
{
    public class SetButton
    {
        private readonly IModuleWriteRepository _moduleRepository;
        private readonly IUserRoleWriteRepository _userRoleRepository;
        private readonly IRoleModulePermissionWriteRepository _roleModulePermissionRepository;
        public SetButton(IModuleWriteRepository moduleRepository, IUserRoleWriteRepository userRoleRepository, IRoleModulePermissionWriteRepository roleModulePermissionRepository)
        {
            _moduleRepository = moduleRepository;
            _userRoleRepository = userRoleRepository;
            _roleModulePermissionRepository = roleModulePermissionRepository;
        }

        [UnitOfWork]
        public List<PermissionButtonModel> SetButtons(HttpCookie cookie, string controller)
        {
            //var routeData = RouteData.Route.GetRouteData(this.HttpContext);
            //if (routeData != null)
            //{
            //    var controller = routeData.Values["controller"];
            //    var action = routeData.Values["action"];
            //}
            //var action = RouteData.Values["action"].ToString().ToLower();
            var ticket = FormsAuthentication.Decrypt(cookie.Value);
            var currentUser = JsonSerializationHelper.DeserializeWithType<BackLoginModel>(ticket.UserData);

            var roleIds =
                _userRoleRepository.GetAll().Where(u => u.UserId == currentUser.Id).Select(u => u.RoleId).ToList();
            var module = _moduleRepository.FirstOrDefault(m => m.Controller.ToLower() == controller);

            if (module != null)
            {
                var permissionList =
                    _roleModulePermissionRepository.GetAll()
                        .Where(r => r.ModuleId == module.Id && roleIds.Contains(r.RoleId))
                        .Select(r => r.Permission)
                        .ToList();
                var buttonList =
                    permissionList.Select(p => new PermissionButtonModel { Code = p.Code, Name = p.Name, Icon = p.Icon });
                return buttonList.ToList();
            }
            return new List<PermissionButtonModel>();
        }
    }
}