using Common.Helper;
using Common.Result;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;


namespace Common.Attribute
{
    public class PwdValidateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            this.ErrorMessage = "密码应为6~16位英文字母、数字";
            if (value != null)
            {
                var RegexStr = StringHelper.Instance.regPwd;
                if (Regex.IsMatch(value.ToString(), RegexStr))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }

    /// <summary>
    /// 密码可为空，不空就要符合格式
    /// </summary>
    public class PwdValidate2Attribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            //ErrorTips e = new ErrorTips();
            //e.errorClass = "pwd";
            //e.tips = "密码应为6~16位英文字母、数字";
            //this.ErrorMessage = "pwd,密码应为6~16位英文字母、数字";
            //ErrorMessage = "密码应为6~16位英文字母、数字";
            if (value != null)
            {
                var RegexStr = StringHelper.Instance.regPwd;
                if (Regex.IsMatch(value.ToString(), RegexStr))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }
    }
}