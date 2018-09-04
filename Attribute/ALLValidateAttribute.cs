using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Common.Attribute
{
    /// <summary>
    /// 纬度
    /// </summary>
    public class LatValidateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            this.ErrorMessage = "纬度应为0-90度";
            if (value != null)
            {
                var temp = Convert.ToDecimal(value);
                if (temp >= 0 && temp <= 90)
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

    public class IpAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            //this.ErrorMessage = "纬度应为0-90度";
            if (value != null)
            {

                var RegexStr = @"^\d+$";
                if (Regex.IsMatch(value.ToString(), RegexStr))
                {
                    var temp = Convert.ToInt32(value);
                    if (temp >= 0 && temp <= 255)
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
            else
            {
                return false;
            }
        }

    }

    public class PortAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            //this.ErrorMessage = "纬度应为0-90度";
            if (value != null)
            {

                var RegexStr = @"^\d+$";
                if (Regex.IsMatch(value.ToString(), RegexStr))
                {
                    var temp = Convert.ToInt32(value);
                    if (temp >= 1 && temp <= 65535)
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

                //var temp = Convert.ToDecimal(value);
                //if (temp >= 1 && temp <= 65535)
                //{
                //    return true;
                //}
                //else
                //{
                //    return false;
                //}
            }
            else
            {
                return false;
            }
        }

    }

    /// <summary>
    /// 纬度，可空
    /// </summary>
    public class LatValidate2Attribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            this.ErrorMessage = "纬度应为0-90度";
            if (value != null)
            {
                var temp = Convert.ToDecimal(value);
                if (temp >= 0 && temp <= 90)
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

    /// <summary>
    /// 经度
    /// </summary>
    public class LngValidateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            this.ErrorMessage = "经度应为0-180度";
            if (value != null)
            {
                var temp = Convert.ToDecimal(value);
                if (temp >= 0 && temp <= 180)
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
    /// 经度可空
    /// </summary>
    public class LngValidate2Attribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            this.ErrorMessage = "经度应为0-180度";
            if (value != null)
            {
                var temp = Convert.ToDecimal(value);
                if (temp >= 0 && temp <= 180)
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

    public class IdNotZeroValidateAttribute : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            if (value != null)
            {
                var temp = Convert.ToInt32(value);
                if (temp > 0)
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
    /// 字符串不为null，不为空
    /// </summary>
    public class StringValidateAttribute : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            if (value != null)
            {
                var temp = value.ToString().Trim();
                if (temp != "")
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
    /// 评价星级
    /// </summary>
    public class StarsValidateAttribute : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            if (value != null)
            {
                var temp = Convert.ToInt32(value);
                if (temp >= 0 && temp <= 5)
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
    /// 正小数（0），可以不传，传了必须是小数或整数
    /// </summary>
    public class DecimalValidate2Attribute : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            if (value != null)
            {
                var r = 0m;
                var flag = decimal.TryParse(value.ToString(), out r);
                //var temp = value.ToString().Trim();
                if (flag)
                {
                    if (r < 0)
                        return false;
                    else
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

    public class MinuteSecondAttribute : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            if (value != null)
            {
                var arr = value.ToString().Split(':');
                if (arr.Length != 2)
                    return false;
                var r = 0;
                var flag = int.TryParse(arr[1], out r);
                if (!flag)
                {
                    ErrorMessage = "秒数不是整数";
                    return false;
                }
                if (r < 0)
                {
                    ErrorMessage = "秒数不能小于0";
                    return false;
                }
                if (r > 59)
                {
                    ErrorMessage = "秒数不能大于59";
                    return false;
                }
                flag = int.TryParse(arr[0], out r);
                if (!flag)
                {
                    ErrorMessage = "分钟数不是整数";
                    return false;
                }
                if (r < 0)
                {
                    ErrorMessage = "分钟数不能小于0";
                    return false;
                }
                return true;

            }
            else
            {
                return false;
            }
        }
    }

    public class DecimalValidateAttribute : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            //this.ErrorMessage = "";
            if (value != null)
            {
                var r = 0m;
                var flag = decimal.TryParse(value.ToString(), out r);
                //var temp = value.ToString().Trim();
                if (flag)
                {
                    if (r < 0)
                        return false;
                    else
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
    /// 正整数（0），可以不传，传了必须是整数
    /// </summary>
    public class IntValidate2Attribute : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            if (value != null)
            {
                var r = 0;
                var flag = int.TryParse(value.ToString(), out r);
                //var temp = value.ToString().Trim();
                if (flag)
                {
                    if (r < 0)
                        return false;
                    else
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

    /// <summary>
    /// 正整数，必填
    /// </summary>
    public class IntValidateAttribute : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            if (value != null)
            {
                var r = 0;
                var flag = int.TryParse(value.ToString(), out r);
                //var temp = value.ToString().Trim();
                if (flag)
                {
                    if (r <= 0)
                        return false;
                    else
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
    /// 字符串全是数字，可以不传
    /// </summary>
    public class IntString2ValidateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            //this.ErrorMessage = "密码应为6~16位英文字母、数字";
            if (value != null)
            {
                var RegexStr = @"^\d+$";
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

    /// <summary>
    /// 字符串全是数字，必填
    /// </summary>
    public class IntStringValidateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            //this.ErrorMessage = "密码应为6~16位英文字母、数字";
            if (value != null)
            {
                var RegexStr = @"^\d+$";
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
    /// 密码
    /// </summary>
    //public class PwdValidateAttribute : ValidationAttribute
    //{
    //    public override bool IsValid(object value)
    //    {
    //        this.ErrorMessage = "密码应为6~16位英文字母、数字";
    //        //if (value != null)
    //        //{
    //        //    var RegexStr = StringHelperHere.Instance.regPwd;
    //        //    if (Regex.IsMatch(value.ToString(), RegexStr))
    //        //    {
    //        //        return true;
    //        //    }
    //        //    else
    //        //    {
    //        //        return false;
    //        //    }
    //        //}
    //        //else
    //        //{
    //        //    return false;
    //        //}
    //        return false;
    //    }
    //}

}