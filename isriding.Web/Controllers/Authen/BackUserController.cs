using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Web.Models;
using isriding.Entities.Authen;
using isriding.Web.Extension.Fliter;
using isriding.Web.Models.Authen;
using isriding.Web.Models.Common;
using AutoMapper;
using isriding.Entities;
using isriding.Helper;

namespace isriding.Web.Controllers.Authen
{
    public class BackUserController : isridingControllerBase
    {
        private readonly IRepository<BackUser> _backUserRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<UserRole> _userRoleRepository;
        private readonly IRepository<Entities.School> _schoolRepository;

        public BackUserController(IRepository<BackUser> backUserrepository, IRepository<Role> roleRepository, IRepository<UserRole> userRoleRepository, IRepository<Entities.School> schoolRepository)
        {
            _backUserRepository = backUserrepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _schoolRepository = schoolRepository;
        }
        
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }
        [AdminLayout]
        //[AdminPermission(PermissionCustomMode.Enforce)]
        public ActionResult List()
        {
            var model = new BackUserModel();
            PrepareAllUserModel(model);
            return View(model);
        }

        [DontWrapResult, UnitOfWork]
        public virtual ActionResult InitDataTable(DataTableParameter param)
        {
            var expr = BuildSearchCriteria();
            var temp = _backUserRepository.GetAll();
            if (expr != null)
            {
                temp = temp.Where(expr);
            }
            var query =
                temp.OrderBy(s => s.Id).Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var total = temp.Count();
            var filterResult = query.Select(t => new BackUserModel
            {
                Id = t.Id,
                LoginName = t.LoginName,
                FullName = t.FullName,
                Phone = t.Phone,
                Email = t.Email,
                Enabled = t.Enabled,
                PwdErrorCount = t.PwdErrorCount,
                LoginCount = t.LoginCount,
                RegisterTime = t.RegisterTime,
                LastLoginTime = t.LastLoginTime
            }).ToList();
            int sortId = param.iDisplayStart + 1;
            var result = from t in filterResult
                         select new[]
                             {
                                sortId++.ToString(),
                                t.LoginName,
                                t.FullName,
                                t.Phone,
                                t.Email,
                                t.Enabled ? "1":"0",
                                t.Id.ToString()
                            };

            return DataTableJsonResult(param.sEcho, param.iDisplayStart, total, total, result);
        }

        public ActionResult Create()
        {
            var model = new BackUserModel();
            PrepareAllUserModel(model);
            return PartialView(model);
        }

        [HttpPost, UnitOfWork]
        public virtual ActionResult Create(BackUserModel model)
        {
            if (ModelState.IsValid)
            {
                Mapper.Initialize(t => t.CreateMap<BackUserModel, BackUser>());
                var user = Mapper.Map<BackUser>(model);
                user.LoginPwd = DESProvider.EncryptString("123456");//初始密码
                _backUserRepository.Insert(user);
                //SuccessNotification("添加成功");
                foreach (var roleId in model.SelectedRoleList)
                {
                    _userRoleRepository.Insert(new UserRole
                    {
                        BackUser = user,
                        RoleId = roleId,
                        UserId = user.Id
                    });
                }
                return Json(model);
            }
            return Json(null);
        }
        [UnitOfWork]
        public virtual ActionResult Edit(int id)
        {
            Mapper.Initialize(t => t.CreateMap<BackUser, BackUserModel>());
            var entity = _backUserRepository.Get(id);
            var model = Mapper.Map<BackUserModel>(entity);
            //var model = role.ToModel();
            PrepareAllUserModel(model);
            var userrole = _userRoleRepository.GetAll().Where(t => t.UserId == model.Id).ToList();
            foreach (var item in userrole)
            {
                model.SelectedRoleList.Add(item.RoleId);
            }
            return PartialView(model);
        }

        [HttpPost, UnitOfWork]
        public virtual ActionResult Edit(BackUserModel model)
        {
            

            if (ModelState.IsValid)
            {
                //删除重复
                _userRoleRepository.Delete(ur => ur.UserId == model.Id && !model.SelectedRoleList.Contains(ur.RoleId));
                var user = _backUserRepository.Get(model.Id);
                var userrole = _userRoleRepository.GetAll().Where(t => t.UserId == model.Id).ToList();
                user.FullName = model.FullName;
                user.Phone = model.Phone;
                user.Email = model.Email;
                user.Enabled = model.Enabled;
                
                foreach (var roleId in model.SelectedRoleList)
                {
                    if (userrole.All(ur => ur.RoleId != roleId))
                    {
                        _userRoleRepository.Insert(new UserRole
                        {
                            BackUser = user,
                            RoleId = roleId,
                            UserId = user.Id
                        });
                    }
                }

                _backUserRepository.InsertOrUpdate(user);
                //role = model.ToEntity(role);
                //_roleService.UpdateRole(role);

                //SuccessNotification("更新成功");
                return Json(model);
            }
            return Json(null);
        }

        [HttpPost, UnitOfWork]
        public virtual ActionResult Delete(int id)
        {
            _backUserRepository.Delete(s => s.Id == id);
            //var role = _roleService.GetRoleById(id);
            //_roleService.DeleteRole(role);

            return Json(new { success = true });
        }

        #region 公共方法
        [NonAction, UnitOfWork]
        protected virtual void PrepareAllUserModel(BackUserModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));
            //var schoolid = CommonHelper.GetSchoolId();
            model.RoleList =
                _roleRepository.GetAll()
                    .Where(r => r.Enabled)
                    .OrderBy(r => r.OrderSort)
                    .Select(r => new KeyValueModel {Text = r.Name, Value = r.Id.ToString()})
                    .ToList();
            //var RoleNames =
            //    _userRoleRepository.GetAll().Where(t => t.UserId == user_id).Select(t => t.Role.Name.ToLower()).ToList();
            //model.RoleList.Add(
            //    _schoolRepository.GetAll().Select(b => new SelectListItem { Text = b.Name, Value = b.Id.ToString() }));
            //var temp = _schoolRepository.GetAll()
            //        .Where(t => RoleNames.Contains(t.TenancyName.ToLower()) || t.TenancyName.ToLower() == "default")
            //        .Select(b => new SelectListItem { Text = b.Name, Value = b.Id.ToString() })
            //        .ToList();
            //model.SchoolList.AddRange(temp);
            //model.SchoolList.Insert(0, new SelectListItem {Text = "---请选择---", Value = "0"});
            //model.Search.SchoolList.AddRange(temp);
            //model.Search.SchoolList.Insert(0, new SelectListItem { Text = "---请选择---", Value = "0" });
        }

        #region 构建查询表达式
        /// <summary>
        /// 构建查询表达式
        /// </summary>
        /// <returns></returns>
        private Expression<Func<BackUser, Boolean>> BuildSearchCriteria()
        {
            DynamicLambda<BackUser> bulider = new DynamicLambda<BackUser>();
            Expression<Func<BackUser, Boolean>> expr = null;
            if (!string.IsNullOrEmpty(Request["LoginName"]))
            {
                var data = Request["LoginName"].Trim();
                Expression<Func<BackUser, Boolean>> tmp = t => t.LoginName.Contains(data);
                expr = bulider.BuildQueryAnd(expr, tmp);
            }
            if (!string.IsNullOrEmpty(Request["FullName"]))
            {
                var data = Request["FullName"].Trim();
                Expression<Func<BackUser, Boolean>> tmp = t => t.FullName.Contains(data);
                expr = bulider.BuildQueryAnd(expr, tmp);
            }
            if (!string.IsNullOrEmpty(Request["Enabled"]) && Request["Enabled"].Trim() != "-1")
            {
                var data = Convert.ToInt32(Request["Enabled"].Trim()) == 1;
                Expression<Func<BackUser, Boolean>> tmp = t => t.Enabled == data;
                expr = bulider.BuildQueryAnd(expr, tmp);
            }
            if (!string.IsNullOrEmpty(Request["School_id"]) && Request["School_id"].Trim() != "0")
            {
                var data = Convert.ToInt32(Request["School_id"].Trim());
                Expression<Func<BackUser, Boolean>> tmp = t => t.School_id == data;
                expr = bulider.BuildQueryAnd(expr, tmp);
            }
            //var id = CommonHelper.GetSchoolId();
            //Expression<Func<BackUser, Boolean>> tmpSolid = t => t.School_id == id;
            //expr = bulider.BuildQueryAnd(expr, tmpSolid);

            return expr;
        }

        #endregion
        #endregion
    }
}