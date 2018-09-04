using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common.Extend;
using System.ComponentModel.DataAnnotations;

namespace Common.Attribute
{
    public class DateTimeValidateAttribute : ValidationAttribute
    {
        /// <summary>
        /// 是否允许为空
        /// </summary>
        public bool AllowEmpty { get; set; }
        public override bool IsValid(object value)
        {
            if (AllowEmpty == true)
            {
                return true;
            }
            if (value == null)
            {
                return false;
            }
            else
            {
                DateTime date = (DateTime)value;
                if (date.IsNullOrEmpty())
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }

    /// <summary>
    /// 时间字符串过滤器，可不传
    /// </summary>
    public class DateTimeValidate2Attribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }
            else
            {
                //DateTime date = (DateTime)value;
                DateTime r = new DateTime();
                var flag = DateTime.TryParse(value.ToString(), out r);
                if (!flag)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }
}