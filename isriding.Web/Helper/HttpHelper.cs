using System;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace isriding.Web.Helper
{
    public class HttpHelper
    {
        private static Encoding encoding = (Encoding)new UTF8Encoding();

        static HttpHelper()
        {
        }

        /// <summary>
        /// 主要功能：访问远程URL
        ///             作    者：孙亚民
        ///             创建日期：2010/06/07
        /// 
        /// </summary>
        /// <param name="url">远程访问的地址</param><param name="data">参数</param>
        /// <returns>
        /// 远程页面调用结果
        /// </returns>
        public static string CallUrl(string url, string data, int timeout = 30000)
        {
            HttpWebRequestMethod method = HttpWebRequestMethod.POST;
            return HttpHelper.CallUrl(url, data, method, timeout);
        }

        /// <summary>
        /// 主要功能：访问远程URL
        ///             作    者：孙亚民
        ///             创建日期：2010/06/07
        /// 
        /// </summary>
        /// <param name="url">远程访问的地址</param><param name="data">参数</param><param name="method">Http页面请求方法</param>
        /// <returns>
        /// 远程页面调用结果
        /// </returns>
        public static string CallUrl(string url, string data, HttpWebRequestMethod method, int timeout = 30000)
        {
            if (method == HttpWebRequestMethod.GET)
                url = url.IndexOf("?") <= 0 ? url + "?" : url + "&";
            try
            {
                HttpWebRequest httpWebRequest = WebRequest.Create(url) as HttpWebRequest;
                switch (method)
                {
                    case HttpWebRequestMethod.GET:
                        httpWebRequest.Method = ((object)HttpWebRequestMethod.GET).ToString();
                        break;
                    case HttpWebRequestMethod.POST:
                        httpWebRequest.Method = ((object)HttpWebRequestMethod.POST).ToString();
                        byte[] bytes = Encoding.UTF8.GetBytes(data);
                        httpWebRequest.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
                        httpWebRequest.ContentLength = (long)bytes.Length;
                        Stream requestStream = ((WebRequest)httpWebRequest).GetRequestStream();
                        requestStream.Write(bytes, 0, bytes.Length);
                        requestStream.Close();
                        break;
                }
                HttpWebResponse httpWebResponse;
                try
                {
                    httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                }
                catch (WebException ex)
                {
                    httpWebResponse = (HttpWebResponse)ex.Response;
                }
                Stream responseStream = httpWebResponse.GetResponseStream();
                StreamReader streamReader = new StreamReader(responseStream);
                string str = streamReader.ReadToEnd();
                streamReader.Close();
                responseStream.Close();
                httpWebResponse.Close();
                return str;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 向服务器提交XML数据
        /// 
        /// </summary>
        /// <param name="url">远程访问的地址</param><param name="data">参数</param><param name="method">Http页面请求方法</param>
        /// <returns>
        /// 远程页面调用结果
        /// </returns>
        public static string PostJsonDataToServer(string url, string data, HttpWebRequestMethod method)
        {
            try
            {
                HttpWebRequest httpWebRequest = WebRequest.Create(url) as HttpWebRequest;
                switch (method)
                {
                    case HttpWebRequestMethod.GET:
                        httpWebRequest.Method = ((object)HttpWebRequestMethod.GET).ToString();
                        break;
                    case HttpWebRequestMethod.POST:
                        httpWebRequest.Method = ((object)HttpWebRequestMethod.POST).ToString();
                        byte[] bytes = Encoding.UTF8.GetBytes(data);
                        httpWebRequest.ContentType = "application/json;charset=utf-8";
                        httpWebRequest.ContentLength = (long)bytes.Length;
                        Stream requestStream = ((WebRequest)httpWebRequest).GetRequestStream();
                        requestStream.Write(bytes, 0, bytes.Length);
                        requestStream.Close();
                        break;
                }
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                Stream responseStream = httpWebResponse.GetResponseStream();
                StreamReader streamReader = new StreamReader(responseStream);
                string str = streamReader.ReadToEnd();
                streamReader.Close();
                responseStream.Close();
                httpWebResponse.Close();
                return str;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 通过HttpWebRequest方法从服务器取方法
        /// 
        /// </summary>
        /// <param name="url">需请求的URL</param>
        /// <returns>
        /// 返回的字符串
        /// </returns>
        public static string GetDataFromServer(string url)
        {
            return HttpHelper.GetDataFromServer(url, string.Empty);
        }

        /// <summary>
        /// 通过HttpWebRequest方法从服务器取方法
        /// 
        /// </summary>
        /// <param name="url">需请求的URL</param><param name="inputCharset">字符集</param>
        /// <returns>
        /// 返回的字符串
        /// </returns>
        public static string GetDataFromServer(string url, string inputCharset)
        {
            try
            {
                using (HttpWebResponse httpWebResponse = (HttpWebResponse)WebRequest.Create(url).GetResponse())
                {
                    using (Stream responseStream = httpWebResponse.GetResponseStream())
                    {
                        Encoding encoding = (Encoding)null;
                        if (!string.IsNullOrEmpty(inputCharset))
                        {
                            try
                            {
                                encoding = Encoding.GetEncoding(inputCharset);
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        return (encoding == null ? (TextReader)new StreamReader(responseStream) : (TextReader)new StreamReader(responseStream, encoding)).ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static string PostDataToServerForFlight(string url, string data, HttpWebRequestMethod method)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)null;
            try
            {
                switch (method)
                {
                    case HttpWebRequestMethod.GET:
                        url = string.Format("{0}?{1}", (object)url, (object)data);
                        httpWebRequest = WebRequest.Create(url) as HttpWebRequest;
                        httpWebRequest.Method = ((object)HttpWebRequestMethod.GET).ToString();
                        break;
                    case HttpWebRequestMethod.POST:
                        httpWebRequest = WebRequest.Create(url) as HttpWebRequest;
                        httpWebRequest.Method = ((object)HttpWebRequestMethod.POST).ToString();
                        byte[] bytes = Encoding.UTF8.GetBytes(data);
                        httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                        httpWebRequest.ContentLength = (long)bytes.Length;
                        Stream requestStream = ((WebRequest)httpWebRequest).GetRequestStream();
                        requestStream.Write(bytes, 0, bytes.Length);
                        requestStream.Close();
                        break;
                }
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                Stream responseStream = httpWebResponse.GetResponseStream();
                StreamReader streamReader = new StreamReader(responseStream);
                string str = streamReader.ReadToEnd();
                streamReader.Close();
                responseStream.Close();
                httpWebResponse.Close();
                return str;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static string PostDataToServer(string url, string data)
        {
            return HttpHelper.PostDataToServer(url, data, HttpWebRequestMethod.POST, 30000);
        }

        /// <summary>
        /// 向服务器提交XML数据
        /// 
        /// </summary>
        /// <param name="url">远程访问的地址</param><param name="data">参数</param><param name="method">Http页面请求方法</param>
        /// <returns>
        /// 远程页面调用结果
        /// </returns>
        public static string PostDataToServer(string url, string data, HttpWebRequestMethod method, int timeout = 30000)
        {
            try
            {
                HttpWebRequest httpWebRequest = WebRequest.Create(url) as HttpWebRequest;
                httpWebRequest.Timeout = timeout;
                switch (method)
                {
                    case HttpWebRequestMethod.GET:
                        httpWebRequest.Method = ((object)HttpWebRequestMethod.GET).ToString();
                        break;
                    case HttpWebRequestMethod.POST:
                        httpWebRequest.Method = ((object)HttpWebRequestMethod.POST).ToString();
                        byte[] bytes = Encoding.UTF8.GetBytes(data);
                        httpWebRequest.ContentType = "application/xml;charset=utf-8";
                        httpWebRequest.ContentLength = (long)bytes.Length;
                        Stream requestStream = ((WebRequest)httpWebRequest).GetRequestStream();
                        requestStream.Write(bytes, 0, bytes.Length);
                        requestStream.Close();
                        break;
                }
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                Stream responseStream = httpWebResponse.GetResponseStream();
                StreamReader streamReader = new StreamReader(responseStream);
                string str = streamReader.ReadToEnd();
                streamReader.Close();
                responseStream.Close();
                httpWebResponse.Close();
                return str;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 向服务器提交XML数据(https调用，加入Tls协议)
        /// 
        /// </summary>
        /// <param name="url">远程访问的地址</param><param name="data">参数</param><param name="method">Http页面请求方法</param><param name="securityProtocolType">Schannel安全协议类型，默认为tls</param>
        /// <returns>
        /// 远程页面调用结果
        /// </returns>
        public static string PostDataToServerForHttps(string url, string data, HttpWebRequestMethod method, SecurityProtocolType securityProtocolType = SecurityProtocolType.Tls, int timeout = 10000)
        {
            try
            {
                string str = string.Empty;
                HttpWebRequest httpWebRequest = WebRequest.Create(url) as HttpWebRequest;
                httpWebRequest.Timeout = timeout;
                httpWebRequest.Proxy = (IWebProxy)null;
                ServicePointManager.SecurityProtocol = securityProtocolType;
                switch (method)
                {
                    case HttpWebRequestMethod.GET:
                        httpWebRequest.Method = ((object)HttpWebRequestMethod.GET).ToString();
                        break;
                    case HttpWebRequestMethod.POST:
                        httpWebRequest.Method = ((object)HttpWebRequestMethod.POST).ToString();
                        byte[] bytes = Encoding.UTF8.GetBytes(data);
                        httpWebRequest.ContentType = "application/xml;charset=utf-8";
                        httpWebRequest.ContentLength = (long)bytes.Length;
                        Stream requestStream = ((WebRequest)httpWebRequest).GetRequestStream();
                        requestStream.Write(bytes, 0, bytes.Length);
                        requestStream.Close();
                        break;
                }
                using (HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    using (Stream responseStream = httpWebResponse.GetResponseStream())
                    {
                        using (StreamReader streamReader = new StreamReader(responseStream))
                            str = streamReader.ReadToEnd();
                    }
                }
                return str;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string CallUrl(string url, string data, HttpWebRequestMethod method, string inputCharset, int timeout = 0)
        {
            if (method == HttpWebRequestMethod.GET)
                url = url.IndexOf("?") <= 0 ? url + "?" : url + "&";
            try
            {
                HttpWebRequest httpWebRequest = WebRequest.Create(url) as HttpWebRequest;
                if (timeout > 0)
                    httpWebRequest.Timeout = timeout;
                switch (method)
                {
                    case HttpWebRequestMethod.GET:
                        httpWebRequest.Method = ((object)HttpWebRequestMethod.GET).ToString();
                        break;
                    case HttpWebRequestMethod.POST:
                        httpWebRequest.Method = ((object)HttpWebRequestMethod.POST).ToString();
                        byte[] bytes = Encoding.GetEncoding(inputCharset).GetBytes(data);
                        httpWebRequest.ContentType = string.Format("application/x-www-form-urlencoded;charset={0}", (object)inputCharset);
                        httpWebRequest.ContentLength = (long)bytes.Length;
                        Stream requestStream = ((WebRequest)httpWebRequest).GetRequestStream();
                        requestStream.Write(bytes, 0, bytes.Length);
                        requestStream.Close();
                        break;
                }
                string str = string.Empty;
                HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
                using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.GetEncoding(inputCharset)))
                    str = streamReader.ReadToEnd();
                httpWebResponse.Close();
                return str;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetRequest(string requestXml, string mQuestUrl)
        {
            return HttpHelper.GetRequest(requestXml, mQuestUrl, 0);
        }

        /// <summary>
        /// 发起HTTP请求
        /// 
        /// </summary>
        /// <param name="m_Doc"/><param name="mQuestUrl"/>
        /// <returns/>
        public static string GetRequest(string requestXml, string mQuestUrl, int timeout)
        {
            string str1 = string.Empty;
            string str2;
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(mQuestUrl);
                byte[] bytes = Encoding.GetEncoding("UTF-8").GetBytes(requestXml);
                httpWebRequest.Method = "Post";
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                httpWebRequest.ContentLength = (long)bytes.Length;
                ((NameValueCollection)httpWebRequest.Headers).Add("Accept-Encoding", "gzip");
                if (timeout > 0)
                    httpWebRequest.Timeout = timeout;
                Stream requestStream = ((WebRequest)httpWebRequest).GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                Stream stream = httpWebResponse.GetResponseStream();
                if (httpWebResponse.ContentEncoding.ToLower().Contains("gzip"))
                    stream = (Stream)new GZipStream(stream, CompressionMode.Decompress);
                StreamReader streamReader = new StreamReader(stream, Encoding.GetEncoding("UTF-8"));
                str2 = streamReader.ReadToEnd();
                streamReader.Close();
            }
            catch (WebException ex1)
            {
                if (ex1.Status != WebExceptionStatus.Timeout)
                {
                    try
                    {
                        HttpWebResponse httpWebResponse = (HttpWebResponse)ex1.Response;
                        new StreamReader(httpWebResponse.GetResponseStream(), HttpHelper.encoding).ReadToEnd();
                        httpWebResponse.Close();
                    }
                    catch (Exception ex2)
                    {
                        string message = ex2.Message;
                        throw ex2;
                    }
                    throw ex1;
                }
                else
                    str2 = string.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return str2;
        }

        /// <summary>
        /// 向服务器提交XML数据(https调用，加入Tls协议)
        /// 
        /// </summary>
        /// <param name="url">远程访问的地址</param><param name="data">参数</param><param name="method">Http页面请求方法</param>
        /// <param name="cer">证书</param>
        /// <param name="securityProtocolType">Schannel安全协议类型，默认为tls</param>
        /// <returns>
        /// 远程页面调用结果
        /// </returns>
        public static string PostDataToServerForHttps(string url, string data, HttpWebRequestMethod method, X509Certificate cer, SecurityProtocolType securityProtocolType = SecurityProtocolType.Tls, int timeout = 10000)
        {
            try
            {
                string str = string.Empty;
                HttpWebRequest httpWebRequest = WebRequest.Create(url) as HttpWebRequest;
                httpWebRequest.Timeout = timeout;
                httpWebRequest.ClientCertificates.Add(cer);
                httpWebRequest.Proxy = (IWebProxy)null;
                ServicePointManager.SecurityProtocol = securityProtocolType;
                switch (method)
                {
                    case HttpWebRequestMethod.GET:
                        httpWebRequest.Method = ((object)HttpWebRequestMethod.GET).ToString();
                        break;
                    case HttpWebRequestMethod.POST:
                        httpWebRequest.Method = ((object)HttpWebRequestMethod.POST).ToString();
                        byte[] bytes = Encoding.UTF8.GetBytes(data);
                        httpWebRequest.ContentType = "application/xml;charset=utf-8";
                        httpWebRequest.ContentLength = (long)bytes.Length;
                        Stream requestStream = ((WebRequest)httpWebRequest).GetRequestStream();
                        requestStream.Write(bytes, 0, bytes.Length);
                        requestStream.Close();
                        break;
                }
                using (HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    using (Stream responseStream = httpWebResponse.GetResponseStream())
                    {
                        using (StreamReader streamReader = new StreamReader(responseStream))
                            str = streamReader.ReadToEnd();
                    }
                }
                return str;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public enum HttpWebRequestMethod
    {
        GET,
        POST,
    }
}