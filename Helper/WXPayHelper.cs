using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helper
{
    public class WXPayHelper : SingleTon<WXPayHelper>
    {
        #region APP支付
        string appId = ConfigurationManager.AppSettings["wxAppId"].ToString();//
        //string mch_id = "1494504712";//微信支付分配的商户号
        string mchId = ConfigurationManager.AppSettings["wxMchId"].ToString();
        string mchKey = ConfigurationManager.AppSettings["wxMchKey"].ToString();

        string nonceStr = RandHelper.Instance.Str(24, true);//随机字符串，不长于32位。推荐随机数生成算法

        string body = "饭的支付";//商品描述-腾讯充值中心-QQ会员充值
        //string out_trade_no = "thisisWXOrder" + DateTime.Now.ToString("yyyy-MM-dd");//商品订单号只能是数字、大小写字母_-|*@  32位以内

        public string CreateWXOrder(decimal totalAmount, string outTradeNo, string notifyUrl, string Spbill_create_ip)
        {
            string nonce_str = RandHelper.Instance.Str(24, true); ;//随机字符串，不长于32位。推荐随机数生成算法

            //string body = "饭的支付测试";//商品描述-腾讯充值中心-QQ会员充值
            string out_trade_no = outTradeNo;//商品订单号只能是数字、大小写字母_-|*@  32位以内
            //var fee = new Decimal(totalAmount);

            string total_fee = (Math.Round((decimal)totalAmount * 100, 0)).ToString();//总金额
            string spbill_create_ip = Spbill_create_ip;//终端ip-用户端实际ip-app传给我
            string notify_url = notifyUrl;//通知地址
            string trade_type = "APP";//交易类型

            //string mch_key = "2BCF8DD9490E328D2FCEDE7B26643231";

            var dict = new Dictionary<string, string>();
            dict.Add("appid", appId);
            dict.Add("attach", "1");
            dict.Add("mch_id", mchId);
            dict.Add("nonce_str", nonce_str);
            //dict.Add("sign", sign);
            dict.Add("body", body);
            dict.Add("out_trade_no", out_trade_no);
            dict.Add("total_fee", total_fee);
            dict.Add("spbill_create_ip", spbill_create_ip);
            dict.Add("notify_url", notify_url);
            dict.Add("trade_type", trade_type);
            //var signTemp = WXPayHelper.getParamSrc(dict) + "&key=" + mch_key;
            //string sign = MD5Helper.StrToMD5_UTF8(signTemp);//签名-签名生成算法

            string sign = GetWXSign(dict, mchKey);

            var xml = StringHelper.Instance.GetWX_XML(appId, "1", body, mchId, nonce_str, notify_url, out_trade_no, spbill_create_ip, total_fee, trade_type, sign);
            var url = "https://api.mch.weixin.qq.com/pay/unifiedorder";

            Stream outstream = null;
            Stream instream = null;
            StreamReader sr = null;
            HttpWebResponse response = null;
            HttpWebRequest request = null;
            // 要注意的这是这个编码方式，还有内容的Xml内容的编码方式
            Encoding encoding = Encoding.GetEncoding("UTF-8");
            byte[] data = encoding.GetBytes(xml);

            // 准备请求,设置参数
            request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "text/xml";

            //request.ContentLength = data.Length;

            outstream = request.GetRequestStream();
            outstream.Write(data, 0, data.Length);
            outstream.Flush();
            outstream.Close();
            //发送请求并获取相应回应数据

            response = request.GetResponse() as HttpWebResponse;
            //直到request.GetResponse()程序才开始向目标网页发送Post请求
            instream = response.GetResponseStream();

            sr = new StreamReader(instream, encoding);
            //返回结果网页(html)代码

            string content = sr.ReadToEnd();
            //return content;
            //var a = 1;
            return content;
        }

        public static String getParamSrc(Dictionary<string, string> paramsMap)
        {
            var vDic = (from objDic in paramsMap orderby objDic.Key ascending select objDic);
            StringBuilder str = new StringBuilder();
            foreach (KeyValuePair<string, string> kv in vDic)
            {
                string pkey = kv.Key;
                string pvalue = kv.Value;
                str.Append(pkey + "=" + pvalue + "&");
            }

            String result = str.ToString().Substring(0, str.ToString().Length - 1);
            return result;
        }

        /// <summary>
        /// 微信计算签名
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="mchKey"></param>
        /// <returns></returns>
        public string GetWXSign(Dictionary<string, string> dict, string mchKey)
        {
            var signTemp = WXPayHelper.getParamSrc(dict) + "&key=" + mchKey;
            return MD5Helper.Instance.StrToMD5_UTF8(signTemp);
        }
        #endregion
    }



}
