using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Runtime.Caching;
using Abp.Web.Models;
using isriding;
using isriding.Web;
using isriding.Web.Controllers;
using isriding.Web.Extension.Fliter;
using isriding.Web.Helper;
using isriding.Web.Models.Common;
using isriding.Web.Models.School;

namespace isriding.Web.Controllers.School
{
    public class AlarmController : isridingControllerBase
    {
        private readonly IRepository<Entities.Bike> _bikeRepository;
        private readonly ISqlExecuter _sqlExecuter;
        private readonly ICacheManager _cacheManager;

        public AlarmController(IRepository<Entities.Bike> bikeRepository, ISqlExecuter sqlExecuter, ICacheManager cacheManager)
        {
            _bikeRepository = bikeRepository;
            _sqlExecuter = sqlExecuter;
            _cacheManager = cacheManager;
        }
        // GET: Alarm
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }
        [AdminLayout]
        public ActionResult List()
        {
            BikeModel model = new BikeModel();
            return View(model);
        }

        [DontWrapResult, UnitOfWork]
        public virtual ActionResult InitDataTable(DataTableParameter param)
        {
            var expr = BuildSearchCriteria();
            var temp = _bikeRepository.GetAll();
            if (expr != null)
            {
                temp = temp.Where(expr);
            }


            StringBuilder sqlstr = new StringBuilder();
            sqlstr.Append(
                "select b.id,b.ble_name,b.ble_serial,b.ble_type,b.vlock_status,b.bike_img,s.`name` as alarmbikesitename,l.op_time as alarmtime,u.`name` as user_name,u.phone");
            sqlstr.Append(" from bike as b LEFT JOIN log as l ON b.id = l.bike_id LEFT JOIN bikesite as s on l.bikesite_id = s.id LEFT JOIN `user` as u on b.user_id = u.id");
            sqlstr.Append(" where b.vlock_status = 5");
            
            if (!string.IsNullOrEmpty(Request["Ble_name"]))
            {
                var data = Request["Ble_name"].Trim();
                sqlstr.AppendFormat(" and b.ble_name like '%{0}%'", data);
            }
            if (!string.IsNullOrEmpty(Request["Ble_type"]) && Request["Ble_type"].Trim() != "0")
            {
                var data = Convert.ToInt32(Request["Ble_type"].Trim());
                sqlstr.AppendFormat(" and b.ble_type={0}", data);
            }

            var sessionschoolids = Session["SchoolIds"] as List<int>;
            if (sessionschoolids != null && sessionschoolids.Count > 0)
            {
                sqlstr.Append(" and b.school_id in(");
                foreach (var sessionschoolid in sessionschoolids)
                {
                    if (sessionschoolids.IndexOf(sessionschoolid) == sessionschoolids.Count - 1)
                    {
                        sqlstr.AppendFormat("{0}", sessionschoolid);
                    }
                    else
                    {
                        sqlstr.AppendFormat("{0},", sessionschoolid);
                    }
                }
                sqlstr.Append(")");
            }


            sqlstr.Append(" GROUP BY b.id,b.ble_name,b.vlock_status order by l.op_time DESC");

            sqlstr.AppendFormat(" limit {0}, {1}" ,param.iDisplayStart ,param.iDisplayLength);

            
            var total = temp.Count();
            
            var filterResult = _sqlExecuter.SqlQuery<BikeModel>(sqlstr.ToString()).ToList();

            int sortId = param.iDisplayStart + 1;
            var result = from t in filterResult
                         select new[]
                             {
                                sortId++.ToString(),
                                t.Ble_name,
                                t.Ble_type.ToString(),
                                t.Vlock_status.ToString(),
                                t.AlarmTime.ToString(),
                                t.AlarmBikesiteName,
                                t.User_name,
                                t.Phone,
                                t.Bike_img,
                                t.Id.ToString()
                            };
            
            return DataTableJsonResult(param.sEcho, param.iDisplayStart, total, total, result);
        }

        #region 构建查询表达式
        /// <summary>
        /// 构建查询表达式
        /// </summary>
        /// <returns></returns>
        private Expression<Func<Entities.Bike, Boolean>> BuildSearchCriteria()
        {
            DynamicLambda<Entities.Bike> bulider = new DynamicLambda<Entities.Bike>();
            Expression<Func<Entities.Bike, Boolean>> expr = null;
            if (!string.IsNullOrEmpty(Request["Ble_serial"]))
            {
                var data = Request["Ble_serial"].Trim();
                Expression<Func<Entities.Bike, Boolean>> tmp = t => t.Ble_serial.Contains(data);
                expr = bulider.BuildQueryAnd(expr, tmp);
            }
            if (!string.IsNullOrEmpty(Request["Ble_name"]))
            {
                var data = Request["Ble_name"].Trim();
                Expression<Func<Entities.Bike, Boolean>> tmp = t => t.Ble_name.Contains(data);
                expr = bulider.BuildQueryAnd(expr, tmp);
            }
            if (!string.IsNullOrEmpty(Request["Ble_type"]) && Request["Ble_type"].Trim() != "0")
            {
                var data = Convert.ToInt32(Request["Ble_type"].Trim());
                Expression<Func<Entities.Bike, Boolean>> tmp = t => t.Ble_type >= data;
                expr = bulider.BuildQueryAnd(expr, tmp);
            }

            Expression<Func<Entities.Bike, Boolean>> tmpd = t => t.Vlock_status == 5;
            expr = bulider.BuildQueryAnd(expr, tmpd);

            //var id = CommonHelper.GetSchoolId();
            //if (id > 1)
            //{
            //    Expression<Func<Entities.Bike, Boolean>> tmpSolid = t => t.School_id == id;
            //    expr = bulider.BuildQueryAnd(expr, tmpSolid);
            //}

            return expr;
        }

        #endregion
    }
}