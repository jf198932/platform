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
using Newtonsoft.Json;

namespace isriding.Web.Controllers.SchoolManage
{
    public class BikesitemanageController : isridingControllerBase
    {
        private readonly IBikesiteWriteRepository _bikesiteRepository;
        private readonly IBikesiteReadRepository _bikesiteReadRepository;
        private readonly ISchoolReadRepository _schoolReadRepository;

        public BikesitemanageController(IBikesiteWriteRepository bikesiteRepository
            , IBikesiteReadRepository bikesiteReadRepository
            , ISchoolReadRepository schoolReadRepository)
        {
            _bikesiteRepository = bikesiteRepository;
            _bikesiteReadRepository = bikesiteReadRepository;
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
            var model = new BikesitemanageModel();
            PrepareTrackModel(model);
            return View(model);
        }

        [DontWrapResult, UnitOfWork]
        public virtual ActionResult InitDataTable(DataTableParameter param)
        {
            var expr = BuildSearchCriteria();
            var temp = _bikesiteReadRepository.GetAll();
            if (expr != null)
            {
                temp = temp.Where(expr);
            }
            var query = temp.OrderBy(s => s.Id).Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var total = temp.Count();
            var filterResult = query.Select(t => new BikesitemanageModel
            {
                Id = t.Id,
                School = t.School.Name,
                Name = t.Name,
                Type = t.Type,
                Bike_count = t.Bike_count,
                Available_count = t.Available_count
            }).ToList();
            int sortId = param.iDisplayStart + 1;
            var result = from t in filterResult
                         select new[]
                             {
                                sortId++.ToString(),
                                t.School,
                                t.Name,
                                t.Type.ToString(),
                                t.Bike_count.ToString(),
                                t.Available_count.ToString(),
                                t.Id.ToString()
                            };
            return DataTableJsonResult(param.sEcho, param.iDisplayStart, total, total, result);
        }

        [DontWrapResult, UnitOfWork]
        public virtual ActionResult GetBikesitemanageInfo(int id)
        {
            var track = _bikesiteReadRepository.GetAll()
                .Where(t => t.Id == id)
                .Select(t => new BikesitemanageModel
                {
                    Id = t.Id,
                    School = t.School.Name,
                    Name = t.Name,
                    Type = t.Type,
                    Bike_count = t.Bike_count,
                    Available_count = t.Available_count,
                    Description = t.Description
                }).FirstOrDefault();
            track.Type_name = TranslateType(track.Type);
            return PartialView(track);
        }

        private string TranslateType(int? type)
        {
            string typeResult = string.Empty;
            switch (type)
            {
                case 1:
                    typeResult = "普通";
                    break;
                case 2:
                    typeResult = "防盗";
                    break;
                case 3:
                    typeResult = "租车";
                    break;
                default:
                    typeResult = "异常";
                    break;
            }
            return typeResult;
        }

        //private string BindBikesite()
        //{

        //}

        #region 构建查询表达式
        /// <summary>
        /// 构建查询表达式
        /// </summary>
        /// <returns></returns>
        private Expression<Func<Entities.Bikesite, Boolean>> BuildSearchCriteria()
        {
            DynamicLambda<Entities.Bikesite> bulider = new DynamicLambda<Entities.Bikesite>();
            Expression<Func<Entities.Bikesite, Boolean>> expr = null;
            if (!string.IsNullOrEmpty(Request["School_id"]))
            {
                var data = Convert.ToInt32(Request["School_id"].Trim());
                if (data != 0)
                {
                    Expression<Func<Entities.Bikesite, Boolean>> tmp = t => t.School_id == data;
                    expr = bulider.BuildQueryAnd(expr, tmp);
                }
                else
                {
                    var sessionschoolids = Session["SchoolIds"] as List<int>;
                    if (sessionschoolids != null && sessionschoolids.Count > 0)
                    {
                        Expression<Func<Entities.Bikesite, Boolean>> tmp = t => sessionschoolids.Contains((int)t.School_id);
                        expr = bulider.BuildQueryAnd(expr, tmp);
                    }
                }
            }
            else
            {
                var sessionschoolids = Session["SchoolIds"] as List<int>;
                if (sessionschoolids != null && sessionschoolids.Count > 0)
                {
                    Expression<Func<Entities.Bikesite, Boolean>> tmp = t => sessionschoolids.Contains((int)t.School_id);
                    expr = bulider.BuildQueryAnd(expr, tmp);
                }
            }
            if (!string.IsNullOrEmpty(Request["Name"]))
            {
                var data = Convert.ToInt32(Request["Name"].Trim());
                if (data != 0)
                {
                    Expression<Func<Entities.Bikesite, Boolean>> tmp = t => t.Id == data;
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
            var listBikesite = _bikesiteReadRepository.GetAll();

            var sessionschoolids = Session["SchoolIds"] as List<int>;
            if (sessionschoolids != null && sessionschoolids.Count > 0)
            {
                list = list.Where(t => sessionschoolids.Contains(t.Id));
            }
            var schoollist = list.Select(b => new SelectListItem { Text = b.Name, Value = b.Id.ToString() });
            model.Search.SchoolList.AddRange(schoollist);
            model.Search.SchoolList.Insert(0, new SelectListItem { Text = "---请选择---", Value = "0" });
            model.Search.BikesiteList.Insert(0, new SelectListItem { Text = "---请选择---", Value = "0" });
            ViewData["Bikesite"] = JsonConvert.SerializeObject(listBikesite.ToList());
        }
        #endregion
    }
}