using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Web.Models;
using isriding.Web.Helper;
using isriding.Web.Models.Common;
using isriding.Web.Models.School;
using AutoMapper;

namespace isriding.Web.Controllers.School
{
    public class VersionUpdateController : isridingControllerBase
    {
        private readonly IRepository<Entities.VersionUpdate> _versionUpdateRepository;

        public VersionUpdateController(IRepository<Entities.VersionUpdate> versionUpdateRepository)
        {
            _versionUpdateRepository = versionUpdateRepository;
        }

        // GET: VersionUpdate
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            var model = new VersionUpdateModel();
            return View(model);
        }

        [DontWrapResult, UnitOfWork]
        public virtual ActionResult InitDataTable(DataTableParameter param)
        {

            var expr = BuildSearchCriteria();
            var temp = _versionUpdateRepository.GetAll();
            if (expr != null)
            {
                temp = temp.Where(expr);
            }
            
            if (expr != null)
            {
                temp = temp.Where(expr);
            }
            var query = temp.OrderBy(s => s.Id).Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var total = temp.Count();
            var filterResult = query.Select(t => new VersionUpdateModel
            {
                Id = t.Id,
                device_os = t.device_os,
                versionCode = t.versionCode,
                versionName = t.versionName,
                versionUrl = t.versionUrl,
                upgrade = t.upgrade
            }).ToList();
            int sortId = param.iDisplayStart + 1;
            var result = from t in filterResult
                         select new[]
                             {
                                sortId++.ToString(),
                                t.device_os.ToString(),
                                t.versionCode.ToString(),
                                t.versionName,
                                t.versionUrl,
                                t.upgrade.ToString(),
                                t.Id.ToString()
                            };

            return DataTableJsonResult(param.sEcho, param.iDisplayStart, total, total, result);
        }

        public ActionResult Create()
        {
            var model = new VersionUpdateModel();
            return PartialView(model);
        }

        [HttpPost, UnitOfWork]
        public virtual ActionResult Create(VersionUpdateModel model)
        {
            if (ModelState.IsValid)
            {
                Mapper.Initialize(t=> t.CreateMap<VersionUpdateModel, Entities.VersionUpdate>());
                var user = Mapper.Map<Entities.VersionUpdate>(model);
                _versionUpdateRepository.Insert(user);

                //SuccessNotification("添加成功");
                return Json(model);
            }
            return Json(null);
        }
        [UnitOfWork]
        public virtual ActionResult Edit(int id)
        {
            Mapper.Initialize(t=> t.CreateMap<Entities.VersionUpdate, VersionUpdateModel>());
            var model = Mapper.Map<VersionUpdateModel>(_versionUpdateRepository.Get(id));
            return PartialView(model);
        }

        [HttpPost, UnitOfWork]
        public virtual ActionResult Edit(VersionUpdateModel model)
        {
            var user = _versionUpdateRepository.Get(model.Id);

            if (ModelState.IsValid)
            {
                user.versionCode = model.versionCode;
                user.versionName = model.versionName;
                user.versionUrl = model.versionUrl;
                user.upgrade = model.upgrade;
                _versionUpdateRepository.Update(user);

                //SuccessNotification("更新成功");
                return Json(model);
            }
            return Json(null);
        }

        [HttpPost, UnitOfWork]
        public virtual ActionResult Delete(int id)
        {
            _versionUpdateRepository.Delete(s => s.Id == id);
            //var role = _roleService.GetRoleById(id);
            //_roleService.DeleteRole(role);

            return Json(new { success = true });
        }

        #region 公共方法
        
        #region 构建查询表达式
        /// <summary>
        /// 构建查询表达式
        /// </summary>
        /// <returns></returns>
        private Expression<Func<Entities.VersionUpdate, Boolean>> BuildSearchCriteria()
        {
            DynamicLambda<Entities.VersionUpdate> bulider = new DynamicLambda<Entities.VersionUpdate>();
            Expression<Func<Entities.VersionUpdate, Boolean>> expr = null;
            if (!string.IsNullOrEmpty(Request["versionCode"]))
            {
                var dataint = 0;
                int.TryParse(Request["versionCode"].Trim(),out dataint);
                Expression<Func<Entities.VersionUpdate, Boolean>> tmp = t => t.versionCode == dataint;
                expr = bulider.BuildQueryAnd(expr, tmp);
            }
            if (!string.IsNullOrEmpty(Request["device_os"]) && Request["device_os"].Trim() != "0")
            {
                var dataint = 0;
                int.TryParse(Request["device_os"].Trim(), out dataint);
                Expression<Func<Entities.VersionUpdate, Boolean>> tmp = t => t.device_os == dataint;
                expr = bulider.BuildQueryAnd(expr, tmp);
            }
            if (!string.IsNullOrEmpty(Request["versionName"]))
            {
                var data = Request["versionName"].Trim();
                Expression<Func<Entities.VersionUpdate, Boolean>> tmp = t => t.versionName.Contains(data);
                expr = bulider.BuildQueryAnd(expr, tmp);
            }
            if (!string.IsNullOrEmpty(Request["upgrade"]) && Request["upgrade"].Trim() != "0")
            {
                var data = Convert.ToInt32(Request["upgrade"].Trim());
                Expression<Func<Entities.VersionUpdate, Boolean>> tmp = t => t.upgrade == data;
                expr = bulider.BuildQueryAnd(expr, tmp);
            }
            return expr;
        }
        #endregion
        #endregion
    }
}