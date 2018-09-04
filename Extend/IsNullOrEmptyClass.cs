using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Extend
{
    /// <summary>
    /// 数据是否为空
    /// </summary>
    public static class IsNullOrEmptyClass
    {
        /// <summary>
        /// Int空值
        /// </summary>
        private readonly static int EmptyInt = 0;
        /// <summary>
        /// DateTime空值
        /// </summary>
        private readonly static DateTime EmptyTime = DateTime.MinValue;
        public static bool IsNullOrEmpty(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return true;
            }
            else if (s == "0" || s == EmptyInt.ToString() || s == EmptyTime.ToString())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsNullOrEmpty(this DateTime s)
        {
            if (s == EmptyTime)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsNullOrEmpty(this double s)
        {
            if (s == EmptyInt)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsNullOrEmpty(this bool s)
        {
            return false;
        }


        public static bool IsNullOrEmpty(this decimal s)
        {
            if (s == EmptyInt)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsNullOrEmpty(this int s)
        {
            if (s == EmptyInt)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsNullOrEmpty(this bool? s)
        {
            if (s == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public static bool IsNullOrEmpty(this int? s)
        {
            if (s == null || s == EmptyInt)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsNullOrEmpty(this double? s)
        {
            if (s == null || s == EmptyInt)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsNullOrEmpty(this DateTime? s)
        {
            if (s == null || s == EmptyTime)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool NotDelete(this bool? s)
        {
            if (s == null || s == false)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool NotDelete(this bool s)
        {
            if (s == false)
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
