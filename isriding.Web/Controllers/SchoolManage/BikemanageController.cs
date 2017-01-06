using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Web.Models;
using isriding.Web.Models.Common;
using isriding.Web.Models.SchoolManage;

namespace isriding.Web.Controllers.SchoolManage
{
    public class BikemanageController : isridingControllerBase
    {
        private readonly IRepository<Entities.School> _schoolRepository;
        private readonly ISqlExecuter _sqlExecuter;
        private readonly string _sqlStr = @"CALL SP_SelectBikeManage";

        public BikemanageController(IRepository<Entities.School> schoolRepository, ISqlExecuter sqlExecuter)
        {
            _schoolRepository = schoolRepository;
            _sqlExecuter = sqlExecuter;
        }

        // GET: Track
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }
        //[AdminLayout]
        public ActionResult List()
        {
            var model = new BikemanageModel();
            PrepareBikemanageModel(model);
            return View(model);
        }

        [DontWrapResult, UnitOfWork]
        public virtual ActionResult InitDataTable(DataTableParameter param)
        {
            var expr = BuildSearchCriteria();
            //var temp = _bikesiteRepository.GetAll();
            var temp = _sqlExecuter.SqlQuery<BikemanageModel>(_sqlStr);
            if (expr != null)
            {
                temp = temp.Where(expr);
            }
            var query = temp.OrderByDescending(s => s.MaxRentTime).Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var total = temp.Count();
            var filterResult = query.ToList();
            int sortId = param.iDisplayStart + 1;
            var result = from t in filterResult
                         select new[]
                             {
                                sortId++.ToString(),
                                t.BleName,
                                t.MaxRentTime.ToString(),
                                t.BikeVer,
                                t.BleType,
                                t.StartTime.ToString(),
                                t.RentTimeCnt,
                                t.RentCnt,
                                t.Payment,
                                t.BikeStatus,
                                t.SchoolName,
                                t.SchoolId.ToString(),
                                t.BikeId.ToString()
                            };
            return DataTableJsonResult(param.sEcho, param.iDisplayStart, total, total, result);
        }

        [DontWrapResult, UnitOfWork]
        public virtual ActionResult GetBikemanageInfo(DataTableParameter param, int id)
        {
            //var track = _bikesiteRepository.GetAll()
            //    .Where(t => t.Id == id)
            //    .Select(t => new BikesitemanageModel
            //    {
            //        Id = t.Id,
            //        School = t.School.Name,
            //        Name = t.Name,
            //        Type = t.Type,
            //        Bike_count = t.Bike_count,
            //        Available_count = t.Available_count,
            //        Description = t.Description
            //    }).FirstOrDefault();
            //track.Type_name = TranslateType(track.Type);
            //return PartialView(track);
            StringBuilder sqlStr1 = new StringBuilder();
            sqlStr1.Append(
                "SELECT a.id as 'Id',a.pay_docno AS 'PayDocno',a.bike_id AS 'BikeId',b.ble_name AS 'BikeName',a.user_id AS 'UserId',c.`name` AS 'UserName',a.start_time as 'StartTime',a.end_time as 'EndTime',d.`name` AS 'StartSite',e.`name` AS 'EndSite',TIMESTAMPDIFF(MINUTE,IFNULL(a.start_time,NOW()),IFNULL(a.end_time,NOW())) AS 'RentTimeCnt',a.payment");
            sqlStr1.Append(" FROM track AS a");
            sqlStr1.Append(" INNER JOIN bike AS b ON b.id = a.bike_id");
            sqlStr1.Append(" INNER JOIN `user` AS c ON c.id = a.user_id");
            sqlStr1.Append(" LEFT JOIN bikesite AS d ON d.id = a.start_site_id");
            sqlStr1.Append(" LEFT JOIN bikesite AS e ON d.id = a.end_site_id");
            var track = _sqlExecuter.SqlQuery<BikemanageDetailModel>(sqlStr1.ToString()).Where(t=> t.BikeId == id);
            var total = track.Count();
            var temp = track.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();
            int sortId = param.iDisplayStart + 1;
            var filterResult = temp.ToList();
            var result = from t in filterResult
                         select new[]
                             {
                                sortId++.ToString(),
                                t.PayDocno,
                                t.BikeName,
                                t.UserName,
                                t.StartTime.ToString(),
                                t.EndTime.ToString(),
                                t.StartSite,
                                t.EndSite,
                                t.RentTimeCnt,
                                t.payment
                            };
            //return PartialView(track);
            return DataTableJsonResult(param.sEcho, param.iDisplayStart, total, total, result);
        }

        #region 构建查询表达式
        /// <summary>
        /// 构建查询表达式
        /// </summary>
        /// <returns></returns>
        private Expression<Func<BikemanageModel, Boolean>> BuildSearchCriteria()
        {
            DynamicLambda<BikemanageModel> bulider = new DynamicLambda<BikemanageModel>();
            Expression<Func<BikemanageModel, Boolean>> expr = null;
            if (!string.IsNullOrEmpty(Request["SchoolId"]))
            {
                var data = Convert.ToInt32(Request["SchoolId"].Trim());
                if (data != 0)
                {
                    Expression<Func<BikemanageModel, Boolean>> tmp = t => t.SchoolId == data;
                    expr = bulider.BuildQueryAnd(expr, tmp);
                }
                else
                {
                    var sessionschoolids = Session["SchoolIds"] as List<int>;
                    if (sessionschoolids != null && sessionschoolids.Count > 0)
                    {
                        Expression<Func<BikemanageModel, Boolean>> tmp = t => sessionschoolids.Contains((int)t.SchoolId);
                        expr = bulider.BuildQueryAnd(expr, tmp);
                    }
                }
            }
            else
            {
                var sessionschoolids = Session["SchoolIds"] as List<int>;
                if (sessionschoolids != null && sessionschoolids.Count > 0)
                {
                    Expression<Func<BikemanageModel, Boolean>> tmp = t => sessionschoolids.Contains((int)t.SchoolId);
                    expr = bulider.BuildQueryAnd(expr, tmp);
                }
            }

            if (!string.IsNullOrEmpty(Request["Ble_name"]))
            {
                var data = Request["Ble_name"].Trim();
                Expression<Func<BikemanageModel, Boolean>> tmp = t => t.BleName.Contains(data);
                expr = bulider.BuildQueryAnd(expr, tmp);
            }

            if (!string.IsNullOrEmpty(Request["Bstatus"]))
            {
                var data = Request["Bstatus"].Trim();
                Expression<Func<BikemanageModel, Boolean>> tmp = t => t.BikeStatus == data;
                expr = bulider.BuildQueryAnd(expr, tmp);
            }
            return expr;
        }

        [NonAction, UnitOfWork]
        protected virtual void PrepareBikemanageModel(BikemanageModel model)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            var list = _schoolRepository.GetAll();
            //var listBikesite = _bikesiteRepository.GetAll();

            var sessionschoolids = Session["SchoolIds"] as List<int>;
            if (sessionschoolids != null && sessionschoolids.Count > 0)
            {
                list = list.Where(t => sessionschoolids.Contains(t.Id));
            }
            var schoollist = list.Select(b => new SelectListItem { Text = b.Name, Value = b.Id.ToString() });
            model.Search.SchoolList.AddRange(schoollist);
            model.Search.SchoolList.Insert(0, new SelectListItem { Text = "---请选择---", Value = "0" });
            //model.Search.BikesiteList.Insert(0, new SelectListItem { Text = "---请选择---", Value = "0" });
            //ViewData["Bikesite"] = JsonConvert.SerializeObject(listBikesite.ToList());
        }
        #endregion
    }
}