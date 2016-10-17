using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Web.Models;
using isriding.Entities;
using isriding.Web.Extension.Fliter;
using isriding.Web.Models.Chart;

namespace isriding.Web.Controllers.Chart
{
    public class UserChartController : Controller
    {
        private readonly IRepository<User> _useRepository;
        private readonly IRepository<Entities.School> _schoolRepository;

        public UserChartController(IRepository<User> useRepository, IRepository<Entities.School> schoolRepository)
        {
            _useRepository = useRepository;
            _schoolRepository = schoolRepository;
        }

        // GET: UserChart
        public ActionResult Index()
        {
            return RedirectToAction("UserChartBar");
        }
        [AdminLayout]
        public ActionResult UserChartBar()
        {
            var model = new UserChartSearchModel();
            PrepareUserChartModel(model);
            return View(model);
        }

        [DontWrapResult, UnitOfWork]
        public virtual ActionResult GetUserChartData(int School_id, int Month)
        {
            var user = _useRepository.GetAll();
            if (School_id > 0)
            {
                user = user.Where(t => t.School_id == School_id);
            }
            else
            {
                var sessionschoolids = Session["SchoolIds"] as List<int>;
                if (sessionschoolids != null && sessionschoolids.Count > 0)
                {
                    user = user.Where(t => sessionschoolids.Contains((int)t.School_id));
                }
            }
            var userlist = user.ToList();
            var now = DateTime.Now;
            
            List<string> months = new List<string>();
            List<int> datars = new List<int>();
            List<int> datacs = new List<int>();
            if (Month > 0)
            {
                int days = DateTime.DaysInMonth(now.Year, Month);
                for (int i = 1; i <= days; i++)
                {
                    var time = new DateTime(now.Year, Month, i);
                    months.Add(time.ToString("MM-dd"));
                    datars.Add(userlist.Count(t => DateTime.Parse(t.Created_at.ToString()).ToString("yyyy/MM/dd") == time.ToString("yyyy/MM/dd")));
                    datacs.Add(userlist.Count(t => DateTime.Parse(t.Created_at.ToString()).ToString("yyyy/MM/dd") == time.ToString("yyyy/MM/dd") && t.Certification == 3));
                }
            }
            else
            {
                for (int i = 1; i <= 12; i++)
                {
                    var time = new DateTime(now.Year, i, 1);
                    months.Add(i + "月");
                    datars.Add(userlist.Count(t => DateTime.Parse(t.Created_at.ToString()).ToString("yyyy/MM") == time.ToString("yyyy/MM")));
                    datacs.Add(userlist.Count(t => DateTime.Parse(t.Created_at.ToString()).ToString("yyyy/MM") == time.ToString("yyyy/MM") && t.Certification == 3));
                }
            }
            
            return Json(new { months = months, datars = datars, datacs = datacs}, JsonRequestBehavior.AllowGet);
        }

        [NonAction, UnitOfWork]
        protected virtual void PrepareUserChartModel(UserChartSearchModel model)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            var list = _schoolRepository.GetAll();

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