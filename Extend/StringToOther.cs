using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common.Extend
{
    public static class StringConvert
    {
        /// <summary>
        /// String转换int
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int? ParseInt(this string s)
        {
            int result;
            if (int.TryParse(s, out result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// String转换DateTime
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static DateTime? ParseDateTime(this string s)
        {
            DateTime result;
            if (DateTime.TryParse(s, out result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// String转换double
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static double? ParseDouble(this string s)
        {
            double result;
            if (double.TryParse(s, out result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// String转换Bool
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool? ParseBool(this string s)
        {
            bool result;
            if (bool.TryParse(s, out result))
            {
                return result;
            }
            else
            {
                if (s == "0")
                {
                    return false;
                }
                else if (s == "1")
                {
                    return true;
                }
                return null;
            }
        }

        /// <summary>
        /// List<string>转换string
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ParseString(this List<string> list)
        {
            string result = "";
            foreach (var item in list)
            {
                if (item.IsNullOrEmpty())
                    continue;
                result = "," + result;
            }
            return result.Substring(1);
        }

        /// <summary>
        /// List<string>转换string
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ParseString(this List<int> list)
        {
            string result = "";
            foreach (var item in list)
            {
                if (item.IsNullOrEmpty())
                    continue;
                result = "," + item + result;
            }
            return result.Substring(1);
        }

        /// <summary>
        /// Enum转换为String
        /// </summary>
        public static string GetStringValue(this Enum value)
        {
            Type type = value.GetType();
            FieldInfo fi = type.GetField(value.ToString());
            var customAttribute = fi.CustomAttributes.Where(p => p.AttributeType.Name == "EnumStringValueAttribute").ToList().FirstOrDefault();
            if (customAttribute != null)
            {
                var name = customAttribute.NamedArguments.Where(p => p.MemberName == "Value").Select(p => p.TypedValue.Value).FirstOrDefault();
                if (name != null)
                    return name.ToString();
            }
            return null;
        }
    }
}
