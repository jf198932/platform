using System.Collections.Generic;

namespace isriding.Web.Helper.Wxpay
{
    public class WxPayHelper
    {
        public string GetBizSign(Dictionary<string, string> bizObj, bool flag)
        {
            string unSignParaString = CommonUtil.FormatBizQueryParaMap(bizObj, flag);
            return MD5SignUtil.Sign(unSignParaString, WxpayConfig.appkey);
            //Dictionary<string, string> bizParameters = new Dictionary<string, string>();

            //foreach (KeyValuePair<string, string> item in bizObj)
            //{
            //    if (item.Key != "")
            //    {
            //        bizParameters.Add(item.Key.ToLower(), item.Value);
            //    }
            //}

            //if (this.AppKey == "")
            //{
            //    throw new SDKRuntimeException("APPKEY为空！");
            //}
            //bizParameters.Add("appkey", AppKey);
            //string bizString = CommonUtil.FormatBizQueryParaMap(bizParameters, false);

            //return SHA1Util.Sha1(bizString);

        }
    }
}