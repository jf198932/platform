using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using Abp.Domain.Uow;
using Abp.Web.Models;
using AutoMapper;
using isriding.School;
using isriding.TbTroubleFeedback;
using isriding.Web.Extension.Fliter;
using isriding.Web.Models.Common;
using isriding.Web.Models.SchoolManage;

namespace isriding.Web.Controllers.SchoolManage
{
    public class TroubleController : isridingControllerBase
    {

        private readonly ITbTroubleFeedbackReadRepository _tbTroubleFeedbackReadRepository;
        private readonly ITbTroubleFeedbackWriteRepository _tbTroubleFeedbackWriteRepository;
        private readonly ISchoolReadRepository _schoolReadRepository;

        public TroubleController(ITbTroubleFeedbackReadRepository tbTroubleFeedbackReadRepository
            , ITbTroubleFeedbackWriteRepository tbTroubleFeedbackWriteRepository
            , ISchoolReadRepository schoolReadRepository)
        {
            _tbTroubleFeedbackReadRepository = tbTroubleFeedbackReadRepository;
            _tbTroubleFeedbackWriteRepository = tbTroubleFeedbackWriteRepository;
            _schoolReadRepository = schoolReadRepository;
        }

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }
        [AdminLayout]
        //[AdminPermission(PermissionCustomMode.Enforce)]
        public ActionResult List()
        {
            var model = new TroubleModel();
            PrepareAllUserModel(model);
            return View(model);
        }

        [DontWrapResult, UnitOfWork]
        public virtual ActionResult InitDataTable(DataTableParameter param)
        {
            var expr = BuildSearchCriteria();
            var temp = _tbTroubleFeedbackReadRepository.GetAll();
            if (expr != null)
            {
                temp = temp.Where(expr);
            }
            var query = temp.OrderBy(s => s.Id).Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var total = temp.Count();
            var filterResult = query.Select(t => new TroubleModel
            {
                Id = t.Id,
                create_by = t.create_by,
                update_by = t.update_by,
                bike_number = t.bike_number,
                trouble1 = t.trouble1,
                trouble2 = t.trouble2,
                trouble3 = t.trouble3,
                comments = t.comments,
                img_url = t.img_url,
                verify_status = t.verify_status,
                deal_status = t.deal_status,
                create_time = t.create_time,
                update_time = t.update_time,
                schoolname = t.User.School.Name,
                username = t.User.Name,
                phone = t.User.Phone
            }).ToList();
            int sortId = param.iDisplayStart + 1;
            var result = from t in filterResult
                         select new[]
                             {
                                sortId++.ToString(),
                                t.Id.ToString(),
                                t.create_time.ToString(),
                                t.username,
                                t.schoolname,
                                t.phone,
                                t.bike_number,
                                t.trouble1,
                                t.trouble2,
                                t.trouble3,
                                t.comments,
                                t.img_url,
                                t.verify_status.ToString(),
                                t.deal_status.ToString(),
                                t.Id.ToString()
                            };

            return DataTableJsonResult(param.sEcho, param.iDisplayStart, total, total, result);
        }

        //public ActionResult Create()
        //{
        //    var model = new TroubleModel();
        //    PrepareAllUserModel(model);
        //    return PartialView(model);
        //}

        //[HttpPost, UnitOfWork]
        //public virtual ActionResult Create(TroubleModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        Mapper.Initialize(t => t.CreateMap<TroubleModel, Entities.Tb_trouble_feedback>());
        //        var user = Mapper.Map<Entities.Tb_trouble_feedback>(model);
        //        user = _tbTroubleFeedbackWriteRepository.Insert(user);

        //        //SuccessNotification("添加成功");
        //        return Json(model);
        //    }
        //    return Json(null);
        //}
        [UnitOfWork]
        public virtual ActionResult Edit(int id)
        {
            Mapper.Initialize(t => t.CreateMap<Entities.Tb_trouble_feedback, TroubleModel>());
            var model = Mapper.Map<TroubleModel>(_tbTroubleFeedbackReadRepository.Get(id));
            //var model = role.ToModel();
            PrepareAllUserModel(model);
            return PartialView(model);
        }

        [HttpPost, UnitOfWork]
        public virtual ActionResult Edit(TroubleModel model)
        {
            var trouble = _tbTroubleFeedbackWriteRepository.Get(model.Id);

            if (ModelState.IsValid)
            {
                trouble.verify_status = model.verify_status;
                trouble.deal_status = model.deal_status;

                _tbTroubleFeedbackWriteRepository.Update(trouble);
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
            _tbTroubleFeedbackWriteRepository.Delete(s => s.Id == id);
            //var role = _roleService.GetRoleById(id);
            //_roleService.DeleteRole(role);

            return Json(new { success = true });
        }

        #region 公共方法
        [NonAction, UnitOfWork]
        protected virtual void PrepareAllUserModel(TroubleModel model)
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
        private Expression<Func<Entities.Tb_trouble_feedback, Boolean>> BuildSearchCriteria()
        {
            DynamicLambda<Entities.Tb_trouble_feedback> bulider = new DynamicLambda<Entities.Tb_trouble_feedback>();
            Expression<Func<Entities.Tb_trouble_feedback, Boolean>> expr = null;
            if (!string.IsNullOrEmpty(Request["verify_status"]) && Request["verify_status"].Trim() != "0")
            {
                var data = Convert.ToInt32(Request["verify_status"].Trim());
                Expression<Func<Entities.Tb_trouble_feedback, Boolean>> tmp = t => t.verify_status == data;
                expr = bulider.BuildQueryAnd(expr, tmp);
            }
            if (!string.IsNullOrEmpty(Request["deal_status"]) && Request["deal_status"].Trim() != "0")
            {
                var data = Convert.ToInt32(Request["deal_status"].Trim());
                Expression<Func<Entities.Tb_trouble_feedback, Boolean>> tmp = t => t.deal_status == data;
                expr = bulider.BuildQueryAnd(expr, tmp);
            }
            if (!string.IsNullOrEmpty(Request["StartDate"]))
            {
                var data = DateTime.Parse(Request["StartDate"].Trim());
                Expression<Func<Entities.Tb_trouble_feedback, Boolean>> tmp = t => t.create_time.Value >= data;
                expr = bulider.BuildQueryAnd(expr, tmp);
            }
            if (!string.IsNullOrEmpty(Request["EndDate"]))
            {
                var data = DateTime.Parse(Request["EndDate"].Trim());
                Expression<Func<Entities.Tb_trouble_feedback, Boolean>> tmp = t => t.create_time.Value <= data;
                expr = bulider.BuildQueryAnd(expr, tmp);
            }
            if (!string.IsNullOrEmpty(Request["School_id"]))
            {
                var data = Convert.ToInt32(Request["School_id"].Trim());
                if (data > 0)
                {
                    Expression<Func<Entities.Tb_trouble_feedback, Boolean>> tmp = t => t.User.School_id == data;
                    expr = bulider.BuildQueryAnd(expr, tmp);
                }
                else
                {
                    var sessionschoolids = Session["SchoolIds"] as List<int>;
                    if (sessionschoolids != null && sessionschoolids.Count > 0)
                    {
                        Expression<Func<Entities.Tb_trouble_feedback, Boolean>> tmp = t => sessionschoolids.Contains((int)t.User.School_id);
                        expr = bulider.BuildQueryAnd(expr, tmp);
                    }
                }
            }
            else
            {
                var sessionschoolids = Session["SchoolIds"] as List<int>;
                if (sessionschoolids != null && sessionschoolids.Count > 0)
                {
                    Expression<Func<Entities.Tb_trouble_feedback, Boolean>> tmp = t => sessionschoolids.Contains((int)t.User.School_id);
                    expr = bulider.BuildQueryAnd(expr, tmp);
                }
            }
            //var id = CommonHelper.GetSchoolId();
            //if (id > 1)
            //{
            //    Expression<Func<Entities.User, Boolean>> tmpSolid = t => t.School_id == id;
            //    expr = bulider.BuildQueryAnd(expr, tmpSolid);
            //}
            return expr;
        }
        #endregion
        #endregion
    }
}