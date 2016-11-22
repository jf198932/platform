using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using Abp.UI;
using isriding.Web.Helper;
using isriding.Web.Helper.Alipay;
using isriding.Web.Helper.Wxpay;

namespace isriding.Web.Controllers
{
    public class AboutController : isridingControllerBase
    {
        public ActionResult Index()
        {
            ////HttpRuntime.AppDomainAppPath + "App_Data\\rsa_private_key.pem"
            //IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", AlipayHelper.app_id,AlipayHelper.private_key,"json", "1.0","RSA", AlipayHelper.alipay_public_key);

            ////IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", "app_id", "merchant_private_key", "json", "RSA", "alipay_public_key", "GBK");
            //AlipayTradeRefundRequest request = new AlipayTradeRefundRequest();

            //request.BizContent = "{" +
            //"\"out_trade_no\":\"201611181515352617\"," +
            //"\"trade_no\":\"2016111821001004570251489448\"," +
            //"\"refund_amount\":99," +
            //"\"refund_reason\":\"正常退款\"," +
            //"\"operator_id\":\"OP001\"," +
            //"\"store_id\":\"NJ_S_001\"," +
            //"\"terminal_id\":\"NJ_T_001\"" +
            //"}";
            //AlipayTradeRefundResponse response = client.Execute(request);
            //if (response.Msg == "Success")
            //{
            //    Console.WriteLine("调用成功");
            //}
            //else
            //{
            //    Console.WriteLine("调用失败");
            //}


            //////////////////////////////////////////////请求参数////////////////////////////////////////////

            ////批次号，必填，格式：当天日期[8位]+序列号[3至24位]，如：201603081000001

            //string batch_no = DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(1000, 9999);

            ////退款笔数，必填，参数detail_data的值中，“#”字符出现的数量加1，最大支持1000笔（即“#”字符出现的数量999个）

            //string batch_num = "1";

            ////退款详细数据，必填，格式（支付宝交易号^退款金额^备注），多笔请用#隔开
            //string detail_data = "2016111821001004570251489448^9.9^正常退款";



            //////////////////////////////////////////////////////////////////////////////////////////////////

            ////把请求参数打包成数组
            //SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
            //sParaTemp.Add("service", Config.service);
            //sParaTemp.Add("partner", Config.partner);
            //sParaTemp.Add("_input_charset", Config.input_charset.ToLower());
            ////sParaTemp.Add("notify_url", Config.notify_url);
            //sParaTemp.Add("seller_user_id", Config.seller_user_id);
            //sParaTemp.Add("refund_date", Config.refund_date);
            //sParaTemp.Add("batch_no", batch_no);
            //sParaTemp.Add("batch_num", batch_num);
            //sParaTemp.Add("detail_data", detail_data);

            ////建立请求
            //string sHtmlText = Submit.BuildRequest(sParaTemp, "get", "确认");
            //Response.Write(sHtmlText);



            var noncestr = CommonUtil.CreateNoncestr();
            var timespan = ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000).ToString();


            Dictionary<string, string> sPara = new Dictionary<string, string>();
            sPara.Add("appid", WxpayConfig.appId);
            //sPara.Add("input_charset", "UTF-8");
            sPara.Add("mch_id", WxpayConfig.mchid);
            sPara.Add("nonce_str", noncestr);
            //sPara.Add("notify_url", ConfigurationManager.AppSettings["Wxpay_notify_url"]);
            sPara.Add("transaction_id", "4004072001201611200255219517");
            sPara.Add("out_refund_no", DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(1000, 9999));
            //sPara.Add("spbill_create_ip", "192.168.1.69");
            sPara.Add("total_fee", "29900");
            sPara.Add("refund_fee", "29900");
            sPara.Add("op_user_id", WxpayConfig.mchid);

            var wxpayhelper = new WxPayHelper();

            var sign = wxpayhelper.GetBizSign(sPara, false);

            sPara.Add("sign", sign);

            var requestXml = CommonUtil.ArrayToXml(sPara);

            //string cert = @"F:\apiclient_cert.p12";
            string cert = @"~/cert/apiclient_cert.p12";
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            X509Certificate cer = new X509Certificate(cert, WxpayConfig.mchid);
            //HttpWebRequest webrequest = (HttpWebRequest)HttpWebRequest.Create("https://api.mch.weixin.qq.com/secapi/pay/refund");
            //webrequest.ClientCertificates.Add(cer);
            //webrequest.Method = "post";
            //byte[] bytes = Encoding.UTF8.GetBytes(requestXml);
            //webrequest.ContentType = "application/xml;charset=utf-8";
            //webrequest.ContentLength = (long)bytes.Length;
            //Stream requestStream = ((WebRequest)webrequest).GetRequestStream();
            //requestStream.Write(bytes, 0, bytes.Length);
            //requestStream.Close();



            //HttpWebResponse webreponse = (HttpWebResponse)webrequest.GetResponse();
            //Stream stream = webreponse.GetResponseStream();
            //string resp = string.Empty;
            //using (StreamReader reader = new StreamReader(stream))
            //{
            //    resp = reader.ReadToEnd();
            //}
            var resp = HttpHelper.PostDataToServerForHttps(
                            "https://api.mch.weixin.qq.com/secapi/pay/refund", requestXml, HttpWebRequestMethod.POST,
                            cer);
            //var result = HttpHelper.PostDataToServerForHttps("https://api.mch.weixin.qq.com/secapi/pay/refund", requestXml, HttpWebRequestMethod.POST);

            //获取预支付ID
            //var xdoc = new XmlDocument();
            //xdoc.LoadXml(result);
            //XmlNode xn = xdoc.SelectSingleNode("xml");
            //XmlNodeList xnl = xn.ChildNodes;
            //if (xnl[0].InnerText != "FAIL")
            //{
            //    if (xnl.Count > 7)
            //    {
            //        prepayId = xnl[7].InnerText;
            //        //package = string.Format("prepay_id={0}", prepayId);
            //    }
            //    Dictionary<string, string> sPara2 = new Dictionary<string, string>();
            //    sPara2.Add("appid", WxpayConfig.appId);
            //    sPara2.Add("noncestr", noncestr);
            //    sPara2.Add("package", WxpayConfig.package);
            //    sPara2.Add("partnerid", WxpayConfig.mchid);
            //    sPara2.Add("prepayid", prepayId);
            //    sPara2.Add("timestamp", timespan);

            //    var sign2 = wxpayhelper.GetBizSign(sPara2, false);

            //    var outresult = new WxpayOutput();
            //    outresult.appid = WxpayConfig.appId;
            //    outresult.noncestr = noncestr;
            //    outresult.package = WxpayConfig.package;
            //    outresult.partnerid = WxpayConfig.mchid;
            //    outresult.prepayid = prepayId;
            //    outresult.timestamp = timespan;
            //    outresult.sign = sign2;

            //    return outresult;
            //}
            //else
            //{
            //    throw new UserFriendlyException(xnl[1].InnerText);
            //}
            return View();
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            if (errors == SslPolicyErrors.None)
                return true;
            return false;
        }
    }
}