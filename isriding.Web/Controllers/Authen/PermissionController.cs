using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Web.Models;
using isriding.Entities.Authen;
using isriding.Web.Extension.Fliter;
using isriding.Web.Helper;
using isriding.Web.Models.Authen;
using isriding.Web.Models.Common;
using AutoMapper;

namespace isriding.Web.Controllers.Authen
{
    public class PermissionController : isridingControllerBase
    {
        private readonly IRepository<Permission> _permissionRepository;
        private readonly IRepository<Entities.School> _schoolRepository;

        public PermissionController(IRepository<Permission> permissionRepository, IRepository<Entities.School> schoolRepository)
        {
            _permissionRepository = permissionRepository;
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
            var model = new PermissionModel();
            PrepareAllUserModel(model);
            return View(model);
        }

        [DontWrapResult, UnitOfWork]
        public virtual ActionResult InitDataTable(DataTableParameter param)
        {
            var expr = BuildSearchCriteria();
            var temp = _permissionRepository.GetAll();
            if (expr != null)
            {
                temp = temp.Where(expr);
            }
            var query = temp.OrderBy(s => s.Id).Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var total = temp.Count();
            var filterResult = query.Select(t => new PermissionModel
            {
                Id = t.Id,
                Code = t.Code,
                Name = t.Name,
                Description = t.Description,
                OrderSort = t.OrderSort,
                Icon = t.Icon,
                Enabled = t.Enabled
            }).ToList();
            int sortId = param.iDisplayStart + 1;
            var result = from t in filterResult
                         select new[]
                             {
                                sortId++.ToString(),
                                t.Name,
                                t.Code,
                                t.Icon,
                                t.OrderSort.ToString(),
                                t.Description,
                                t.Enabled ? "1":"0",
                                t.Id.ToString()
                            };

            return DataTableJsonResult(param.sEcho, param.iDisplayStart, total, total, result);
        }

        public ActionResult Create()
        {
            var model = new PermissionModel();
            PrepareAllUserModel(model);
            return PartialView(model);
        }

        [HttpPost, UnitOfWork]
        public virtual ActionResult Create(PermissionModel model)
        {
            if (ModelState.IsValid)
            {
                Mapper.Initialize(t=> t.CreateMap<PermissionModel, Permission>());
                var permission = Mapper.Map<Permission>(model);
                //permission.School_id = 1;
                _permissionRepository.Insert(permission);

                //SuccessNotification("添加成功");
                return Json(model);
            }
            return Json(null);
        }
        [UnitOfWork]
        public virtual ActionResult Edit(int id)
        {
            Mapper.Initialize(t=> t.CreateMap<Permission, PermissionModel>());
            var model = Mapper.Map<PermissionModel>(_permissionRepository.Get(id));
            //var model = role.ToModel();
            PrepareAllUserModel(model);
            return PartialView(model);
        }

        [HttpPost, UnitOfWork]
        public virtual ActionResult Edit(PermissionModel model)
        {
            var user = _permissionRepository.Get(model.Id);

            if (ModelState.IsValid)
            {
                user.Name = model.Name;
                user.Code = model.Code;
                user.Icon = model.Icon;
                user.Description = model.Description;
                user.OrderSort = model.OrderSort;
                user.Enabled = model.Enabled;

                _permissionRepository.Update(user);
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
            _permissionRepository.Delete(s => s.Id == id);
            //var role = _roleService.GetRoleById(id);
            //_roleService.DeleteRole(role);

            return Json(new { success = true });
        }

        [NonAction, UnitOfWork]
        protected virtual void PrepareAllUserModel(PermissionModel model)
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

        #region 构建查询表达式
        /// <summary>
        /// 构建查询表达式
        /// </summary>
        /// <returns></returns>
        private Expression<Func<Permission, Boolean>> BuildSearchCriteria()
        {
            DynamicLambda<Permission> bulider = new DynamicLambda<Permission>();
            Expression<Func<Permission, Boolean>> expr = null;
            if (!string.IsNullOrEmpty(Request["Name"]))
            {
                var data = Request["Name"].Trim();
                Expression<Func<Permission, Boolean>> tmp = t => t.Name.Contains(data);
                expr = bulider.BuildQueryAnd(expr, tmp);
            }
            if (!string.IsNullOrEmpty(Request["Code"]))
            {
                var data = Request["Code"].Trim();
                Expression<Func<Permission, Boolean>> tmp = t => t.Code.Contains(data);
                expr = bulider.BuildQueryAnd(expr, tmp);
            }
            if (!string.IsNullOrEmpty(Request["Enabled"]) && Request["Enabled"].Trim() != "-1")
            {
                var data = Convert.ToInt32(Request["Enabled"].Trim()) == 1;
                Expression<Func<Permission, Boolean>> tmp = t => t.Enabled == data;
                expr = bulider.BuildQueryAnd(expr, tmp);
            }
            //var id = CommonHelper.GetSchoolId();
            //Expression<Func<Permission, Boolean>> tmpSolid = t => t.School_id == 1;
            //expr = bulider.BuildQueryAnd(expr, tmpSolid);

            return expr;
        }

        #endregion
    }
}