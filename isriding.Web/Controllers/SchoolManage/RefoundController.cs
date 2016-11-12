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
using isriding.Refound;
using isriding.School;
using isriding.Web.Models.Common;
using isriding.Web.Models.SchoolManage;

namespace isriding.Web.Controllers.SchoolManage
{
    public class RefoundController : isridingControllerBase
    {
        private readonly IRefoundReadRepository _refoundReadRepository;
        private readonly IRefoundWriteRepository _refoundWriteRepository;
        private readonly IRecharge_detailReadRepository _rechargeDetailReadRepository;
        private readonly ISchoolReadRepository _schoolReadRepository;
        private readonly IRechargeWriteRepository _rechargeWriteRepository;

        public RefoundController(IRefoundReadRepository refoundReadRepository
            , IRefoundWriteRepository refoundWriteRepository
            , IRecharge_detailReadRepository rechargeDetailReadRepository
            , ISchoolReadRepository schoolReadRepository
            , IRechargeWriteRepository rechargeWriteRepository)
        {
            _rechargeDetailReadRepository = rechargeDetailReadRepository;
            _schoolReadRepository = schoolReadRepository;
            _refoundReadRepository = refoundReadRepository;
            _refoundWriteRepository = refoundWriteRepository;
            _rechargeWriteRepository = rechargeWriteRepository;
        }

        // GET: Refound
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            var model = new RefoundModel();
            PrepareAllUserModel(model);
            return View(model);
        }

        [DontWrapResult, UnitOfWork]
        public virtual ActionResult InitDataTable(DataTableParameter param)
        {
            var expr = BuildSearchCriteria();
            var temp = _refoundReadRepository.GetAll();
            if (expr != null)
            {
                temp = temp.Where(expr);
            }
            var query = temp.OrderBy(s => s.Id).Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var total = temp.Count();
            var filterResult = query.Select(t => new RefoundModel
            {
                Id = t.Id,
                Refound_amount = t.Refound_amount,
                Created_at = t.Created_at,
                Refound_status = t.Refound_status,
                Updated_at = t.Updated_at,
                User_id = t.User_id,
                User_name = t.User == null ? "" : t.User.Name,
                School_name = t.User.School.Name
            }).ToList();
            int sortId = param.iDisplayStart + 1;
            var result = from t in filterResult
                         select new[]
                             {
                                sortId++.ToString(),
                                t.School_name,
                                t.User_name,
                                t.Refound_amount.ToString(),
                                t.Refound_status.ToString(),
                                t.Id.ToString()
                            };

            return DataTableJsonResult(param.sEcho, param.iDisplayStart, total, total, result);
        }

        [UnitOfWork]
        public virtual ActionResult Edit(int id)
        {
            Mapper.Initialize(t => t.CreateMap<Entities.Refound, RefoundModel>());
            var model = Mapper.Map<RefoundModel>(_refoundReadRepository.Get(id));
            //var model = role.ToModel();
            PrepareAllUserModel(model);
            return PartialView(model);
        }

        [HttpPost, UnitOfWork]
        public virtual ActionResult Edit(RefoundModel model)
        {
            var user = _refoundWriteRepository.Get(model.Id);

            if (ModelState.IsValid)
            {
                user.Refound_status = model.Refound_status;
                user.Updated_at = DateTime.Now;

                _refoundWriteRepository.Update(user);

                if (model.Refound_status == 4)//成功退款
                {
                    var recharge = _rechargeWriteRepository.FirstOrDefault(t => t.User_id == user.User_id);
                    recharge.Deposit = 0;
                    _rechargeWriteRepository.Update(recharge);
                }

                return Json(model);
            }
            return Json(null);
        }

        [HttpPost, UnitOfWork]
        public virtual ActionResult Delete(int id)
        {
            _refoundWriteRepository.Delete(s => s.Id == id);
            //var role = _roleService.GetRoleById(id);
            //_roleService.DeleteRole(role);

            return Json(new { success = true });
        }

        #region 公共方法
        [NonAction, UnitOfWork]
        protected virtual void PrepareAllUserModel(RefoundModel model)
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
        private Expression<Func<Entities.Refound, Boolean>> BuildSearchCriteria()
        {
            DynamicLambda<Entities.Refound> bulider = new DynamicLambda<Entities.Refound>();
            Expression<Func<Entities.Refound, Boolean>> expr = null;
            if (!string.IsNullOrEmpty(Request["User_name"]))
            {
                var data = Request["User_name"].Trim();
                Expression<Func<Entities.Refound, Boolean>> tmp = t => t.User.Name.Contains(data);
                expr = bulider.BuildQueryAnd(expr, tmp);
            }
            if (!string.IsNullOrEmpty(Request["Refound_amount"]))
            {
                var data = Convert.ToDouble(Request["Refound_amount"].Trim());
                Expression<Func<Entities.Refound, Boolean>> tmp = t => t.Refound_amount == data;
                expr = bulider.BuildQueryAnd(expr, tmp);
            }
            if (!string.IsNullOrEmpty(Request["Refound_status"]) && Request["Refound_status"] != "0")
            {
                var data = Convert.ToInt32(Request["Refound_status"].Trim());
                Expression<Func<Entities.Refound, Boolean>> tmp = t => t.Refound_status == data;
                expr = bulider.BuildQueryAnd(expr, tmp);
            }
            if (!string.IsNullOrEmpty(Request["School_id"]))
            {
                var data = Convert.ToInt32(Request["School_id"].Trim());
                if (data > 0)
                {
                    Expression<Func<Entities.Refound, Boolean>> tmp = t => t.User.School_id == data;
                    expr = bulider.BuildQueryAnd(expr, tmp);
                }
                else
                {
                    var sessionschoolids = Session["SchoolIds"] as List<int>;
                    if (sessionschoolids != null && sessionschoolids.Count > 0)
                    {
                        Expression<Func<Entities.Refound, Boolean>> tmp = t => sessionschoolids.Contains((int)t.User.School_id);
                        expr = bulider.BuildQueryAnd(expr, tmp);
                    }
                }
            }
            else
            {
                var sessionschoolids = Session["SchoolIds"] as List<int>;
                if (sessionschoolids != null && sessionschoolids.Count > 0)
                {
                    Expression<Func<Entities.Refound, Boolean>> tmp = t => sessionschoolids.Contains((int)t.User.School_id);
                    expr = bulider.BuildQueryAnd(expr, tmp);
                }
            }
            return expr;
        }
        #endregion
        #endregion
    }
}