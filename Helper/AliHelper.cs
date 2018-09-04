using Aop.Api;
using Aop.Api.Domain;
using Aop.Api.Request;
using Aop.Api.Response;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helper
{
    public class AliHelper:SingleTon<AliHelper>
    {
        #region 基本配置

        public string app_private_key = ConfigurationManager.AppSettings["app_private_key"];
        public static string alipay_public_key = ConfigurationManager.AppSettings["alipay_public_key"];
        public static string charset = "utf-8";

        //public string gateway = "https://openapi.alipaydev.com/gateway.do";
        public string gateway = "https://openapi.alipay.com/gateway.do";//正式？


        public string appid = ConfigurationManager.AppSettings["aliAppId"];//应用2.0签约2017120190774565


        #endregion

        /// <summary>
        /// .NET服务端SDK生成APP支付订单信息示例
        /// </summary>
        /// 
        //.NET服务端SDK生成APP支付订单信息示例(https://docs.open.alipay.com/54/106370/)根据文档上的说法，这样就行了。
        /// <summary>
        /// 这个行了
        /// </summary>
        /// <param name="totalAmount"></param>
        /// <param name="outTradeNo"></param>
        /// <param name="notifyUrl"></param>
        /// <returns></returns>
        public string CreateAlipayOrder(string totalAmount, string outTradeNo, string notifyUrl)
        {

            IAopClient client = new DefaultAopClient(gateway, appid, app_private_key, "json", "1.0", "RSA2", alipay_public_key, charset, false);
            //实例化具体API对应的request类,类名称和接口名称对应,当前调用接口名称如：alipay.trade.app.pay
            AlipayTradeAppPayRequest request = new AlipayTradeAppPayRequest();
            //SDK已经封装掉了公共参数，这里只需要传入业务参数。以下方法为sdk的model入参方式(model和biz_content同时存在的情况下取biz_content)。
            request.SetApiVersion("1.0");
            AlipayTradeAppPayModel model = new AlipayTradeAppPayModel();
            model.Body = "body";
            model.Subject = "App支付";
            model.TotalAmount = totalAmount;
            model.ProductCode = "QUICK_MSECURITY_PAY";
            //订单号

            model.OutTradeNo = outTradeNo;
            model.TimeoutExpress = "30m";
            request.SetBizModel(model);
            request.SetNotifyUrl(notifyUrl);
            //这里和普通的接口调用不同，使用的是sdkExecute
            AlipayTradeAppPayResponse response = client.SdkExecute(request);
            //HttpUtility.HtmlEncode是为了输出到页面时防止被浏览器将关键参数html转义，实际打印到日志以及http传输不会有这个问题
            var a = response.Body;
            return response.Body;
            //return HttpUtility.HtmlEncode(response.Body).Replace("&amp;", "&");

            //Response.Write(HttpUtility.HtmlEncode(response.Body));
            //页面输出的response.Body就是orderString 可以直接给客户端请求，无需再做处理。
        }

    }
}
