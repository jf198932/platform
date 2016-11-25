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