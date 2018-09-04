using Common.Helper;
using Common.Result;
using System.Web.Mvc;

namespace Common.Filter.Mvc
{
    /// <summary>
    /// Mvc异常过滤器
    /// </summary>
    public class MvcExceptionAttribute : FilterAttribute, IExceptionFilter
    {
        /// <summary>
        /// 发生错误之后
        /// </summary>
        /// <param name="filterContext">条件内容</param>
        public void OnException(ExceptionContext filterContext)
        {
            JsonResult jsonResult = new JsonResult();
            ResultForWeb result = new ResultForWeb();
            result.HttpCode = 500;
            result.Message = filterContext.Exception.Message;
            LogHelper.WriteLog(typeof(MvcExceptionAttribute), filterContext.Exception);
            jsonResult.Data = result;
            jsonResult.ContentType = "application/json";
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            filterContext.Result = jsonResult;
            filterContext.ExceptionHandled = true;
        }
    }
}
