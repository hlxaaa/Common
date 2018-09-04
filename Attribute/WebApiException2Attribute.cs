using System.Web.Http.Filters;
using System.Net.Http;
using System.Configuration;
using Common.Result;

namespace Common.Attribute
{
    public class WebApiException2Attribute : ExceptionFilterAttribute
    {
        string isServer = ConfigurationManager.AppSettings.Get("isServer");

        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            ResultRes r = new ResultRes
            {
                httpcode = 500
            };
            var test = actionExecutedContext.Exception.Message;
            if (isServer == "0")
                r.message = test;
            else
                r.message = "网络不稳定";
            actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(System.Net.HttpStatusCode.OK, r);
            base.OnException(actionExecutedContext);
        }
    }
}