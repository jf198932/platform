using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using Abp.Domain.Uow;
using Abp.Web.Models;
using isriding.Bikesite;
using isriding.School;
using isriding.Web.Extension.Fliter;
using isriding.Web.Models.Common;
using isriding.Web.Models.SchoolManage;

namespace isriding.Web.Controllers.SchoolManage
{
    public class SchooltrackController : isridingControllerBase
    {
        private readonly IBikesiteReadRepository _bikesiteReadRepository;
        private readonly ISchoolReadRepository _schoolReadRepository;
        private readonly ISqlReadExecuter _sqlReadExecuter;
        private readonly string sqlStr = @"CALL SP_SelectSchoolTrack";

        public SchooltrackController(IBikesiteReadRepository bikesiteReadRepository, ISchoolReadRepository schoolReadRepository, ISqlReadExecuter sqlReadExecuter)
        {
            _bikesiteReadRepository = bikesiteReadRepository;
            _schoolReadRepository = schoolReadRepository;
            _sqlReadExecuter = sqlReadExecuter;
        }

        // GET: Track
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }
        [AdminLayout]
        public ActionResult List()
        {
            var model = new BikesitemanageModel();
            PrepareTrackModel(model);
            return View(model);
        }

        [DontWrapResult, UnitOfWork]
        public virtual ActionResult InitDataTable(DataTableParameter param)
        {
            var expr = BuildSearchCriteria();

            var temp = _sqlReadExecuter.SqlQuery<SchooltrackModel>(sqlStr.ToString());
            if (expr != null)
            {
                temp = temp.Where(expr);
            }
            var query = temp.OrderBy(s => s.Id).Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var total = temp.Count();
            var filterResult = query.ToList();
            int sortId = param.iDisplayStart + 1;
            var result = from t in filterResult
                         select new[]
                             {
                                sortId++.ToString(),
                                t.SchoolName,
                                t.RentCount.ToString(),
                                t.ReadyCount.ToString(),
                                t.ErrorCount.ToString(),
                                t.ErrorCause,
                                t.SchoolId.ToString(),
                                t.Id.ToString()
                            };
            return DataTableJsonResult(param.sEcho, param.iDisplayStart, total, total, result);
        }

        [DontWrapResult, UnitOfWork]
        public virtual ActionResult GetSchooltrackInfo(DataTableParameter param, int id)
        {
            string sqlStr1 =
                @"select `aa`.`BikeId` AS `BikeId`,`aa`.`SchoolId` AS `SchoolId`,`aa`.`SchoolName` AS `SchoolName`,`aa`.`BikeName` AS `BikeName`,(case when (`aa`.`Status` in ('1','2','5')) then '异常' when (`aa`.`Status` = '3') then '待租中' when (`aa`.`Status` = '4') then '已租中' end) AS `Status`,(case when (`aa`.`Status` = '1') then '已租未还' when (`aa`.`Status` = '2') then '已还未付' else '' end) AS `ErrorCause`,`aa`.`StartTime` AS `StartTime`,`aa`.`EndTime` AS `EndTime`,`aa`.`StartSite` AS `StartSite`,`aa`.`EndSite` AS `EndSite`,`aa`.`UserName` AS `UserName` from (select `f`.`name` AS `UserName`,ifnull(date_format(`a`.`start_time`,'%Y-%m-%d %T'),'') AS `StartTime`,ifnull(date_format(`a`.`end_time`,'%Y-%m-%d %T'),'') AS `EndTime`,ifnull(`d`.`name`,'') AS `StartSite`,ifnull(`e`.`name`,'') AS `EndSite`,`a`.`bike_id` AS `BikeId`,`c`.`id` AS `SchoolId`,`c`.`name` AS `SchoolName`,`b`.`ble_name` AS `BikeName`,(case when ((`a`.`pay_status` = 1) and (now() > (`a`.`start_time` + interval (select `bms`.`parameter`.`parameter_value` from `bms`.`parameter` where (`bms`.`parameter`.`program` = 'ERROR_RENT_UNBACK_PERIOD')) hour))) then '1' when ((`a`.`pay_status` = 2) and (now() > (`a`.`end_time` + interval (select `bms`.`parameter`.`parameter_value` from `bms`.`parameter` where (`bms`.`parameter`.`program` = 'ERROR_BACK_UNPAY_PERIOD')) hour))) then '2' when (ifnull(`b`.`bike_status`,1) = 1) then '3' when (ifnull(`b`.`bike_status`,1) = 2) then '4' else '5' end) AS `Status` from (((((`bms`.`track` `a` join `bms`.`bike` `b` on((`b`.`id` = `a`.`bike_id`))) left join `bms`.`bikesite` `d` on((`d`.`id` = `a`.`start_site_id`))) left join `bms`.`bikesite` `e` on((`e`.`id` = `a`.`end_site_id`))) join `bms`.`user` `f` on((`f`.`id` = `a`.`user_id`))) join `bms`.`school` `c` on((`c`.`id` = `b`.`school_id`)))) `aa`
";
            var track = _sqlReadExecuter.SqlQuery<SchooltrackDetailModel>(sqlStr1)
                .Where(t => t.SchoolId == id)
                .Select(t => new SchooltrackDetailModel
                {
                    Id = t.BikeId,
                    SchoolName = t.SchoolName,
                    BikeName = t.BikeName,
                    Status = t.Status,
                    ErrorCause = t.ErrorCause,
                    StartTime = t.StartTime,
                    EndTime = t.EndTime,
                    StartSite = t.StartSite,
                    EndSite = t.EndSite,
                    UserName = t.UserName
                });
            var total = track.Count();
            int sortId = param.iDisplayStart + 1;
            var filterResult = track.ToList();
            var result = from t in filterResult
                         select new[]
                             {
                                sortId++.ToString(),
                                t.SchoolName,
                                t.BikeName,
                                t.UserName,
                                t.StartTime,
                                t.EndTime,
                                t.StartSite,
                                t.EndSite,
                                t.Status,
                                t.ErrorCause,
                                t.SchoolId.ToString(),
                                t.Id.ToString()
                            };
            //return PartialView(track);
            return DataTableJsonResult(param.sEcho, param.iDisplayStart, total, total, result);
        }

        #region 构建查询表达式
        /// <summary>
        /// 构建查询表达式
        /// </summary>
        /// <returns></returns>
        private Expression<Func<SchooltrackModel, Boolean>> BuildSearchCriteria()
        {
            DynamicLambda<SchooltrackModel> bulider = new DynamicLambda<SchooltrackModel>();
            Expression<Func<SchooltrackModel, Boolean>> expr = null;
            if (!string.IsNullOrEmpty(Request["School_id"]))
            {
                var data = Convert.ToInt32(Request["School_id"].Trim());
                if (data != 0)
                {
                    Expression<Func<SchooltrackModel, Boolean>> tmp = t => t.SchoolId == data;
                    expr = bulider.BuildQueryAnd(expr, tmp);
                }
                else
                {
                    var sessionschoolids = Session["SchoolIds"] as List<int>;
                    if (sessionschoolids != null && sessionschoolids.Count > 0)
                    {
                        Expression<Func<SchooltrackModel, Boolean>> tmp = t => sessionschoolids.Contains((int)t.SchoolId);
                        expr = bulider.BuildQueryAnd(expr, tmp);
                    }
                }
            }
            else
            {
                var sessionschoolids = Session["SchoolIds"] as List<int>;
                if (sessionschoolids != null && sessionschoolids.Count > 0)
                {
                    Expression<Func<SchooltrackModel, Boolean>> tmp = t => sessionschoolids.Contains((int)t.SchoolId);
                    expr = bulider.BuildQueryAnd(expr, tmp);
                }
            }
            return expr;
        }

        [NonAction, UnitOfWork]
        protected virtual void PrepareTrackModel(BikesitemanageModel model)
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