using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using Abp.Domain.Uow;
using Abp.Web.Models;
using isriding.Entities.Authen;
using isriding.Web.Extension.Fliter;
using isriding.Web.Models.Authen;
using isriding.Web.Models.Common;
using AutoMapper;
using isriding.Authen.Module;
using isriding.Authen.ModulePermission;
using isriding.Authen.Permission;

namespace isriding.Web.Controllers.Authen
{
    public class ModuleController : isridingControllerBase
    {
        private readonly IModuleWriteRepository _moduleRepository;
        private readonly IPermissionWriteRepository _permissionRepository;
        private readonly IModulePermissionWriteRepository _modulePermissionRepository;
        //private readonly ISchoolWriteRepository _schoolRepository; 
        private readonly IModuleReadRepository _moduleReadRepository;
        private readonly IPermissionReadRepository _permissionReadRepository;
        private readonly IModulePermissionReadRepository _modulePermissionReadRepository;

        public ModuleController(IModuleWriteRepository moduleRepository
            , IPermissionWriteRepository permissionRepository
            , IModulePermissionWriteRepository modulePermissionRepository
            , IModuleReadRepository moduleReadRepository
            , IPermissionReadRepository permissionReadRepository
            , IModulePermissionReadRepository modulePermissionReadRepository)
        {
            _moduleRepository = moduleRepository;
            _permissionRepository = permissionRepository;
            _modulePermissionRepository = modulePermissionRepository;
            //_schoolRepository = schoolRepository;
            _moduleReadRepository = moduleReadRepository;
            _permissionReadRepository = permissionReadRepository;
            _modulePermissionReadRepository = modulePermissionReadRepository;
        }

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }
        [AdminLayout]
        //[AdminPermission(PermissionCustomMode.Enforce)]
        public ActionResult List()
        {
            var model = new ModuleModel();
            PrepareAllUserModel(model);
            return View(model);
        }

        [DontWrapResult, UnitOfWork]
        public virtual ActionResult InitDataTable(DataTableParameter param)
        {
            var expr = BuildSearchCriteria();
            var temp = _moduleReadRepository.GetAll();
            if (expr != null)
            {
                temp = temp.Where(expr);
            }
            var query = temp.OrderBy(s => s.Id).Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var total = temp.Count();
            var filterResult = query.Select(t => new ModuleModel
            {
                Id = t.Id,
                Name = "<i class='" + t.Icon + "'></i> " + t.Name,
                Code = t.Code,
                Icon = t.Icon,
                ParentId = t.ParentId,
                ParentName = t.ParentModule != null ? t.ParentModule.Name : "",
                LinkUrl = t.LinkUrl ?? "",
                OrderSort = t.OrderSort,
                IsMenu = t.IsMenu,
                Enabled = t.Enabled,
                Area = t.Area,
                Controller = t.Controller,
                Action = t.Action
            }).ToList();
            int sortId = param.iDisplayStart + 1;
            var result = from t in filterResult
                         select new[]
                             {
                                sortId++.ToString(),
                                t.Name,
                                t.Code,
                                t.ParentName,
                                t.LinkUrl,
                                t.OrderSort.ToString(),
                                t.IsMenu ? "1":"0",
                                t.Enabled ? "1":"0",
                                t.Id.ToString()
                            };

            return DataTableJsonResult(param.sEcho, param.iDisplayStart, total, total, result);
        }

        public ActionResult Create()
        {
            var model = new ModuleModel();
            PrepareAllUserModel(model);
            return PartialView(model);
        }

        [HttpPost, UnitOfWork]
        public virtual ActionResult Create(ModuleModel model)
        {
            if (ModelState.IsValid)
            {
                Mapper.Initialize(t => t.CreateMap<ModuleModel, Module>());
                var module = Mapper.Map<Module>(model);
                if (!string.IsNullOrEmpty(model.LinkUrl))
                {
                    string[] link = model.LinkUrl.Split('/');
                    if (link.Length > 2)
                    {
                        module.Area = link[0];
                        module.Controller = link[1];
                        module.Action = link[2];
                    }
                    else
                    {
                        module.Controller = link[0];
                        module.Action = link[1];
                    }
                }
                _moduleRepository.Insert(module);
                //SuccessNotification("添加成功");
                return Json(model);
            }
            return Json(null);
        }

        [UnitOfWork]
        public virtual ActionResult SetButton(int id)
        {
            var module = _moduleReadRepository.Get(id);
            var model = new ButtonModel();
            if (module == null)
            {
                return PartialView(model);
            }
            model.ModuleId = module.Id;
            model.ModuleName = module.Name;
            PrepareAllUserModel(model);

            var modelpermission = _modulePermissionReadRepository.GetAllList(t => t.ModuleId == id);

            foreach (var item in modelpermission)
            {
                model.SelectedButtonList.Add(item.PermissionId);
            }
            return PartialView(model);
        }
        [HttpPost, UnitOfWork]
        public virtual ActionResult SetButton(ButtonModel model)
        {
            if (ModelState.IsValid)
            {
                //删除重复
                _modulePermissionRepository.Delete(mp=> mp.ModuleId == model.ModuleId && !model.SelectedButtonList.Contains(mp.PermissionId));
                var modulePermissionList =
                    _modulePermissionRepository.GetAll().Where(mp => mp.ModuleId == model.ModuleId).ToList();
                //List<ModulePermission> setMP = new List<ModulePermission>();
                foreach (var permissionId in model.SelectedButtonList)
                {
                    if (modulePermissionList.All(ur => ur.PermissionId != permissionId))
                    {
                        //setMP.Add(new ModulePermission
                        //{
                        //    ModuleId = model.ModuleId,
                        //    PermissionId = permissionId
                        //});
                        _modulePermissionRepository.Insert(new ModulePermission
                        {
                            ModuleId = model.ModuleId,
                            PermissionId = permissionId
                        });
                    }
                }
                //_modulePermissionRepository.Insert(new ModulePermission());
                //_unitOfWorkManager.Current.SaveChanges();
                return PartialView(model);
                
            }
            else
            {
                return PartialView(model);
            }
        }

        [UnitOfWork]
        public virtual ActionResult Edit(int id)
        {
            Mapper.Initialize(t => t.CreateMap<Module, ModuleModel>());
            var entity = _moduleReadRepository.Get(id);
            var model = Mapper.Map<ModuleModel>(entity);
            //var model = role.ToModel();
            PrepareAllUserModel(model);
            return PartialView(model);
        }

        [HttpPost, UnitOfWork]
        public virtual ActionResult Edit(ModuleModel model)
        {
            if (ModelState.IsValid)
            {
                var module = _moduleRepository.FirstOrDefault(m => m.Id == model.Id);
                module.Icon = model.Icon;
                module.IsMenu = model.IsMenu;
                module.LinkUrl = model.LinkUrl;
                module.OrderSort = model.OrderSort;
                module.ParentId = model.ParentId;
                module.Code = model.Code;
                module.Enabled = model.Enabled;
                module.Name = model.Name;
                module.Description = model.Description;
                if (!string.IsNullOrEmpty(model.LinkUrl))
                {
                    string[] link = model.LinkUrl.Split('/');
                    if (link.Length > 2)
                    {
                        module.Area = link[0];
                        module.Controller = link[1];
                        module.Action = link[2];
                    }
                    else
                    {
                        module.Controller = link[0];
                        module.Action = link[1];
                    }
                }

                _moduleRepository.InsertOrUpdate(module);
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
            _moduleRepository.Delete(s => s.Id == id);
            //var role = _roleService.GetRoleById(id);
            //_roleService.DeleteRole(role);

            return Json(new { success = true });
        }

        #region 公共方法
        [NonAction, UnitOfWork]
        protected virtual void PrepareAllUserModel(ModuleModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));
            //var schoolid = CommonHelper.GetSchoolId();
            model.ParentModuleItems.AddRange(
                _moduleReadRepository.GetAll()
                    .Where(m => m.Enabled && m.IsMenu)
                    .OrderBy(m => m.OrderSort)
                    .Select(m => new SelectListItem {Text = m.Name, Value = m.Id.ToString()}));
            model.ParentModuleItems.Insert(0, new SelectListItem {Text = "--根模块--", Value = ""});

            //var temp = _schoolRepository.GetAll()
            //        .Where(t => t.TenancyName.ToLower() != "default")
            //        .Select(b => new SelectListItem { Text = b.Name, Value = b.Id.ToString() })
            //        .ToList();
            //model.SchoolList.AddRange(temp);
            //model.SchoolList.Insert(0, new SelectListItem { Text = "---请选择---", Value = "0" });
            //model.Search.SchoolList.AddRange(temp);
            //model.Search.SchoolList.Insert(0, new SelectListItem { Text = "---请选择---", Value = "0" });

        }

        [NonAction, UnitOfWork]
        protected virtual void PrepareAllUserModel(ButtonModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));
            //var schoolid = CommonHelper.GetSchoolId();
            model.ButtonList =
                _permissionReadRepository.GetAll()
                    .Where(r => r.Enabled)
                    .OrderBy(r => r.OrderSort)
                    .Select(r => new KeyValueModel { Text = r.Name, Value = r.Id.ToString() })
                    .ToList();

            //model.RoleList.Add(
            //    _schoolRepository.GetAll().Select(b => new SelectListItem { Text = b.Name, Value = b.Id.ToString() }));
        }
        #region 构建查询表达式
        /// <summary>
        /// 构建查询表达式
        /// </summary>
        /// <returns></returns>
        private Expression<Func<Module, Boolean>> BuildSearchCriteria()
        {
            DynamicLambda<Module> bulider = new DynamicLambda<Module>();
            Expression<Func<Module, Boolean>> expr = null;
            if (!string.IsNullOrEmpty(Request["Name"]))
            {
                var data = Request["Name"].Trim();
                Expression<Func<Module, Boolean>> tmp = t => t.Name.Contains(data);
                expr = bulider.BuildQueryAnd(expr, tmp);
            }
            if (!string.IsNullOrEmpty(Request["Code"]))
            {
                var data = Request["Code"].Trim();
                Expression<Func<Module, Boolean>> tmp = t => t.Code.Contains(data);
                expr = bulider.BuildQueryAnd(expr, tmp);
            }
            if (!string.IsNullOrEmpty(Request["IsMenu"]) && Request["IsMenu"].Trim() != "-1")
            {
                var data = Convert.ToInt32(Request["IsMenu"].Trim()) == 1;
                Expression<Func<Module, Boolean>> tmp = t => t.IsMenu == data;
                expr = bulider.BuildQueryAnd(expr, tmp);
            }
            if (!string.IsNullOrEmpty(Request["Enabled"]) && Request["Enabled"].Trim() != "-1")
            {
                var data = Convert.ToInt32(Request["Enabled"].Trim()) == 1;
                Expression<Func<Module, Boolean>> tmp = t => t.Enabled == data;
                expr = bulider.BuildQueryAnd(expr, tmp);
            }
            //var id = CommonHelper.GetSchoolId();
            //Expression<Func<Module, Boolean>> tmpSolid = t => t.School_id == 1;
            //expr = bulider.BuildQueryAnd(expr, tmpSolid);

            return expr;
        }

        #endregion
        #endregion
    }
}