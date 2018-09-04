using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helper
{
    public class HttpHelper : SingleTon<HttpHelper>
    {
        public string HttpGet(string url)
        {
            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);

            httpRequest.Method = "GET";
            httpRequest.ContentType = "application/json";
            httpRequest.Referer = null;
            httpRequest.AllowAutoRedirect = true;
            httpRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.2; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
            httpRequest.Accept = "*/*";

            HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();

            Stream receiveStream = httpResponse.GetResponseStream();

            string result = string.Empty;
            using (StreamReader sr = new StreamReader(receiveStream))
            {
                result = sr.ReadToEnd();
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData">就是传的数据的model的字符串</param>
        /// <returns></returns>
        public string HttpPost(string url, string postData)
        {
            HttpWebRequest httpRequest = (HttpWebRequest)HttpWebRequest.Create(url);

            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/json";
            httpRequest.Referer = null;
            httpRequest.AllowAutoRedirect = true;
            httpRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.2; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
            httpRequest.Accept = "*/*";

            Stream requestStem = httpRequest.GetRequestStream();
            StreamWriter sw = new StreamWriter(requestStem);
            sw.Write(postData);
            sw.Close();
            HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            Stream receiveStream = httpResponse.GetResponseStream();
            string result = string.Empty;
            using (StreamReader sr = new StreamReader(receiveStream))
            {
                result = sr.ReadToEnd();
            }
            return result;
        }

    }
}
