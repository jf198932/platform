using System;
using System.Linq;
using System.Web.Mvc;
using Abp.Domain.Uow;
using Abp.Web.Models;
using isriding.Web.Extension.Fliter;
using isriding.Web.Models.Common;
using isriding.Web.Models.School;
using AutoMapper;
using isriding.School;

namespace isriding.Web.Controllers.School
{
    public class SchoolController : isridingControllerBase
    {
        private readonly ISchoolWriteRepository _schoolRepository;
        private readonly ISchoolReadRepository _schoolReadRepository;

        public SchoolController(ISchoolWriteRepository schoolRepository, ISchoolReadRepository schoolReadRepository)
        {
            _schoolRepository = schoolRepository;
            _schoolReadRepository = schoolReadRepository;
        }

        // GET: School
        //[AdminLayout]
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        [AdminLayout]
        //[AdminPermission(PermissionCustomMode.Enforce)]
        public ActionResult List()
        {
            return View();
        }

        [DontWrapResult, UnitOfWork]
        public virtual ActionResult InitDataTable(DataTableParameter param)
        {

            var query =
                _schoolReadRepository.GetAll().OrderBy(s => s.Id).Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var total = _schoolReadRepository.Count();
            var filterResult = query.Select(t => new SchoolModel
            {
                Id = t.Id,
                Name = t.Name,
                Areacode = t.Areacode,
                Gps_point = t.Gps_point,
                Site_count = t.Site_count,
                Bike_count = t.Bike_count,
                Time_charge = t.Time_charge,
                Free_time = t.Free_time,
                Deposit = t.Deposit
            }).ToList();
            int sortId = param.iDisplayStart + 1;
            var result = from t in filterResult
                         select new[]
                             {
                                sortId++.ToString(),
                                t.Name,
                                t.Areacode,
                                t.Gps_point,
                                t.Site_count.ToString(),
                                t.Time_charge.ToString(),
                                t.Deposit.ToString(),
                                t.Free_time.ToString(),
                                t.Id.ToString()
                            };

            return DataTableJsonResult(param.sEcho, param.iDisplayStart, total, total, result);
        }

        public ActionResult Create()
        {
            var model = new SchoolModel();
            return PartialView(model);
        }

        [HttpPost, UnitOfWork, DontWrapResult]
        public virtual ActionResult Create(SchoolModel model)
        {
            if (ModelState.IsValid)
            {
                Mapper.Initialize(t=> t.CreateMap<SchoolModel, Entities.School>());
                var school = Mapper.Map<Entities.School>(model);
                _schoolRepository.Insert(school);
                CurrentUnitOfWork.SaveChanges();

                return Json(model);
            }
            return Json(null);
        }
        [UnitOfWork]
        public virtual ActionResult Edit(int id)
        {
            Mapper.Initialize(t=> t.CreateMap<Entities.School, SchoolModel>());
            var model = Mapper.Map<SchoolModel>(_schoolReadRepository.Get(id));
            //var model = role.ToModel();

            return PartialView(model);
        }

        [HttpPost, UnitOfWork, DontWrapResult]
        public virtual ActionResult Edit(SchoolModel model)
        {
            var school = _schoolRepository.Get(model.Id);

            if (ModelState.IsValid)
            {
                school.Name = model.Name;
                school.Areacode = model.Areacode;
                school.Bike_count = model.Bike_count;
                school.Gps_point = model.Gps_point;
                school.Site_count = model.Site_count;
                school.Time_charge = model.Time_charge;
                school.Refresh_date = DateTime.Now;
                school.Updated_at = DateTime.Now;

                _schoolRepository.Update(school);
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
            _schoolRepository.Delete(s => s.Id == id);
            //var role = _roleService.GetRoleById(id);
            //_roleService.DeleteRole(role);

            return Json(new { success = true });
        }

        [UnitOfWork, DontWrapResult]
        public virtual ActionResult CheckTenancyNameExists(string schoolname)
        {
            var model = _schoolReadRepository.FirstOrDefault(t => t.TenancyName.ToLower() == schoolname.ToLower());
            if (model != null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}