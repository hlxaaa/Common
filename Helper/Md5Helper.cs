using System.Text;
using System.Security.Cryptography;
using System;

namespace Common.Helper
{
    public class MD5Helper : SingleTon<MD5Helper>
    {
        ///   <summary>  
        ///   给一个字符串进行MD5加密  
        ///   </summary>  
        ///   <param   name="strText">待加密字符串</param>  
        ///   <returns>加密后的字符串</returns>  
        public string StrToMD5(string str)
        {
            byte[] data = Encoding.GetEncoding("GB2312").GetBytes(str);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] OutBytes = md5.ComputeHash(data);

            string OutString = "";
            for (int i = 0; i < OutBytes.Length; i++)
            {
                OutString += OutBytes[i].ToString("x2");
            }
            // return OutString.ToUpper();
            return OutString.ToUpper();
        }

        /// <summary>
        /// 加密方式是UTF-8
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string StrToMD5_UTF8(string str)
        {
            byte[] data = Encoding.UTF8.GetBytes(str);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] OutBytes = md5.ComputeHash(data);

            string OutString = "";
            for (int i = 0; i < OutBytes.Length; i++)
            {
                OutString += OutBytes[i].ToString("x2");
            }
            // return OutString.ToUpper();
            return OutString.ToUpper();
        }

        /// <summary>
        /// MD5加密 32位
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string Md5_32(string str)
        {
            byte[] result = Encoding.Default.GetBytes(str);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(result);
            return BitConverter.ToString(output).Replace("-", "");
        }
    }
}
