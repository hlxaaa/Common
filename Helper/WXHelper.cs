using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helper
{
    public class WXHelper : SingleTon<WXHelper>
    {
        /// <summary>
        /// 微信计算签名
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="mchKey"></param>
        /// <returns></returns>
        public string GetWXSign(Dictionary<string, string> dict, string mchKey)
        {
            var signTemp = WXHelper.getParamSrc(dict) + "&key=" + mchKey;
            return MD5Helper.Instance.StrToMD5_UTF8(signTemp);
        }

        public static String getParamSrc(Dictionary<string, string> paramsMap)
        {
            var vDic = (from objDic in paramsMap orderby objDic.Key ascending select objDic);
            StringBuilder str = new StringBuilder();
            foreach (KeyValuePair<string, string> kv in vDic)
            {
                string pkey = kv.Key;
                string pvalue = kv.Value;
                str.Append(pkey + "=" + pvalue + "&");
            }

            String result = str.ToString().Substring(0, str.ToString().Length - 1);
            return result;
        }
    }
}
