using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helper
{
    public class RandHelper:SingleTon<RandHelper>
    {
        #region 生成随机数字
        /// <summary>  
        /// 生成随机数字  
        /// </summary>  
        /// <param name="length">生成长度</param>  
        public string Number(int Length)
        {
            return Number(Length, false);
        }

        /// <summary>  
        /// 生成随机数字  
        /// </summary>  
        /// <param name="Length">生成长度</param>  
        /// <param name="Sleep">是否要在生成前将当前线程阻止以避免重复</param>  
        public string Number(int Length, bool Sleep)
        {
            if (Sleep) System.Threading.Thread.Sleep(3);
            string result = "";
            System.Random random = new Random();
            for (int i = 0; i < Length; i++)
            {
                result += random.Next(10).ToString();
            }
            return result;
        }
        #endregion

        #region 生成随机字母与数字
        /// <summary>  
        /// 生成随机字母与数字  
        /// </summary>  
        /// <param name="IntStr">生成长度</param>  
        public string Str(int Length)
        {
            return Str(Length, false);
        }

        /// <summary>  
        /// 生成随机字母与数字  
        /// </summary>  
        /// <param name="Length">生成长度</param>  
        /// <param name="Sleep">是否要在生成前将当前线程阻止以避免重复</param>  
        public string Str(int Length, bool Sleep)
        {
            if (Sleep) System.Threading.Thread.Sleep(3);
            char[] Pattern = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            string result = "";
            int n = Pattern.Length;
            System.Random random = new Random(~unchecked((int)DateTime.Now.Ticks));
            for (int i = 0; i < Length; i++)
            {
                int rnd = random.Next(0, n);
                result += Pattern[rnd];
            }
            return result;
        }
        #endregion

        #region 生成随机纯字母随机数
        /// <summary>  
        /// 生成随机纯字母随机数  
        /// </summary>  
        /// <param name="IntStr">生成长度</param>  
        public string Str_char(int Length)
        {
            return Str_char(Length, false);
        }

        /// <summary>  
        /// 生成随机纯字母随机数  
        /// </summary>  
        /// <param name="Length">生成长度</param>  
        /// <param name="Sleep">是否要在生成前将当前线程阻止以避免重复</param>  
        public string Str_char(int Length, bool Sleep)
        {
            if (Sleep) System.Threading.Thread.Sleep(3);
            char[] Pattern = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            string result = "";
            int n = Pattern.Length;
            System.Random random = new Random(~unchecked((int)DateTime.Now.Ticks));
            for (int i = 0; i < Length; i++)
            {
                int rnd = random.Next(0, n);
                result += Pattern[rnd];
            }
            return result;
        }
        #endregion


        /// <summary>
        /// 生成一个指定范围的随机整数，该随机数范围包括最小值，但不包括最大值
        /// </summary>
        /// <param name="minNum">最小值</param>
        /// <param name="maxNum">最大值</param>
        public int GetRandomInt(int minNum, int maxNum)
        {
            return new Random().Next(minNum, maxNum);
        }
    }
}
