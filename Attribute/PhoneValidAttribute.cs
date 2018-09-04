using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Common.Attribute
{
    /// <summary>
    /// 操作验证
    /// </summary>
    public class PhoneValidAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            this.ErrorMessage = "手机号码验证失败";
            if (value == null)
            {
                return false;
            }
            else
            {
                //电信手机号码正则        
                string dianxin = @"^1[3578][01379]\d{8}$";
                Regex dReg = new Regex(dianxin);
                //联通手机号正则        
                string liantong = @"^1[34578][01256]\d{8}$";
                Regex tReg = new Regex(liantong);
                //移动手机号正则        
                string yidong = @"^(134[012345678]\d{7}|1[34578][012356789]\d{8})$";
                Regex yReg = new Regex(yidong);
                if (dReg.IsMatch(value.ToString()) || tReg.IsMatch(value.ToString()) || yReg.IsMatch(value.ToString()))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
