using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Web.Models;
using isriding.Web.Extension.Fliter;
using isriding.Web.Helper;
using isriding.Web.Models.Common;
using isriding.Web.Models.School;
using AutoMapper;

namespace isriding.Web.Controllers.School
{
    public class SitemonitorController : isridingControllerBase
    {
        private readonly IRepository<Entities.Sitemonitor> _sitemonitorRepository;
        private readonly IRepository<Entities.Bikesite> _bikesiteRepository;
        private readonly IRepository<Entities.School> _schoolRepository;

        public SitemonitorController(IRepository<Entities.Sitemonitor> sitemonitorRepository, IRepository<Entities.Bikesite> bikesiteRepository, IRepository<Entities.School> schoolRepository)
        {
            _sitemonitorRepository = sitemonitorRepository;
            _bikesiteRepository = bikesiteRepository;
            _schoolRepository = schoolRepository;
        }

        // GET: Sitemonitor
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        [AdminLayout]
        public ActionResult List()
        {
            var model = new SitemonitorModel();
            PrepareAllUserModel(model);
            return View(model);
        }

        [DontWrapResult, UnitOfWork]
        public virtual ActionResult InitDataTable(DataTableParameter param)
        {
            var expr = BuildSearchCriteria();
            var temp = _sitemonitorRepository.GetAll();
            if (null != expr)
            {
                temp = temp.Where(expr);
            }
            var query = temp.OrderBy(s => s.Id).Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var total = temp.Count();
            var filterResult = query.Select(t => new SitemonitorModel
            {
                Id = t.Id,
                Name = t.Name,
                Bikesite_id = t.Bikesite_id,
                Bikesite_name = t.Bikesite == null? "": t.Bikesite.Name,
                Status = t.Status??1,
                Enabled = t.Enabled,
                School_name = t.Bikesite.School.Name
            }).ToList();
            int sortId = param.iDisplayStart + 1;
            var result = from t in filterResult
                         select new[]
                             {
                                sortId++.ToString(),
                                t.Name,
                                t.Bikesite_name,
                                t.School_name,
                                t.Status.ToString(),
                                t.Enabled.ToString(),
                                t.Id.ToString()
                            };

            return DataTableJsonResult(param.sEcho, param.iDisplayStart, total, total, result);
        }

        public ActionResult Create()
        {
            var model = new SitemonitorModel();
            PrepareAllUserModel(model);
            return PartialView(model);
        }

        [HttpPost, UnitOfWork]
        public virtual ActionResult Create(SitemonitorModel model)
        {
            if (ModelState.IsValid)
            {
                Mapper.Initialize(t=> t.CreateMap<SitemonitorModel, Entities.Sitemonitor>());
                var sitemonitor = Mapper.Map<Entities.Sitemonitor>(model);
                _sitemonitorRepository.Insert(sitemonitor);

                //SuccessNotification("添加成功");
                return Json(model);
            }
            return Json(null);
        }
        [UnitOfWork]
        public virtual ActionResult Edit(int id)
        {
            Mapper.Initialize(t=> t.CreateMap<Entities.Sitemonitor, SitemonitorModel>());
            var model = Mapper.Map<SitemonitorModel>(_sitemonitorRepository.Get(id));
            //var model = role.ToModel();
            PrepareAllUserModel(model);
            return PartialView(model);
        }

        [HttpPost, UnitOfWork]
        public virtual ActionResult Edit(SitemonitorModel model)
        {
            var sitemonitor = _sitemonitorRepository.Get(model.Id);

            if (ModelState.IsValid)
            {
                sitemonitor.Name = model.Name;
                sitemonitor.Bikesite_id = model.Bikesite_id;
                sitemonitor.Status = model.Status;
                sitemonitor.Enabled = model.Enabled;
                _sitemonitorRepository.Update(sitemonitor);
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
            _sitemonitorRepository.Delete(s => s.Id == id);
            //var role = _roleService.GetRoleById(id);
            //_roleService.DeleteRole(role);

            return Json(new { success = true });
        }

        #region 公共方法
        [NonAction, UnitOfWork]
        protected virtual void PrepareAllUserModel(SitemonitorModel model)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            var list =
                _bikesiteRepository.GetAll().Select(b => new SelectListItem {Text = b.Name, Value = b.Id.ToString()});

            model.BikesiteList.AddRange(list);
            model.BikesiteList.Insert(0, new SelectListItem {Text = "---请选择---", Value = "0"});
            model.Search.BikesiteList.AddRange(list);
            model.Search.BikesiteList.Insert(0, new SelectListItem { Text = "---请选择---", Value = "0" });

            var slist = _schoolRepository.GetAll();

            var sessionschoolids = Session["SchoolIds"] as List<int>;
            if (sessionschoolids != null && sessionschoolids.Count > 0)
            {
                slist = slist.Where(t => sessionschoolids.Contains(t.Id));
            }
            var schoollist = slist.ToList().Select(b => new SelectListItem { Text = b.Name, Value = b.Id.ToString() });
            model.Search.SchoolList.AddRange(schoollist);
            model.Search.SchoolList.Insert(0, new SelectListItem { Text = "---请选择---", Value = "0" });
        }
        #region 构建查询表达式
        /// <summary>
        /// 构建查询表达式
        /// </summary>
        /// <returns></returns>
        private Expression<Func<Entities.Sitemonitor, Boolean>> BuildSearchCriteria()
        {
            DynamicLambda<Entities.Sitemonitor> bulider = new DynamicLambda<Entities.Sitemonitor>();
            Expression<Func<Entities.Sitemonitor, Boolean>> expr = null;
            if (!string.IsNullOrEmpty(Request["Name"]))
            {
                var data = Request["Name"].Trim();
                Expression<Func<Entities.Sitemonitor, Boolean>> tmp = t => t.Name.Contains(data);
                expr = bulider.BuildQueryAnd(expr, tmp);
            }
            if (!string.IsNullOrEmpty(Request["Bikesite_id"]) && Request["Bikesite_id"].Trim() != "0")
            {
                var data = Convert.ToInt32(Request["Bikesite_id"].Trim());
                Expression<Func<Entities.Sitemonitor, Boolean>> tmp = t => t.Bikesite_id == data;
                expr = bulider.BuildQueryAnd(expr, tmp);
            }
            if (!string.IsNullOrEmpty(Request["Status"]) && Request["Status"].Trim() != "-1")
            {
                var data = Convert.ToInt32(Request["Status"].Trim());
                Expression<Func<Entities.Sitemonitor, Boolean>> tmp = t => t.Status == data;
                expr = bulider.BuildQueryAnd(expr, tmp);
            }
            if (!string.IsNullOrEmpty(Request["Enabled"]) && Request["Enabled"].Trim() != "-1")
            {
                var data = Convert.ToInt32(Request["Enabled"].Trim()) == 1;
                Expression<Func<Entities.Sitemonitor, Boolean>> tmp = t => t.Enabled == data;
                expr = bulider.BuildQueryAnd(expr, tmp);
            }
            if (!string.IsNullOrEmpty(Request["School_id"]))
            {
                var data = Convert.ToInt32(Request["School_id"].Trim());
                if (data > 0)
                {
                    Expression<Func<Entities.Sitemonitor, Boolean>> tmp = t => t.Bikesite.School_id == data;
                    expr = bulider.BuildQueryAnd(expr, tmp);
                }
                else
                {
                    var sessionschoolids = Session["SchoolIds"] as List<int>;
                    if (sessionschoolids != null && sessionschoolids.Count > 0)
                    {
                        Expression<Func<Entities.Sitemonitor, Boolean>> tmp = t => sessionschoolids.Contains((int)t.Bikesite.School_id);
                        expr = bulider.BuildQueryAnd(expr, tmp);
                    }
                }
            }
            else
            {
                var sessionschoolids = Session["SchoolIds"] as List<int>;
                if (sessionschoolids != null && sessionschoolids.Count > 0)
                {
                    Expression<Func<Entities.Sitemonitor, Boolean>> tmp = t => sessionschoolids.Contains((int)t.Bikesite.School_id);
                    expr = bulider.BuildQueryAnd(expr, tmp);
                }
            }
            return expr;
        }
        #endregion
        #endregion
    }
}