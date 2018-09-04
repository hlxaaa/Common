using Common.Result;
using System.Web.Mvc;
using Common.Extend;
using System.Linq;
using Newtonsoft.Json;
using Common.Helper;

namespace Common.Filter.Mvc
{
    public class MvcValidateAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Model验证
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            var Controller = actionContext.Controller;
            ModelStateDictionary ModelState = (ModelStateDictionary)ReflexHelper.Instance.GetPropertyValue(Controller, "ModelState");
            bool? IsValid = null;
            if (ModelState != null)
            {
                IsValid = ModelState.IsValid;
            }
            if (IsValid == false)
            {
                ResultJson result = new ResultJson();
                result.HttpCode = 500;
                foreach (var item in ModelState.Values)
                {
                    foreach (var error in item.Errors)
                    {
                        if (!error.ErrorMessage.IsNullOrEmpty())
                        {
                            result.Message += error.ErrorMessage + ",";
                        }
                    }
                }
                if (result.Message != null)
                {
                    result.Message = result.Message.Remove(result.Message.Count() - 1, 1);
                }
                var JsonString = JsonConvert.SerializeObject(result);

                JsonResult jsonResult = new JsonResult();
                jsonResult.Data = result;
                jsonResult.ContentType = "application/json";
                jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                actionContext.Result = jsonResult;
            }
        }
    }


}
