using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Net.Http;
using Common.Result;

namespace Common.Attribute
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                var r = new ResultJson() { HttpCode = 500, Message = "错了" };
                //自定义错误信息
                var item = actionContext.ModelState.Values.Take(1).SingleOrDefault();
                r.Message = item.Errors.Where(b => !string.IsNullOrWhiteSpace(b.ErrorMessage)).Take(1).SingleOrDefault().ErrorMessage;

                actionContext.Response = actionContext.Request.CreateResponse(System.Net.HttpStatusCode.OK, r);
            }
        }
    }
}