using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using Abp.Domain.Uow;
using Abp.Web.Models;
using AutoMapper;
using isriding.Recharge;
using isriding.Recharge_detail;
using isriding.School;
using isriding.Web.Extension.Fliter;
using isriding.Web.Models.Common;
using isriding.Web.Models.School;
using isriding.Web.Models.SchoolManage;

namespace isriding.Web.Controllers.SchoolManage
{
    public class RechargeController : isridingControllerBase
    {
        private readonly IRechargeReadRepository _rechargeReadRepository;
        private readonly IRechargeWriteRepository _rechargeWriteRepository;
        private readonly IRecharge_detailReadRepository _rechargeDetailReadRepository;
        private readonly ISchoolReadRepository _schoolReadRepository;

        public RechargeController(IRechargeReadRepository rechargeReadRepository
            , IRechargeWriteRepository rechargeWriteRepository
            , IRecharge_detailReadRepository rechargeDetailReadRepository
            , ISchoolReadRepository schoolReadRepository)
        {
            _rechargeReadRepository = rechargeReadRepository;
            _rechargeWriteRepository = rechargeWriteRepository;
            _rechargeDetailReadRepository = rechargeDetailReadRepository;
            _schoolReadRepository = schoolReadRepository;
        }

        // GET: Recharge
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }
        [AdminLayout]
        public ActionResult List()
        {
            var model = new RechargeModel();
            PrepareAllUserModel(model);
            return View(model);
        }

        [DontWrapResult, UnitOfWork]
        public virtual ActionResult InitDataTable(DataTableParameter param)
        {
            var expr = BuildSearchCriteria();
            var temp = _rechargeReadRepository.GetAll();
            if (expr != null)
            {
                temp = temp.Where(expr);
            }
            var query = temp.OrderBy(s => s.Id).Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var total = temp.Count();
            var filterResult = query.Select(t => new RechargeModel
            {
                Id = t.Id,
                Deposit = t.Deposit,
                Created_at = t.Created_at,
                Recharge_count = t.Recharge_count,
                Updated_at = t.Updated_at,
                User_id = t.User_id,
                User_name = t.User == null ? "": t.User.Name,
                School_name = t.User.School.Name
            }).ToList();
            int sortId = param.iDisplayStart + 1;
            var result = from t in filterResult
                         select new[]
                             {
                                sortId++.ToString(),
                                t.School_name,
                                t.User_name,
                                t.Deposit.ToString(),
                                t.Recharge_count.ToString(),
                                t.Id.ToString()
                            };

            return DataTableJsonResult(param.sEcho, param.iDisplayStart, total, total, result);
        }
        
        [UnitOfWork]
        public virtual ActionResult Edit(int id)
        {
            Mapper.Initialize(t => t.CreateMap<Entities.Recharge, RechargeModel>());
            var model = Mapper.Map<RechargeModel>(_rechargeReadRepository.Get(id));
            //var model = role.ToModel();
            PrepareAllUserModel(model);
            return PartialView(model);
        }

        [HttpPost, UnitOfWork]
        public virtual ActionResult Edit(RechargeModel model)
        {
            var user = _rechargeWriteRepository.Get(model.Id);

            if (ModelState.IsValid)
            {
                user.Deposit = model.Deposit;
                user.Recharge_count = model.Recharge_count;
                user.Updated_at = DateTime.Now;

                _rechargeWriteRepository.Update(user);

                return Json(model);
            }
            return Json(null);
        }

        [HttpPost, UnitOfWork]
        public virtual ActionResult Delete(int id)
        {
            _rechargeWriteRepository.Delete(s => s.Id == id);
            //var role = _roleService.GetRoleById(id);
            //_roleService.DeleteRole(role);

            return Json(new { success = true });
        }

        #region 公共方法
        [NonAction, UnitOfWork]
        protected virtual void PrepareAllUserModel(RechargeModel model)
        {
            if (model == null)
                throw new ArgumentNullException("model");
            var list = _schoolReadRepository.GetAll();
            
            var sessionschoolids = Session["SchoolIds"] as List<int>;
            if (sessionschoolids != null && sessionschoolids.Count > 0)
            {
                list = list.Where(t => sessionschoolids.Contains(t.Id));
            }
            var schoollist = list.Select(b => new SelectListItem { Text = b.Name, Value = b.Id.ToString() });
            model.Search.SchoolList.AddRange(schoollist);
            model.Search.SchoolList.Insert(0, new SelectListItem { Text = "---请选择---", Value = "0" });
        }
        #region 构建查询表达式
        /// <summary>
        /// 构建查询表达式
        /// </summary>
        /// <returns></returns>
        private Expression<Func<Entities.Recharge, Boolean>> BuildSearchCriteria()
        {
            DynamicLambda<Entities.Recharge> bulider = new DynamicLambda<Entities.Recharge>();
            Expression<Func<Entities.Recharge, Boolean>> expr = null;
            if (!string.IsNullOrEmpty(Request["User_name"]))
            {
                var data = Request["User_name"].Trim();
                Expression<Func<Entities.Recharge, Boolean>> tmp = t => t.User.Name.Contains(data);
                expr = bulider.BuildQueryAnd(expr, tmp);
            }
            if (!string.IsNullOrEmpty(Request["Deposit"]))
            {
                var data = Convert.ToDouble(Request["Deposit"].Trim());
                Expression<Func<Entities.Recharge, Boolean>> tmp = t => t.Deposit == data;
                expr = bulider.BuildQueryAnd(expr, tmp);
            }
            if (!string.IsNullOrEmpty(Request["Recharge_count"]))
            {
                var data = Convert.ToDouble(Request["Recharge_count"].Trim());
                Expression<Func<Entities.Recharge, Boolean>> tmp = t => t.Recharge_count == data;
                expr = bulider.BuildQueryAnd(expr, tmp);
            }
            if (!string.IsNullOrEmpty(Request["School_id"]))
            {
                var data = Convert.ToInt32(Request["School_id"].Trim());
                if (data > 0)
                {
                    Expression<Func<Entities.Recharge, Boolean>> tmp = t => t.User.School_id == data;
                    expr = bulider.BuildQueryAnd(expr, tmp);
                }
                else
                {
                    var sessionschoolids = Session["SchoolIds"] as List<int>;
                    if (sessionschoolids != null && sessionschoolids.Count > 0)
                    {
                        Expression<Func<Entities.Recharge, Boolean>> tmp = t => sessionschoolids.Contains((int)t.User.School_id);
                        expr = bulider.BuildQueryAnd(expr, tmp);
                    }
                }
            }
            else
            {
                var sessionschoolids = Session["SchoolIds"] as List<int>;
                if (sessionschoolids != null && sessionschoolids.Count > 0)
                {
                    Expression<Func<Entities.Recharge, Boolean>> tmp = t => sessionschoolids.Contains((int)t.User.School_id);
                    expr = bulider.BuildQueryAnd(expr, tmp);
                }
            }
            return expr;
        }
        #endregion
        #endregion
    }
}