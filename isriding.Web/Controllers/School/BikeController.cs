using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using Abp.Domain.Uow;
using Abp.UI;
using Abp.Web.Models;
using isriding.Web.Extension.Fliter;
using isriding.Web.Helper;
using isriding.Web.Models.Common;
using isriding.Web.Models.School;
using AutoMapper;
using isriding.Bike;
using isriding.Bikesite;
using isriding.School;
using isriding.User;

namespace isriding.Web.Controllers.School
{
    public class BikeController : isridingControllerBase
    {
        private readonly IBikeWriteRepository _bikeRepository;
        private readonly IBikeReadRepository _bikeReadRepository;
        private readonly IUserReadRepository _userReadRepository;
        private readonly IBikesiteReadRepository _bikesiteReadRepository;
        private readonly ISchoolReadRepository _schoolReadRepository;
        
        public BikeController(IBikeWriteRepository bikeRepository
            , IBikeReadRepository bikeReadRepository
            , IUserReadRepository userReadRepository
            , IBikesiteReadRepository bikesiteReadRepository
            , ISchoolReadRepository schoolReadRepository)
        {
            _bikeRepository = bikeRepository;
            _bikeReadRepository = bikeReadRepository;
            _userReadRepository = userReadRepository;
            _bikesiteReadRepository = bikesiteReadRepository;
            _schoolReadRepository = schoolReadRepository;
        }

        // GET: Bike
        //[AdminLayout]
        //[AdminPermission(PermissionCustomMode.Enforce)]
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }
        [AdminLayout]
        //[AdminPermission(PermissionCustomMode.Enforce)]
        public ActionResult List()
        {
            BikeModel model = new BikeModel();
            PrepareAllBikeModel(model);
            return View(model);
        }

        [DontWrapResult, UnitOfWork]
        public virtual ActionResult InitDataTable(DataTableParameter param)
        {
            var expr = BuildSearchCriteria();
            var temp = _bikeReadRepository.GetAll();
            if (expr != null)
            {
                temp = temp.Where(expr);
            }
            var query = temp
                    .OrderBy(s => s.Id)
                    .Skip(param.iDisplayStart)
                    .Take(param.iDisplayLength);
            var total = temp.Count();
            var filterResult = query.Select(t => new BikeModel
            {
                Id = t.Id,
                Ble_name = t.Ble_name,
                Ble_serial = t.Ble_serial,
                Ble_type = t.Ble_type,
                Lock_status = t.Lock_status,
                Bike_status = t.Bike_status,
                Vlock_status = t.Vlock_status,
                Position = t.Position,
                Battery = t.Battery,
                User_id = t.User_id,
                Bikesite_id = t.Bikesite_id,
                Insite_status = t.Bikesite_id == null ? 2 : 1,
                Bikesite_name = t.Bikesite ==null ? "" : t.Bikesite.Name,
                User_name = t.User == null ? "" : t.User.Name,
                Phone = t.User == null ? "" : t.User.Phone,
                School_id = t.School_id,
                School_name = t.School.Name,
                Rent_type = t.Rent_type
            }).ToList();
            int sortId = param.iDisplayStart + 1;
            var result = from t in filterResult
                         select new[]
                             {
                                sortId++.ToString(),
                                t.Ble_serial,
                                t.Ble_name,
                                t.Ble_type.ToString(),
                                t.School_name,
                                t.Vlock_status.ToString(),
                                t.Bikesite_name,
                                t.Phone,
                                t.Position,
                                t.Rent_type.ToString(),
                                t.Insite_status.ToString(),
                                t.Id.ToString()
                            };

            return DataTableJsonResult(param.sEcho, param.iDisplayStart, total, total, result);
        }

        public ActionResult Create()
        {
            var model = new BikeModel();
            PrepareAllBikeModel(model);
            return PartialView(model);
        }

        [HttpPost, UnitOfWork]
        public virtual ActionResult Create(BikeModel model)
        {
            if (ModelState.IsValid)
            {
                Mapper.Initialize(t=> t.CreateMap<BikeModel, Entities.Bike>());
                var bike = Mapper.Map<Entities.Bike>(model);
                //bike.School_id = CommonHelper.GetSchoolId();
                bike.Insite_status = model.Bikesite_id == null ? 2 : 1;
                _bikeRepository.Insert(bike);

                //SuccessNotification("添加成功");
                return Json(model);
            }
            return Json(null);
        }
        [UnitOfWork]
        public virtual ActionResult Edit(int id)
        {
            Mapper.Initialize(t => t.CreateMap<Entities.Bike, BikeModel>());
            var model = Mapper.Map<BikeModel>(_bikeReadRepository.Get(id));
            //var model = role.ToModel();
            PrepareAllBikeModel(model);
            return PartialView(model);
        }

        [HttpPost, UnitOfWork]
        public virtual ActionResult Edit(BikeModel model)
        {
            var bike = _bikeRepository.Get(model.Id);

            if (ModelState.IsValid)
            {
                bike.Ble_name = model.Ble_name;
                bike.Ble_serial = model.Ble_serial;
                bike.Vlock_status = model.Vlock_status;
                bike.Bikesite_id = model.Bikesite_id;
                bike.User_id = model.User_id;
                bike.Position = model.Position;
                bike.Battery = model.Battery;
                bike.Lock_pwd = model.Lock_pwd;
                bike.Insite_status = model.Bikesite_id == null ? 2 : 1;
                bike.Rent_type = model.Rent_type;
                bike.Updated_at = DateTime.Now;

                bike = _bikeRepository.Update(bike);


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
            _bikeRepository.Delete(s => s.Id == id);
            //var role = _roleService.GetRoleById(id);
            //_roleService.DeleteRole(role);

            return Json(new { success = true });
        }

        public virtual ActionResult TemplateBike()
        {
            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("序列号");
            row1.CreateCell(1).SetCellValue("名称");
            row1.CreateCell(2).SetCellValue("类型");
            row1.CreateCell(3).SetCellValue("学校");
            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", "追踪器导入模板.xls");
        }

        [HttpPost]
        public ActionResult ImportExcel()
        {
            HttpPostedFileBase selectedFile = Request.Files["files"];
            if (selectedFile != null)
            {
                
                string fileName = Path.GetFileName(selectedFile.FileName);
                string fileEx = Path.GetExtension(fileName);
                
                DataTable dt = ExcelHelper.ImportExcelFile(selectedFile.InputStream, fileEx);
                //DataView dv = new DataView(dttemp);
                //DataTable dt = dv.ToTable(true, "序列号,名称,类型,学校");//去重
                //检验模板是否正确
                bool flg = false;
                if (dt != null)
                {
                    flg = CheckTemplate(dt);
                }
                if (!flg || dt.Rows.Count <= 0)
                {
                    throw new UserFriendlyException("模板错误");
                }
                if (!CheckData(dt))
                {
                    throw new UserFriendlyException("数据不能空");
                }

                //BackLoginModel user = (BackLoginModel)Session["CurrentUser"];

                #region 判断是否有重复的

                
                #endregion

                int ai = 0;
                foreach (DataRow row in dt.Rows)
                {
                    ai++;
                    var bike = new Entities.Bike
                    {
                        Ble_serial = row[0].ToString(),
                        Ble_name = row[1].ToString(),
                        Ble_type = int.Parse(row[2].ToString()),
                        Vlock_status = 0,
                        School_id = int.Parse(row[3].ToString()),
                        Created_at = DateTime.Now,
                        Updated_at = DateTime.Now,

                    };
                    _bikeRepository.Insert(bike);
                    if (ai >= 5000)
                    {
                        CurrentUnitOfWork.SaveChanges();
                        ai = 0;
                    }
                }
                return Json(null);
            }
            return Json(null);
        }

        #region 公共方法
        [NonAction, UnitOfWork]
        protected virtual void PrepareAllBikeModel(BikeModel model)
        {
            if (model == null)
                throw new ArgumentNullException("model");
            model.UserList.AddRange(
                _userReadRepository.GetAll().Select(b => new SelectListItem { Text = b.Name, Value = b.Id.ToString() }));
            model.UserList.Insert(0, new SelectListItem {Text = "--请选择--", Value = ""});
            model.BikesiteList.AddRange(
                _bikesiteReadRepository.GetAll().Select(b => new SelectListItem {Text = b.Name, Value = b.Id.ToString()}));
            model.BikesiteList.Insert(0, new SelectListItem { Text = "--请选择--", Value = "" });
            model.TypeList.AddRange(new List<SelectListItem>
            {
                new SelectListItem {Text = "追踪器", Value = "1"},
                new SelectListItem {Text = "智能锁", Value = "2"},
                new SelectListItem {Text = "蓝牙锁", Value = "3"},
                new SelectListItem {Text = "机械锁", Value = "4"}
            });
            model.LockStatusList.AddRange(new List<SelectListItem>
            {
                new SelectListItem {Text = "初始", Value = "0"},
                new SelectListItem {Text = "锁闭", Value = "1"},
                new SelectListItem {Text = "锁开", Value = "2"},
                new SelectListItem {Text = "异常", Value = "3"},
                new SelectListItem {Text = "异常(已推送)", Value = "4"},
                new SelectListItem {Text = "报警", Value = "5"}
            });
            model.BikeStatusList.AddRange(new List<SelectListItem>
            {
                new SelectListItem {Text = "可租用", Value = "1"},
                new SelectListItem {Text = "出租中", Value = "2"}
            });
            model.VlockStatusList.AddRange(new List<SelectListItem>
            {
                new SelectListItem {Text = "初始", Value = "0"},
                new SelectListItem {Text = "锁闭", Value = "1"},
                new SelectListItem {Text = "锁开", Value = "2"},
                new SelectListItem {Text = "异常", Value = "3"},
                new SelectListItem {Text = "异常(已推送)", Value = "4"},
                new SelectListItem {Text = "报警", Value = "5"}
            });
            model.InsiteStatusList.AddRange(new List<SelectListItem>
            {
                new SelectListItem {Text = "在桩", Value = "1"},
                new SelectListItem {Text = "离桩", Value = "2"}
            });

            var list = _schoolReadRepository.GetAll();

            var sessionschoolids = Session["SchoolIds"] as List<int>;
            if (sessionschoolids != null && sessionschoolids.Count > 0)
            {
                list = list.Where(t => sessionschoolids.Contains(t.Id));
            }
            var schoollist = list.Select(b => new SelectListItem { Text = b.Name, Value = b.Id.ToString() });
            model.SchoolList.AddRange(schoollist);
            model.SchoolList.Insert(0, new SelectListItem { Text = "---请选择---", Value = "0" });
            model.Search.SchoolList.AddRange(schoollist);
            model.Search.SchoolList.Insert(0, new SelectListItem { Text = "---请选择---", Value = "0" });
        }

        public bool CheckTemplate(DataTable dt)
        {

            if (!dt.Columns.Contains("序列号"))
            {
                return false;
            }
            if (!dt.Columns.Contains("名称"))
            {
                return false;
            }
            if (!dt.Columns.Contains("类型"))
            {
                return false;
            }
            if (!dt.Columns.Contains("学校"))
            {
                return false;
            }
            if (dt.Columns.Count != 4)
            {
                return false;
            }
            return true;
        }

        public bool CheckData(DataTable dt)
        {
            foreach (DataRow dr in dt.Rows)
            {
                if (string.IsNullOrEmpty(dr[0].ToString()) || string.IsNullOrEmpty(dr[1].ToString()) || string.IsNullOrEmpty(dr[2].ToString()) || string.IsNullOrEmpty(dr[3].ToString()))
                {
                    return false;
                }
            }
            return true;
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
            if (!string.IsNullOrEmpty(Request["Vlock_status"]) && Request["Vlock_status"].Trim() != "-1")
            {
                var data = Convert.ToInt32(Request["Vlock_status"].Trim());
                Expression<Func<Entities.Bike, Boolean>> tmp = t => t.Vlock_status == data;
                expr = bulider.BuildQueryAnd(expr, tmp);
            }
            if (!string.IsNullOrEmpty(Request["Ble_type"]) && Request["Ble_type"].Trim() != "0")
            {
                var data = Convert.ToInt32(Request["Ble_type"].Trim());
                Expression<Func<Entities.Bike, Boolean>> tmp = t => t.Ble_type >= data;
                expr = bulider.BuildQueryAnd(expr, tmp);
            }
            if (!string.IsNullOrEmpty(Request["Rent_type"]) && Request["Rent_type"].Trim() != "-1")
            {
                var data = Convert.ToInt32(Request["Rent_type"].Trim());
                Expression<Func<Entities.Bike, Boolean>> tmp = t => t.Rent_type == data;
                expr = bulider.BuildQueryAnd(expr, tmp);
            }
            if (!string.IsNullOrEmpty(Request["School_id"]))
            {
                var data = Convert.ToInt32(Request["School_id"].Trim());
                if (data > 0)
                {
                    Expression<Func<Entities.Bike, Boolean>> tmp = t => t.School_id == data;
                    expr = bulider.BuildQueryAnd(expr, tmp);
                }
                else
                {
                    var sessionschoolids = Session["SchoolIds"] as List<int>;
                    if (sessionschoolids != null && sessionschoolids.Count > 0)
                    {
                        Expression<Func<Entities.Bike, Boolean>> tmp = t => sessionschoolids.Contains((int)t.School_id);
                        expr = bulider.BuildQueryAnd(expr, tmp);
                    }
                }
            }
            else
            {
                var sessionschoolids = Session["SchoolIds"] as List<int>;
                if (sessionschoolids != null && sessionschoolids.Count > 0)
                {
                    Expression<Func<Entities.Bike, Boolean>> tmp = t => sessionschoolids.Contains((int)t.School_id);
                    expr = bulider.BuildQueryAnd(expr, tmp);
                }
            }
            //var sessionschoolids = Session["SchoolIds"] as List<int>;
            //if (sessionschoolids != null && sessionschoolids.Count > 0)
            //{
            //    Expression<Func<Entities.Bike, Boolean>> tmp = t => sessionschoolids.Contains((int)t.School_id);
            //    expr = bulider.BuildQueryAnd(expr, tmp);
            //}
            //var id = CommonHelper.GetSchoolId();
            //if (id > 1)
            //{
            //    Expression<Func<Entities.Bike, Boolean>> tmpSolid = t => t.School_id == id;
            //    expr = bulider.BuildQueryAnd(expr, tmpSolid);
            //}
            return expr;
        }

        #endregion
        #endregion
    }
}