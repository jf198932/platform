using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Abp.Domain.Uow;
using Abp.Web.Models;
using isriding.School;
using isriding.Track;
using isriding.Web.Extension.Fliter;
using isriding.Web.Models.Chart;

namespace isriding.Web.Controllers.Chart
{
    public class TrackChartController : Controller
    {
        private readonly ITrackReadRepository _trackReadRepository;
        private readonly ISchoolReadRepository _schoolReadRepository;

        public TrackChartController(ITrackReadRepository trackReadRepository, ISchoolReadRepository schoolReadRepository)
        {
            _trackReadRepository = trackReadRepository;
            _schoolReadRepository = schoolReadRepository;
        }

        // GET: TrackChart
        public ActionResult Index()
        {
            return RedirectToAction("TrackChartBar");
        }
        [AdminLayout]
        public ActionResult TrackChartBar()
        {
            var model = new TrackChartSearchModel();
            PrepareTrackChartModel(model);
            return View(model);
        }

        [DontWrapResult, UnitOfWork]
        public virtual ActionResult GetTrackChartData(int School_id, int Month)
        {
            var track = _trackReadRepository.GetAll().Where(t => t.Trade_no != null);
            if (School_id > 0)
            {
                track = track.Where(t => t.Bike.School_id == School_id);
            }
            else
            {
                var sessionschoolids = Session["SchoolIds"] as List<int>;
                if (sessionschoolids != null && sessionschoolids.Count > 0)
                {
                    track = track.Where(t => sessionschoolids.Contains((int)t.Bike.School_id));
                }
            }
            var tracklist = track.ToList();
            var now = DateTime.Now;
            
            List<string> months = new List<string>();
            List<int> datars = new List<int>();
            List<decimal> datacs = new List<decimal>();
            if (Month > 0)
            {
                int days = DateTime.DaysInMonth(now.Year, Month);
                for (int i = 1; i <= days; i++)
                {
                    var time = new DateTime(now.Year, Month, i);
                    months.Add(time.ToString("MM-dd"));
                    datars.Add(
                        tracklist.Count(
                            t =>
                                DateTime.Parse(t.Start_time.ToString()).ToString("yyyy/MM/dd") ==
                                time.ToString("yyyy/MM/dd")));
                    datacs.Add(
                        tracklist.Where(
                            t =>
                                DateTime.Parse(t.Start_time.ToString()).ToString("yyyy/MM/dd") ==
                                time.ToString("yyyy/MM/dd")).Sum(t => (decimal)(t.Payment ?? 0)));
                }
            }
            else
            {
                for (int i = 1; i <= 12; i++)
                {
                    var time = new DateTime(now.Year, i, 1);
                    months.Add(i + "月");
                    datars.Add(
                        tracklist.Count(
                            t => DateTime.Parse(t.Start_time.ToString()).ToString("yyyy/MM") == time.ToString("yyyy/MM")));
                    datacs.Add(
                        tracklist.Where(
                            t => DateTime.Parse(t.Start_time.ToString()).ToString("yyyy/MM") == time.ToString("yyyy/MM"))
                            .Sum(t => (decimal)(t.Payment ?? 0)));
                }
            }
            
            return Json(new { months = months, datars = datars, datacs = datacs}, JsonRequestBehavior.AllowGet);
        }

        [NonAction, UnitOfWork]
        protected virtual void PrepareTrackChartModel(TrackChartSearchModel model)
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
            model.SchoolList.AddRange(schoollist);
            model.SchoolList.Insert(0, new SelectListItem { Text = "---请选择---", Value = "0" });
        }
    }
}