using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using Abp.Domain.Uow;
using Abp.UI;
using Abp.Web.Models;
using isriding.Entities.Authen;
using isriding.Web.Extension.Fliter;
using isriding.Web.Models.Authen;
using isriding.Web.Models.Common;
using AutoMapper;
using isriding.Authen.Module;
using isriding.Authen.ModulePermission;
using isriding.Authen.Permission;
using isriding.Authen.Role;
using isriding.Authen.RoleModulePermission;
using Newtonsoft.Json;

namespace isriding.Web.Controllers.Authen
{
    public class RoleController : isridingControllerBase
    {
        private readonly IRoleWriteRepository _roleRepository;
        private readonly IModuleWriteRepository _moduleRepository;
        private readonly IPermissionWriteRepository _permissionRepository;
        private readonly IModulePermissionWriteRepository _modulePermissionRepository; 
        private readonly IRoleModulePermissionWriteRepository _roleModulePermissionRepository;
        //private readonly ISchoolWriteRepository _schoolRepository;
        private readonly IRoleReadRepository _roleReadRepository;
        private readonly IModuleReadRepository _moduleReadRepository;
        private readonly IPermissionReadRepository _permissionReadRepository;
        private readonly IModulePermissionReadRepository _modulePermissionReadRepository;
        private readonly IRoleModulePermissionReadRepository _roleModulePermissionReadRepository;

        public RoleController(IRoleWriteRepository roleRepository
            , IModuleWriteRepository moduleRepository
            , IPermissionWriteRepository permissionRepository
            , IModulePermissionWriteRepository modulePermissionRepository
            , IRoleModulePermissionWriteRepository roleModulePermissionRepository
            , IRoleReadRepository roleReadRepository
            , IModuleReadRepository moduleReadRepository
            , IPermissionReadRepository permissionReadRepository
            , IModulePermissionReadRepository modulePermissionReadRepository
            , IRoleModulePermissionReadRepository roleModulePermissionReadRepository)
        {
            _roleRepository = roleRepository;
            _moduleRepository = moduleRepository;
            _permissionRepository = permissionRepository;
            _modulePermissionRepository = modulePermissionRepository;
            _roleModulePermissionRepository = roleModulePermissionRepository;
            //_schoolRepository = schoolRepository;
            _roleReadRepository = roleReadRepository;
            _moduleReadRepository = moduleReadRepository;
            _permissionReadRepository = permissionReadRepository;
            _modulePermissionReadRepository = modulePermissionReadRepository;
            _roleModulePermissionReadRepository = roleModulePermissionReadRepository;
        }
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }
        [AdminLayout]
        //[AdminPermission(PermissionCustomMode.Enforce)]
        public ActionResult List()
        {
            var model = new RoleModel();
            PrepareAllUserModel(model);
            return View(model);
        }

        [DontWrapResult, UnitOfWork]
        public virtual ActionResult InitDataTable(DataTableParameter param)
        {
            var expr = BuildSearchCriteria();
            var temp = _roleReadRepository.GetAll();
            if (expr != null)
            {
                temp = temp.Where(expr);
            }
            var query = temp.OrderBy(s => s.Id).Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var total = temp.Count();
            var filterResult = query.Select(t => new RoleModel
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                OrderSort = t.OrderSort,
                Enabled = t.Enabled
            }).ToList();
            int sortId = param.iDisplayStart + 1;
            var result = from t in filterResult
                         select new[]
                             {
                                sortId++.ToString(),
                                t.Name,
                                t.Description,
                                t.OrderSort.ToString(),
                                t.Enabled ? "1":"0",
                                t.Id.ToString()
                            };

            return DataTableJsonResult(param.sEcho, param.iDisplayStart, total, total, result);
        }

        public ActionResult Create()
        {
            var model = new RoleModel();
            PrepareAllUserModel(model);
            return PartialView(model);
        }

        [HttpPost, UnitOfWork]
        public virtual ActionResult Create(RoleModel model)
        {
            if (ModelState.IsValid)
            {
                Mapper.Initialize(t=> t.CreateMap<RoleModel, Role>());
                var role = Mapper.Map<Role>(model);
                //role.School_id = 1;
                _roleRepository.Insert(role);

                //SuccessNotification("添加成功");
                return Json(model);
            }
            return Json(null);
        }
        [UnitOfWork]
        public virtual ActionResult Edit(int id)
        {
            Mapper.Initialize(t=> t.CreateMap<Role, RoleModel>());
            var model = Mapper.Map<RoleModel>(_roleReadRepository.Get(id));
            //var model = role.ToModel();
            PrepareAllUserModel(model);
            return PartialView(model);
        }

        [HttpPost, UnitOfWork]
        public virtual ActionResult Edit(RoleModel model)
        {
            var user = _roleRepository.Get(model.Id);

            if (ModelState.IsValid)
            {
                user.Name = model.Name;
                user.Description = model.Description;
                user.OrderSort = model.OrderSort;
                user.Enabled = model.Enabled;

                user = _roleRepository.Update(user);
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
            _roleRepository.Delete(s => s.Id == id);
            //var role = _roleService.GetRoleById(id);
            //_roleService.DeleteRole(role);

            return Json(new { success = true });
        }

        [UnitOfWork]
        public virtual ActionResult SetPermission(int id)
        {
            //角色 - 菜单
            var model = new RoleSelectedModuleModel();
            //var schoolid = CommonHelper.GetSchoolId();
            #region 角色

            var role = _roleReadRepository.Get(id);
            model.RoleId = role.Id;
            model.RoleName = role.Name;

            #endregion

            #region 菜单
            //菜单列表
            model.ModuleDataList =
                _moduleReadRepository.GetAll()
                    .Where(m => m.IsMenu && m.Enabled)
                    .Select(m => new ModuleModel1
                    {
                        ModuleId = m.Id,
                        ParentId = m.ParentId,
                        ModuleName = m.Name,
                        Code = m.Code,
                    })
                    .OrderBy(m => m.Code)
                    .ToList();

            //选中菜单
            var selectdModule =
                _roleModulePermissionReadRepository.GetAll()
                    .Where(t => t.RoleId == id)
                    .Select(t => t.ModuleId)
                    .Distinct()
                    .ToList();

            foreach (var item in model.ModuleDataList)
            {
                if (selectdModule.Contains(item.ModuleId))
                {
                    item.Selected = true;
                }
            }
            #endregion

            return PartialView(model);
        }

        [UnitOfWork, DontWrapResult]
        public virtual ActionResult GetPermission(int roleId, string selectedModules)
        {
            //选中模块
            var selectedModuleId = new List<int>();

            string[] strSelectedModules = selectedModules.Split(',');
            foreach (var item in strSelectedModules)
            {
                var temp = Convert.ToInt32(item);
                if (!selectedModuleId.Contains(temp))
                    selectedModuleId.Add(temp);
            }
            //var schoolid = CommonHelper.GetSchoolId();
            //权限列表
            var model = new RoleSelectedPermissionModel();
            //table头
            model.HeaderPermissionList =
                _permissionReadRepository.GetAll()
                .Where(p => p.Enabled)
                .Select(p => new PermissionModel1
                {
                    PermissionId = p.Id,
                    PermissionName = p.Name,
                    OrderSort = p.OrderSort
                }).ToList();

            
            var allModuleList = _moduleReadRepository.GetAllList();
            var selectedModuleList = allModuleList.Where(m => selectedModuleId.Contains(m.Id)).ToList();


            //模块包含的按钮集合
            var modulePermissionList =
                _modulePermissionReadRepository.GetAll().Where(t => selectedModuleId.Contains(t.ModuleId)).ToList();
            var selectedModulePermissionList =
                _roleModulePermissionReadRepository.GetAll()
                    .Where(t => t.RoleId == roleId && selectedModuleId.Contains(t.ModuleId))
                    .ToList();

            foreach (var item in selectedModuleList)
            {
                var modulePermissionModel = new ModulePermissionModel
                {
                    ModuleId = item.Id,
                    ParentId = item.ParentId,
                    LinkUrl = item.LinkUrl ?? "",
                    ModuleName = item.Name,
                    Code = item.Code
                };

                //所有权限列表
                foreach (var permission in model.HeaderPermissionList)
                {
                    modulePermissionModel.PermissionDataList.Add(new PermissionModel1
                    {
                        PermissionId = permission.PermissionId,
                        PermissionName = permission.PermissionName,
                        OrderSort = permission.OrderSort,
                    });
                }

                var modulePermission = modulePermissionList.Where(m => m.ModuleId == item.Id).ToList();
                var selectedModulePermission = selectedModulePermissionList.Where(m => m.ModuleId == item.Id).ToList();
                var childmodulecount = allModuleList.Count(t => t.ParentId == item.Id);
                if (childmodulecount > 0 && selectedModulePermission.Any())
                {
                    modulePermissionModel.Selected = true;
                }

                foreach (var mp in modulePermission)
                {
                    var permission = model.HeaderPermissionList.FirstOrDefault(t => t.PermissionId == mp.PermissionId);

                    foreach (var p in modulePermissionModel.PermissionDataList)
                    {
                        if (permission != null && p.PermissionId == permission.PermissionId)
                        {
                            //设置Checkbox可用
                            p.Enabled = true;
                            //设置选中
                            var rmp = selectedModulePermission.FirstOrDefault(t=> t.PermissionId == permission.PermissionId);
                            if (rmp != null)
                            {
                                //设置父节点选中
                                modulePermissionModel.Selected = true;
                                p.Selected = true;
                            }
                        }
                    }

                }
                model.ModulePermissionDataList.Add(modulePermissionModel);
            }
            //权限按照Code排序
            model.ModulePermissionDataList = model.ModulePermissionDataList.OrderBy(t => t.Code).ToList();

            return PartialView(model);
        }

        [UnitOfWork, HttpPost]
        public virtual ActionResult SetPermission(int roleId, string isSet, string newModulePermission)
        {
            if (isSet == "0")
            {
                throw new UserFriendlyException("请选择按钮权限");
            }
            var newModulePermissionList = JsonConvert.DeserializeObject<List<RoleModulePermissionModel>>(newModulePermission);
            
            
            //选中的模块权限
            var oldModulePermissionList =
                _roleModulePermissionRepository.GetAll()
                    .Where(t => t.RoleId == roleId)
                    .Select(t => new RoleModulePermissionModel
                    {
                        RoleId = t.RoleId,
                        ModuleId = t.ModuleId,
                        PermissionId = t.PermissionId
                    }).ToList();
            var sameModulePermissionList = oldModulePermissionList.Intersect(newModulePermissionList);
            var addModulePermissionList = newModulePermissionList.Except(sameModulePermissionList);
            var removeModulePermissionList = oldModulePermissionList.Except(sameModulePermissionList);

            foreach (var item in removeModulePermissionList)
            {
                _roleModulePermissionRepository.Delete(t => t.RoleId == item.RoleId && t.ModuleId == item.ModuleId && t.PermissionId == item.PermissionId);
            }
            //提交删除
            CurrentUnitOfWork.SaveChanges();
            foreach (var item in addModulePermissionList)
            {
                _roleModulePermissionRepository.Insert(new RoleModulePermission
                {
                    RoleId = item.RoleId,
                    PermissionId = item.PermissionId,
                    ModuleId = item.ModuleId
                });
            }
            //提交添加
            CurrentUnitOfWork.SaveChanges();
            return Json(newModulePermissionList);
        }

        #region 构建查询表达式
        /// <summary>
        /// 构建查询表达式
        /// </summary>
        /// <returns></returns>
        private Expression<Func<Role, Boolean>> BuildSearchCriteria()
        {
            DynamicLambda<Role> bulider = new DynamicLambda<Role>();
            Expression<Func<Role, Boolean>> expr = null;
            if (!string.IsNullOrEmpty(Request["Name"]))
            {
                var data = Request["Name"].Trim();
                Expression<Func<Role, Boolean>> tmp = t => t.Name.Contains(data);
                expr = bulider.BuildQueryAnd(expr, tmp);
            }
            if (!string.IsNullOrEmpty(Request["Enabled"]) && Request["Enabled"].Trim() != "-1")
            {
                var data = Convert.ToInt32(Request["Enabled"].Trim()) == 1;
                Expression<Func<Role, Boolean>> tmp = t => t.Enabled == data;
                expr = bulider.BuildQueryAnd(expr, tmp);
            }
            //var id = CommonHelper.GetSchoolId();
            //if (id > 1)
            //{
            //    Expression<Func<Role, Boolean>> tmpSolid = t => t.School_id == id;
            //    expr = bulider.BuildQueryAnd(expr, tmpSolid);
            //}
            return expr;
        }

        #endregion

        [NonAction, UnitOfWork]
        protected virtual void PrepareAllUserModel(RoleModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            //var temp = _schoolRepository.GetAll()
            //        .Where(t => t.TenancyName.ToLower() != "default")
            //        .Select(b => new SelectListItem { Text = b.Name, Value = b.Id.ToString() })
            //        .ToList();
            //model.SchoolList.AddRange(temp);
            //model.SchoolList.Insert(0, new SelectListItem { Text = "---请选择---", Value = "0" });
            //model.Search.SchoolList.AddRange(temp);
            //model.Search.SchoolList.Insert(0, new SelectListItem { Text = "---请选择---", Value = "0" });

        }
    }
}