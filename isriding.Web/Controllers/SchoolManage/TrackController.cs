using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Web.Models;
using isriding.Entities;
using isriding.Web.Models.Common;
using isriding.Web.Models.SchoolManage;

namespace isriding.Web.Controllers.SchoolManage
{
    public class TrackController : isridingControllerBase
    {
        private readonly IRepository<Track> _trackRepository;

        public TrackController(IRepository<Track> trackRepository)
        {
            _trackRepository = trackRepository;
        }

        // GET: Track
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            var model = new TrackModel();
            return View(model);
        }

        [DontWrapResult, UnitOfWork]
        public virtual ActionResult InitDataTable(DataTableParameter param)
        {
            var expr = BuildSearchCriteria();
            var temp = _trackRepository.GetAll();
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
            }).ToList();
            int sortId = param.iDisplayStart + 1;
            var result = from t in filterResult
                         select new[]
                             {
                                sortId++.ToString(),
                                t.Pay_docno,
                                t.User_Name,
                                t.Ble_name,
                                t.Start_site_name,
                                t.Start_time.ToString(),
                                t.End_site_name,
                                t.End_time.ToString(),
                                t.Pay_status.ToString(),
                                t.Pay_method,
                                t.Payment.ToString(),
                                t.Id.ToString()
                            };
            return DataTableJsonResult(param.sEcho, param.iDisplayStart, total, total, result);
        }

        [DontWrapResult, UnitOfWork]
        public virtual ActionResult GetTrackInfo(int id)
        {
            var track = _trackRepository.GetAll()
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
            if (!string.IsNullOrEmpty(Request["User_Name"]))
            {
                var data = Request["User_Name"].Trim();
                Expression<Func<Entities.Track, Boolean>> tmp = t => t.User.Name.Contains(data);
                expr = bulider.BuildQueryAnd(expr, tmp);
            }
            if (!string.IsNullOrEmpty(Request["Ble_name"]))
            {
                var data = Request["Ble_name"].Trim();
                Expression<Func<Entities.Track, Boolean>> tmp = t => t.Bike.Ble_name.Contains(data);
                expr = bulider.BuildQueryAnd(expr, tmp);
            }
            if (!string.IsNullOrEmpty(Request["Pay_status"]))
            {
                var data = Convert.ToInt32(Request["Pay_status"].Trim());
                Expression<Func<Entities.Track, Boolean>> tmp = t => t.Pay_status == data;
                expr = bulider.BuildQueryAnd(expr, tmp);
            }
            if (!string.IsNullOrEmpty(Request["Start_site_name"]))
            {
                var data = Request["Start_site_name"].Trim();
                Expression<Func<Entities.Track, Boolean>> tmp = t => t.Bikesitestart.Name.Contains(data);
                expr = bulider.BuildQueryAnd(expr, tmp);
            }
            if (!string.IsNullOrEmpty(Request["End_site_name"]))
            {
                var data = Request["End_site_name"].Trim();
                Expression<Func<Entities.Track, Boolean>> tmp = t => t.Bikesiteend.Name.Contains(data);
                expr = bulider.BuildQueryAnd(expr, tmp);
            }
            
            var sessionschoolids = Session["SchoolIds"] as List<int>;
            if (sessionschoolids != null && sessionschoolids.Count > 0)
            {
                Expression<Func<Entities.Track, Boolean>> tmp = t => sessionschoolids.Contains((int)t.Bike.School_id);
                expr = bulider.BuildQueryAnd(expr, tmp);
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
    }
}