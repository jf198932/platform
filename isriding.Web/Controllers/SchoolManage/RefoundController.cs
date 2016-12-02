using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web.Mvc;
using System.Xml;
using Abp.Domain.Uow;
using Abp.Logging;
using Abp.Web.Models;
using AutoMapper;
using isriding.Recharge;
using isriding.Recharge_detail;
using isriding.School;
using isriding.Web.Extension.Fliter;
using isriding.Web.Helper;
using isriding.Web.Helper.Alipay;
using isriding.Web.Helper.Wxpay;
using isriding.Web.Models.Common;
using isriding.Web.Models.SchoolManage;

namespace isriding.Web.Controllers.SchoolManage
{
    public class RefoundController : isridingControllerBase
    {
        //private readonly IRefoundReadRepository _refoundReadRepository;
        //private readonly IRefoundWriteRepository _refoundWriteRepository;
        private readonly IRecharge_detailReadRepository _rechargeDetailReadRepository;
        private readonly IRecharge_detailWriteRepository _rechargeDetailWriteRepository;
        private readonly ISchoolReadRepository _schoolReadRepository;
        private readonly IRechargeWriteRepository _rechargeWriteRepository;

        public RefoundController(IRecharge_detailReadRepository rechargeDetailReadRepository
            , IRecharge_detailWriteRepository rechargeDetailWriteRepository
            , ISchoolReadRepository schoolReadRepository
            , IRechargeWriteRepository rechargeWriteRepository)
        {
            _rechargeDetailReadRepository = rechargeDetailReadRepository;
            _rechargeDetailWriteRepository = rechargeDetailWriteRepository;
            _schoolReadRepository = schoolReadRepository;
            _rechargeWriteRepository = rechargeWriteRepository;
        }

        // GET: Refound
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }
        [AdminLayout]
        public ActionResult List()
        {
            var model = new RefoundModel();
            PrepareAllUserModel(model);
            return View(model);
        }

        [DontWrapResult, UnitOfWork]
        public virtual ActionResult InitDataTable(DataTableParameter param)
        {
            var expr = BuildSearchCriteria();
            var temp = _rechargeDetailReadRepository.GetAll();
            if (expr != null)
            {
                temp = temp.Where(expr);
            }
            var query = temp.OrderBy(s => s.Id).Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var total = temp.Count();
            var filterResult = query.Select(t => new RefoundModel
            {
                Id = t.Id,
                Recharge_amount = t.Recharge_amount,
                Created_at = t.Created_at,
                Updated_at = t.Updated_at,
                Status = t.Status,
                Type = t.Type,
                Recharge_type = t.Recharge_type,
                Recharge_method = t.Recharge_method,
                User_id = t.User_id,
                User_name = t.User == null ? "" : t.User.Name,
                School_name = t.User.School.Name,
                doc_no = t.doc_no,
                recharge_docno = t.recharge_docno
            }).ToList();
            int sortId = param.iDisplayStart + 1;
            var result = from t in filterResult
                         select new[]
                             {
                                sortId++.ToString(),
                                t.School_name,
                                t.User_name,
                                t.Recharge_amount.ToString(),
                                t.Recharge_method.ToString(),
                                t.Recharge_type.ToString(),
                                t.Type.ToString(),
                                t.Status.ToString(),
                                t.recharge_docno,
                                t.doc_no,
                                t.Id.ToString()
                            };

            return DataTableJsonResult(param.sEcho, param.iDisplayStart, total, total, result);
        }

        [UnitOfWork]
        public virtual ActionResult Edit(int id)
        {
            Mapper.Initialize(t => t.CreateMap<Entities.Refound, RefoundModel>());
            var model = Mapper.Map<RefoundModel>(_rechargeDetailReadRepository.Get(id));
            //var model = role.ToModel();
            PrepareAllUserModel(model);
            return PartialView(model);
        }

        [HttpPost, UnitOfWork]
        public virtual ActionResult Edit(RefoundModel model)
        {
            //var detail = _rechargeDetailWriteRepository.Get(model.Id);

            if (ModelState.IsValid)
            {
                return Json(model);
            }
            return Json(null);
        }

        [HttpPost, UnitOfWork]
        public virtual ActionResult Refund(string id)
        {
            var detail = _rechargeDetailWriteRepository.FirstOrDefault(t => t.doc_no == id);

            if (ModelState.IsValid)
            {

                if (detail.Recharge_method == 1) //支付宝
                {
                    //退款详细数据，必填，格式（支付宝交易号^退款金额^备注），多笔请用#隔开
                    string detail_data = detail.doc_no + "^" + detail.Recharge_amount + "^" + "正常退款";//"2016111821001004570251489448^9.9^正常退款";
                                                                                                     //把请求参数打包成数组
                    SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
                    sParaTemp.Add("partner", Config.partner);
                    sParaTemp.Add("_input_charset", Config.input_charset.ToLower());
                    sParaTemp.Add("service", "refund_fastpay_by_platform_nopwd");
                    //sParaTemp.Add("notify_url", ConfigurationManager.AppSettings["Alipay_refund_notify_url"]);
                    sParaTemp.Add("batch_no", DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(1000, 9999));
                    sParaTemp.Add("refund_date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    sParaTemp.Add("batch_num", "1");
                    sParaTemp.Add("detail_data", detail_data);

                    //建立请求
                    string sHtmlText = Submit.BuildRequest(sParaTemp);
                    //todo log
                    //LogHelper.Logger.Info(sHtmlText);

                    XmlDocument xmlDoc = new XmlDocument();

                    xmlDoc.LoadXml(sHtmlText);
                    XmlNode xn = xmlDoc.SelectSingleNode("alipay");
                    XmlNodeList xnl = xn.ChildNodes;
                    if (xnl[0].InnerText == "T")
                    {
                        detail.Status = 2;
                        detail.Updated_at = DateTime.Now;
                        _rechargeDetailWriteRepository.Update(detail);

                        var recharge = _rechargeWriteRepository.FirstOrDefault(t => t.User_id == detail.User_id);
                        recharge.Deposit = 0;
                        _rechargeWriteRepository.Update(recharge);

                        _rechargeDetailWriteRepository.Insert(new Entities.Recharge_detail
                        {
                            Created_at = DateTime.Now,
                            Updated_at = DateTime.Now,
                            User_id = detail.User_id,
                            Recharge_amount = detail.Recharge_amount,
                            Recharge_method = 1,
                            Recharge_type = detail.Recharge_type,
                            recharge_docno = DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(1000, 9999),
                            doc_no = "",
                            Type = 2,
                            Status = 0,
                            source_recharge_docno = detail.recharge_docno,
                            source_doc_no = detail.doc_no
                        });
                    }
                    return Json("SUCCESS");
                }
                else if (detail.Recharge_method == 2) //微信
                {
                    var noncestr = CommonUtil.CreateNoncestr();
                    //var timespan = ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000).ToString();

                    Dictionary<string, string> sPara = new Dictionary<string, string>();
                    sPara.Add("appid", WxpayConfig.appId);
                    sPara.Add("mch_id", WxpayConfig.mchid);
                    sPara.Add("nonce_str", noncestr);
                    sPara.Add("transaction_id", detail.doc_no);
                    sPara.Add("out_refund_no", DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(1000, 9999));
                    sPara.Add("total_fee", (detail.Recharge_amount * 100).ToString());
                    sPara.Add("refund_fee", (detail.Recharge_amount * 100).ToString());
                    sPara.Add("op_user_id", WxpayConfig.mchid);

                    var wxpayhelper = new WxPayHelper();
                    var sign = wxpayhelper.GetBizSign(sPara, false);
                    sPara.Add("sign", sign);

                    var requestXml = CommonUtil.ArrayToXml(sPara);

                    //string cert = @"~/cert/apiclient_cert.p12";
                    string cert = Server.MapPath("~/cert/apiclient_cert.p12");
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                    var cer = new X509Certificate(cert, WxpayConfig.mchid, X509KeyStorageFlags.MachineKeySet);

                    var resp = HttpHelper.PostDataToServerForHttps(
                        "https://api.mch.weixin.qq.com/secapi/pay/refund", requestXml, HttpWebRequestMethod.POST, cer);

                    //todo log
                    //LogHelper.Logger.Info(resp);

                    var xdoc = new XmlDocument();
                    xdoc.LoadXml(resp);
                    XmlNode xn = xdoc.SelectSingleNode("xml");
                    XmlNodeList xnl = xn.ChildNodes;

                    if (xnl[6].InnerText == "SUCCESS") //退款成功
                    {
                        detail.Status = 2;
                        detail.Updated_at = DateTime.Now;
                        _rechargeDetailWriteRepository.Update(detail);

                        var recharge = _rechargeWriteRepository.FirstOrDefault(t => t.User_id == detail.User_id);
                        recharge.Deposit = 0;
                        _rechargeWriteRepository.Update(recharge);

                        _rechargeDetailWriteRepository.Insert(new Entities.Recharge_detail
                        {
                            Created_at = DateTime.Now,
                            Updated_at = DateTime.Now,
                            User_id = detail.User_id,
                            Recharge_amount = double.Parse(xnl[12].InnerText) / 100,
                            Recharge_method = 2,
                            Recharge_type = detail.Recharge_type,
                            recharge_docno = DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(1000, 9999),
                            doc_no = xnl[10].InnerText,
                            Type = 2,
                            Status = 0,
                            source_recharge_docno = detail.recharge_docno,
                            source_doc_no = detail.doc_no
                        });
                    }

                    return Json("SUCCESS");
                }
            }
            return Json(null);
        }

        [HttpPost, UnitOfWork]
        public virtual ActionResult Delete(int id)
        {
            _rechargeDetailWriteRepository.Delete(s => s.Id == id);
            //var role = _roleService.GetRoleById(id);
            //_roleService.DeleteRole(role);

            return Json(new { success = true });
        }

        #region 公共方法
        [NonAction, UnitOfWork]
        protected virtual void PrepareAllUserModel(RefoundModel model)
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
        #region 构建查询表达式
        /// <summary>
        /// 构建查询表达式
        /// </summary>
        /// <returns></returns>
        private Expression<Func<Entities.Recharge_detail, Boolean>> BuildSearchCriteria()
        {
            DynamicLambda<Entities.Recharge_detail> bulider = new DynamicLambda<Entities.Recharge_detail>();
            Expression<Func<Entities.Recharge_detail, Boolean>> expr = null;
            
            Expression<Func<Entities.Recharge_detail, Boolean>> tm = t => t.doc_no != null;
            expr = bulider.BuildQueryAnd(expr, tm);
            if (!string.IsNullOrEmpty(Request["User_name"]))
            {
                var data = Request["User_name"].Trim();
                Expression<Func<Entities.Recharge_detail, Boolean>> tmp = t => t.User.Name.Contains(data);
                expr = bulider.BuildQueryAnd(expr, tmp);
            }
            if (!string.IsNullOrEmpty(Request["Type"]) && Convert.ToInt32(Request["Type"].Trim()) > 0)
            {
                var data = Convert.ToInt32(Request["Type"].Trim());
                Expression<Func<Entities.Recharge_detail, Boolean>> tmp = t => t.Type == data;
                expr = bulider.BuildQueryAnd(expr, tmp);
            }
            if (!string.IsNullOrEmpty(Request["Recharge_type"]) && Convert.ToInt32(Request["Recharge_type"].Trim()) > 0)
            {
                var data = Convert.ToInt32(Request["Recharge_type"].Trim());
                Expression<Func<Entities.Recharge_detail, Boolean>> tmp = t => t.Recharge_type == data;
                expr = bulider.BuildQueryAnd(expr, tmp);
            }
            //Recharge_method
            if (!string.IsNullOrEmpty(Request["Recharge_method"]) && Convert.ToInt32(Request["Recharge_method"].Trim()) > 0)
            {
                var data = Convert.ToInt32(Request["Recharge_method"].Trim());
                Expression<Func<Entities.Recharge_detail, Boolean>> tmp = t => t.Recharge_method == data;
                expr = bulider.BuildQueryAnd(expr, tmp);
            }
            if (!string.IsNullOrEmpty(Request["Status"]) && Convert.ToInt32(Request["Status"].Trim()) > -1)
            {
                var data = Convert.ToInt32(Request["Status"].Trim());
                Expression<Func<Entities.Recharge_detail, Boolean>> tmp = t => t.Status == data;
                expr = bulider.BuildQueryAnd(expr, tmp);
            }


            if (!string.IsNullOrEmpty(Request["StartDate"]))
            {
                var data = DateTime.Parse(Request["StartDate"].Trim());
                Expression<Func<Entities.Recharge_detail, Boolean>> tmp = t => t.Created_at >= data;
                expr = bulider.BuildQueryAnd(expr, tmp);
            }
            if (!string.IsNullOrEmpty(Request["EndDate"]))
            {
                var data = DateTime.Parse(Request["EndDate"].Trim());
                Expression<Func<Entities.Recharge_detail, Boolean>> tmp = t => t.Created_at <= data;
                expr = bulider.BuildQueryAnd(expr, tmp);
            }


            if (!string.IsNullOrEmpty(Request["School_id"]))
            {
                var data = Convert.ToInt32(Request["School_id"].Trim());
                if (data > 0)
                {
                    Expression<Func<Entities.Recharge_detail, Boolean>> tmp = t => t.User.School_id == data;
                    expr = bulider.BuildQueryAnd(expr, tmp);
                }
                else
                {
                    var sessionschoolids = Session["SchoolIds"] as List<int>;
                    if (sessionschoolids != null && sessionschoolids.Count > 0)
                    {
                        Expression<Func<Entities.Recharge_detail, Boolean>> tmp = t => sessionschoolids.Contains((int)t.User.School_id);
                        expr = bulider.BuildQueryAnd(expr, tmp);
                    }
                }
            }
            else
            {
                var sessionschoolids = Session["SchoolIds"] as List<int>;
                if (sessionschoolids != null && sessionschoolids.Count > 0)
                {
                    Expression<Func<Entities.Recharge_detail, Boolean>> tmp = t => sessionschoolids.Contains((int)t.User.School_id);
                    expr = bulider.BuildQueryAnd(expr, tmp);
                }
            }

            return expr;
        }
        #endregion
        #endregion

        private bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            if (errors == SslPolicyErrors.None)
                return true;
            return false;
        }
    }
}