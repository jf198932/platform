using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Web.Models;
using isriding.Web.Extension.Fliter;
using isriding.Web.Helper;
using isriding.Web.Models.Common;
using isriding.Web.Models.School;
using AutoMapper;

namespace isriding.Web.Controllers.School
{
    public class UserController : isridingControllerBase
    {
        private readonly IRepository<Entities.User> _userRepository;
        private readonly IRepository<Entities.School> _schoolRepository;

        public UserController(IRepository<Entities.User> userRepository, IRepository<Entities.School> schoolRepository)
        {
            _userRepository = userRepository;
            _schoolRepository = schoolRepository;
        }
        // GET: User
        //[AdminLayout]
        public ActionResult Index()
        {
            return RedirectToAction("ListU");
        }
        [AdminLayout]
        //[AdminPermission(PermissionCustomMode.Enforce)]
        public ActionResult ListU()
        {
            var model = new UserModel();
            PrepareAllUserModel(model);
            return View("List", model);
        }

        [DontWrapResult, UnitOfWork]
        public virtual ActionResult InitDataTable(DataTableParameter param)
        {
            var expr = BuildSearchCriteria();
            var temp = _userRepository.GetAll();
            if (expr != null)
            {
                temp = temp.Where(expr);
            }
            var query = temp.OrderBy(s => s.Id).Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var total = temp.Count();
            var filterResult = query.Select(t => new UserModel
            {
                Id = t.Id,
                Name = t.Name,
                Phone = t.Phone,
                Nickname = t.Nickname,
                Weixacc = t.Weixacc,
                School_id = t.School_id,
                Email = t.Email,
                Certification = t.Certification,
                Textmsg = t.Textmsg,
                Textmsg_time = DateTime.Now,
                Remember_token = t.Remember_token,
                Credits = t.Credits,
                Balance = t.Balance,
                School_name = t.School.Name
            }).ToList();
            int sortId = param.iDisplayStart + 1;
            var result = from t in filterResult
                         select new[]
                             {
                                sortId++.ToString(),
                                t.Phone,
                                t.Name,
                                t.Nickname,
                                t.School_name,
                                t.Remember_token,
                                t.Certification.ToString(),
                                t.Id.ToString()
                            };

            return DataTableJsonResult(param.sEcho, param.iDisplayStart, total, total, result);
        }

        public ActionResult Create()
        {
            var model = new UserModel();
            PrepareAllUserModel(model);
            return PartialView(model);
        }

        [HttpPost, UnitOfWork]
        public virtual ActionResult Create(UserModel model)
        {
            if (ModelState.IsValid)
            {
                Mapper.Initialize(t=> t.CreateMap<UserModel, Entities.User>());
                var user = Mapper.Map<Entities.User>(model);
                user = _userRepository.Insert(user);

                //SuccessNotification("添加成功");
                return Json(model);
            }
            return Json(null);
        }
        [UnitOfWork]
        public virtual ActionResult Edit(int id)
        {
            Mapper.Initialize(t=> t.CreateMap<Entities.User, UserModel>());
            var model = Mapper.Map<UserModel>(_userRepository.Get(id));
            //var model = role.ToModel();
            PrepareAllUserModel(model);
            return PartialView(model);
        }

        [HttpPost, UnitOfWork]
        public virtual ActionResult Edit(UserModel model)
        {
            var user = _userRepository.Get(model.Id);

            if (ModelState.IsValid)
            {
                user.Name = model.Name;
                user.Nickname = model.Nickname;
                user.School_id = model.School_id;
                user.Certification = model.Certification;
                user.Credits = model.Credits;
                user.Balance = model.Balance;
                user.Updated_at = DateTime.Now;

                _userRepository.Update(user);
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
            _userRepository.Delete(s => s.Id == id);
            //var role = _roleService.GetRoleById(id);
            //_roleService.DeleteRole(role);

            return Json(new { success = true });
        }

        #region 公共方法
        [NonAction, UnitOfWork]
        protected virtual void PrepareAllUserModel(UserModel model)
        {
            if (model == null)
                throw new ArgumentNullException("model");
            model.SchoolList.AddRange(
                _schoolRepository.GetAll().Select(b => new SelectListItem { Text = b.Name, Value = b.Id.ToString() }));
            model.CertificationList.AddRange(new List<SelectListItem>
            {
                new SelectListItem {Text = "未申请", Value = "1"},
                new SelectListItem {Text = "已申请", Value = "2"},
                new SelectListItem {Text = "已认证", Value = "3"},
                new SelectListItem {Text = "认证失败", Value = "4"}
            });
            var list = _schoolRepository.GetAll();
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
        private Expression<Func<Entities.User, Boolean>> BuildSearchCriteria()
        {
            DynamicLambda<Entities.User> bulider = new DynamicLambda<Entities.User>();
            Expression<Func<Entities.User, Boolean>> expr = null;
            if (!string.IsNullOrEmpty(Request["Name"]))
            {
                var data = Request["Name"].Trim();
                Expression<Func<Entities.User, Boolean>> tmp = t => t.Name.Contains(data);
                expr = bulider.BuildQueryAnd(expr, tmp);
            }
            if (!string.IsNullOrEmpty(Request["Phone"]))
            {
                var data = Request["Phone"].Trim();
                Expression<Func<Entities.User, Boolean>> tmp = t => t.Phone.Contains(data);
                expr = bulider.BuildQueryAnd(expr, tmp);
            }
            if (!string.IsNullOrEmpty(Request["Nickname"]))
            {
                var data = Request["Nickname"].Trim();
                Expression<Func<Entities.User, Boolean>> tmp = t => t.Nickname.Contains(data);
                expr = bulider.BuildQueryAnd(expr, tmp);
            }
            if (!string.IsNullOrEmpty(Request["Certification"]) && Request["Certification"].Trim() != "0")
            {
                var data = Convert.ToInt32(Request["Certification"].Trim());
                Expression<Func<Entities.User, Boolean>> tmp = t => t.Certification == data;
                expr = bulider.BuildQueryAnd(expr, tmp);
            }
            if (!string.IsNullOrEmpty(Request["School_id"]))
            {
                var data = Convert.ToInt32(Request["School_id"].Trim());
                if (data > 0)
                {
                    Expression<Func<Entities.User, Boolean>> tmp = t => t.School_id == data;
                    expr = bulider.BuildQueryAnd(expr, tmp);
                }
                else
                {
                    var sessionschoolids = Session["SchoolIds"] as List<int>;
                    if (sessionschoolids != null && sessionschoolids.Count > 0)
                    {
                        Expression<Func<Entities.User, Boolean>> tmp = t => sessionschoolids.Contains((int)t.School_id);
                        expr = bulider.BuildQueryAnd(expr, tmp);
                    }
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