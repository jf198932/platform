﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using Abp.Domain.Uow;
using Abp.Web.Models;
using isriding.School;
using isriding.Track;
using isriding.Web.Extension.Fliter;
using isriding.Web.Models.Common;
using isriding.Web.Models.SchoolManage;

namespace isriding.Web.Controllers.SchoolManage
{
    public class TrackDetailsController : isridingControllerBase
    {
        private readonly ITrackWriteRepository _trackRepository;
        private readonly ITrackReadRepository _trackReadRepository;
        private readonly ISchoolReadRepository _schoolReadRepository;

        public TrackDetailsController(ITrackWriteRepository trackRepository
            , ITrackReadRepository trackReadRepository
            , ISchoolReadRepository schoolReadRepository)
        {
            _trackRepository = trackRepository;
            _trackReadRepository = trackReadRepository;
            _schoolReadRepository = schoolReadRepository;
        }

        // GET: Track
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }
        [AdminLayout]
        public ActionResult List()
        {
            var model = new TrackModel();
            PrepareTrackModel(model);
            return View(model);
        }

        [DontWrapResult, UnitOfWork]
        public virtual ActionResult InitDataTable(DataTableParameter param)
        {
            var expr = BuildSearchCriteria();
            var temp = _trackReadRepository.GetAll();
            if (expr != null)
            {
                temp = temp.Where(expr);
            }
            var query = temp.OrderBy(s => s.Id).Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var total = temp.Count();
            var filterResult = query.Select(t => new TrackModel
            {
                Id = t.Id,
                User_id = t.User_id,
                User_Name = t.User.Name,
                Pay_status = t.Pay_status,
                Pay_docno = t.Pay_docno,
                Pay_method = t.Pay_method,
                Pay_time = t.Pay_time,
                Remark = t.Remark,
                Trade_no = t.Trade_no,
                Payment = t.Payment,
                School_name = t.Bike.School.Name
            }).ToList();
            int sortId = param.iDisplayStart + 1;
            var result = from t in filterResult
                         select new[]
                             {
                                sortId++.ToString(),
                                t.Trade_no,
                                t.School_name,
                                t.User_Name,
                                t.Pay_status.ToString(),
                                t.Pay_method,
                                t.Payment.ToString(),
                                t.Pay_time.ToString(),
                                t.Id.ToString()
                            };
            return DataTableJsonResult(param.sEcho, param.iDisplayStart, total, total, result);
        }

        [DontWrapResult, UnitOfWork]
        public virtual ActionResult GetTrackInfo(int id)
        {
            var track = _trackReadRepository.GetAll()
                .Where(t => t.Id == id)
                .Select(t => new TrackModel
                {
                    Id = t.Id,
                    User_id = t.User_id,
                    User_Name = t.User.Name,
                    Bike_id = t.Bike_id,
                    Ble_name = t.Bike.Ble_name,
                    Start_site_name = t.Bikesitestart.Name,
                    End_site_name = t.Bikesiteend.Name,
                    Start_site_id = t.Start_site_id,
                    Start_point = t.Start_point,
                    Start_time = t.Start_time,
                    End_point = t.End_point,
                    End_site_id = t.End_site_id,
                    End_time = t.End_time,
                    Pay_status = t.Pay_status,
                    Pay_docno = t.Pay_docno,
                    Pay_method = t.Pay_method,
                    Remark = t.Remark,
                    Trade_no = t.Trade_no,
                    Pay_time = t.Pay_time
                }).FirstOrDefault();
            return PartialView(track);
        }

        #region 构建查询表达式
        /// <summary>
        /// 构建查询表达式
        /// </summary>
        /// <returns></returns>
        private Expression<Func<Entities.Track, Boolean>> BuildSearchCriteria()
        {
            DynamicLambda<Entities.Track> bulider = new DynamicLambda<Entities.Track>();
            Expression<Func<Entities.Track, Boolean>> expr = null;
            Expression<Func<Entities.Track, Boolean>> tm = t => t.Start_site_id != null;
            expr = bulider.BuildQueryAnd(expr, tm);

            if (!string.IsNullOrEmpty(Request["User_Name"]))
            {
                var data = Request["User_Name"].Trim();
                Expression<Func<Entities.Track, Boolean>> tmp = t => t.User.Name.Contains(data);
                expr = bulider.BuildQueryAnd(expr, tmp);
            }
            
            if (!string.IsNullOrEmpty(Request["Pay_status"]) && Convert.ToInt32(Request["Pay_status"].Trim()) > 0)
            {
                var data = Convert.ToInt32(Request["Pay_status"].Trim());
                Expression<Func<Entities.Track, Boolean>> tmp = t => t.Pay_status == data;
                expr = bulider.BuildQueryAnd(expr, tmp);
            }

            if (!string.IsNullOrEmpty(Request["StartDate"]))
            {
                var data = DateTime.Parse(Request["StartDate"].Trim());
                Expression<Func<Entities.Track, Boolean>> tmp = t => t.Pay_time >= data;
                expr = bulider.BuildQueryAnd(expr, tmp);
            }
            if (!string.IsNullOrEmpty(Request["EndDate"]))
            {
                var data = DateTime.Parse(Request["EndDate"].Trim());
                Expression<Func<Entities.Track, Boolean>> tmp = t => t.Pay_time <= data;
                expr = bulider.BuildQueryAnd(expr, tmp);
            }

            if (!string.IsNullOrEmpty(Request["School_id"]))
            {
                var data = Convert.ToInt32(Request["School_id"].Trim());
                if (data > 0)
                {
                    Expression<Func<Entities.Track, Boolean>> tmp = t => t.Bike.School_id == data;
                    expr = bulider.BuildQueryAnd(expr, tmp);
                }
                else
                {
                    var sessionschoolids = Session["SchoolIds"] as List<int>;
                    if (sessionschoolids != null && sessionschoolids.Count > 0)
                    {
                        Expression<Func<Entities.Track, Boolean>> tmp = t => sessionschoolids.Contains((int)t.Bike.School_id);
                        expr = bulider.BuildQueryAnd(expr, tmp);
                    }
                }
            }
            else
            {
                var sessionschoolids = Session["SchoolIds"] as List<int>;
                if (sessionschoolids != null && sessionschoolids.Count > 0)
                {
                    Expression<Func<Entities.Track, Boolean>> tmp = t => sessionschoolids.Contains((int)t.Bike.School_id);
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

        [NonAction, UnitOfWork]
        protected virtual void PrepareTrackModel(TrackModel model)
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
        #endregion
    }
}