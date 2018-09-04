using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Common.Helper
{
    public class StringHelper : SingleTon<StringHelper>
    {

        string imgHost = ConfigurationManager.AppSettings["imgHost"];
        /// <summary>
        /// 类似12.23这种，把小数点及其后面的去掉
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public string RemoverAfterDoc(string n)
        {
            var i = n.IndexOf('.');
            if (i < 0)
                return n;
            else return n.Substring(0, i);
        }

        /// <summary>
        /// url后的时间戳去掉
        /// </summary>
        public string RemoveTimestamp(string url)
        {
            var i = url.LastIndexOf('?');
            if (i < 0)
                return url;
            return url.Substring(0, i);
        }

        /// <summary>
        /// 类似65465465.jpg的，不带路径
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public string[] GetOldImgNames(string content)
        {
            if (content == null || content == "")
                return new string[] { };
            string[] imgs = GetImgPath(content);
            for (int i = 0; i < imgs.Length; i++)
            {
                imgs[i] = imgs[i].Substring(imgs[i].LastIndexOf('/') + 1);
            }
            return imgs;
        }

        public string[] GetImgPath(string content)
        {
            List<string> list = new List<string>();
            string[] temp = content.Split('<');
            for (int i = 0; i < temp.Length; i++)
            {
                string result = Regex.Match(temp[i], "(?<=src=\").*?(?=\")").Value;
                if (result != "" && result != null)
                    list.Add(result);
            }
            return list.ToArray();
        }

        /// <summary>
        /// 正则：6~16位英文字母、数字
        /// </summary>
        public string regPwd = @"^[0-9a-zA-z]{6,16}$";

        /// <summary>
        /// 判断密码格式是否合理
        /// </summary>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public bool IsPwdValidate(string pwd)
        {
            Regex reg = new Regex(regPwd);
            return reg.IsMatch(pwd);
        }

        public string GetApiUrl(string url)
        {
            if (url == null)
                return null;
            if (url.Substring(0, 4) == "http")
                return url + "?" + GetTimeStamp();
            //return (imgHost + url + "?" + GetTimeStamp()).Replace("\\\\","//");
            return (imgHost + url).Replace("\\", "/");
        }

        public string UrlSetTimeStamp(string url)
        {
            var i = url.LastIndexOf('?');
            if (i < 0)
            {
                return url + "?" + GetTimeStamp();
            }
            else
            {
                return url.Substring(0, i) + "?" + GetTimeStamp();
            }
        }

        public string GetWebUrl(string url)
        {
            if (url == null)
                return "";
            if (url.Substring(0, 4) == "http")
                return url;
            return "../" + url;
        }

        /// <summary>
        /// 生成订单编号
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public string CreateOrderNo(string meterNo)
        {
            var str = DateTime.Now.ToString("yyyyMMddHHmmss") + meterNo + RandHelper.Instance.Number(3);
            return str;
        }

        /// <summary>
        /// 生成微信需要的xml字符串
        /// </summary>
        /// <returns></returns>
        public string GetWX_XML(string appid, string attach, string body, string mch_id, string nonce_str, string notify_url, string out_trade_no, string spbill_create_ip, string total_fee, string trade_type, string sign)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<xml>");
            sb.Append("<appid>" + appid + "</appid>");
            sb.Append("<attach>" + attach + "</attach>");
            sb.Append("<body>" + body + "</body>");
            sb.Append("<mch_id>" + mch_id + "</mch_id>");
            sb.Append("<nonce_str>" + nonce_str + "</nonce_str>");
            sb.Append("<notify_url>" + notify_url + "</notify_url>");
            sb.Append("<out_trade_no>" + out_trade_no + "</out_trade_no>");
            sb.Append("<spbill_create_ip>" + spbill_create_ip + "</spbill_create_ip>");
            sb.Append("<total_fee>" + total_fee + "</total_fee>");
            sb.Append("<trade_type>" + trade_type + "</trade_type>");
            sb.Append("<sign>" + sign + "</sign>");
            sb.Append("</xml>");

            return sb.ToString();
        }

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        /// <summary>
        /// 获取字符串的最后五位，少则前方补0
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string GetLastFiveStr(string str)
        {
            do
            {
                str = "0" + str;
            } while (str.Length < 5);
            return str.Substring(str.Length - 5, 5);
        }

        public string GetLastStr(string str, int l)
        {
            var L = str.Length;
            if (L > l)
            {
                return str.Substring(L - l);
            }
            return str;
        }

        /// <summary>
        /// 用0补充数字，比如0000001234
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string GetIntStringWithZero(string str, int length)
        {
            do
            {
                str = "0" + str;
            } while (str.Length < length);
            return str.Substring(str.Length - length, length);
        }

        /// <summary>
        /// 数组转字符串
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public string ArrJoin(Array arr)
        {
            var str = "";
            foreach (var item in arr)
            {
                if (item != null)
                    str += item + ",";
            }
            return RemoveLastOne(str);
        }

        public string ArrJoin2(Array arr)
        {
            var str = "";
            foreach (var item in arr)
            {
                if (item != null)
                    str += "'" + item + "',";
            }
            return RemoveLastOne(str);
        }


        /// <summary>
        /// 去掉字符串最后一个字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string RemoveLastOne(string str)
        {
            if (str.Length < 1)
                return "";
            return str.Substring(0, str.Length - 1);
        }

        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="word">被加密字符串</param>
        /// <param name="key">密钥</param>
        /// <returns>加密后字符串</returns>
        public string Encrypt(string word, string key)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(key, "^[a-zA-Z]*$"))
            {
                throw new Exception("key 必须由字母组成");
            }
            key = key.ToLower();
            //逐个字符加密字符串
            char[] s = word.ToCharArray();
            for (int i = 0; i < s.Length; i++)
            {
                char a = word[i];
                char b = key[i % key.Length];
                s[i] = EncryptChar(a, b);
            }
            return new string(s);
        }

        /// <summary>
        /// 加密单个字符
        /// </summary>
        /// <param name="a">被加密字符</param>
        /// <param name="b">密钥</param>
        /// <returns>加密后字符</returns>
        private char EncryptChar(char a, char b)
        {
            int c = a + b - 'a';
            if (a >= '0' && a <= '9') //字符0-9的转换
            {
                while (c > '9') c -= 10;
            }
            else if (a >= 'a' && a <= 'z') //字符a-z的转换
            {
                while (c > 'z') c -= 26;
            }
            else if (a >= 'A' && a <= 'Z') //字符A-Z的转换
            {
                while (c > 'Z') c -= 26;
            }
            else return a; //不再上面的范围内，不转换直接返回
            return (char)c; //返回转换后的字符
        }

        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="word">被解密字符串</param>
        /// <param name="key">密钥</param>
        /// <returns>解密后字符串</returns>
        public string Decrypt(string word, string key)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(key, "^[a-zA-Z]*$"))
            {
                throw new Exception("key 必须由字母组成");
            }
            key = key.ToLower();
            //逐个字符解密字符串
            char[] s = word.ToCharArray();
            for (int i = 0; i < s.Length; i++)
            {
                char a = word[i];
                char b = key[i % key.Length];
                s[i] = DecryptChar(a, b);
            }
            return new string(s);
        }

        /// <summary>
        /// 解密单个字符
        /// </summary>
        /// <param name="a">被解密字符</param>
        /// <param name="b">密钥</param>
        /// <returns>解密后字符</returns>
        private char DecryptChar(char a, char b)
        {
            int c = a - b + 'a';
            if (a >= '0' && a <= '9') //字符0-9的转换
            {
                while (c < '0') c += 10;
            }
            else if (a >= 'a' && a <= 'z') //字符a-z的转换
            {
                while (c < 'a') c += 26;
            }
            else if (a >= 'A' && a <= 'Z') //字符A-Z的转换
            {
                while (c < 'A') c += 26;
            }
            else return a; //不再上面的范围内，不转换直接返回
            return (char)c; //返回转换后的字符
        }

        /// <summary>
        /// 数字转字符串
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        public string IntToKey(string no)
        {
            string str = no;
            char[] s = str.ToCharArray();
            for (int i = 0; i < s.Length; i++)
            {
                s[i] = IntToKeyChar(s[i]);
            }
            return new string(s);
        }

        /// <summary>
        /// 字符串转数字
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string KeyToInt(string key)
        {
            string str = key;
            char[] s = str.ToCharArray();
            for (int i = 0; i < s.Length; i++)
            {
                s[i] = CharToInt(s[i]);
            }
            return new string(s);
        }

        /// <summary>
        /// 数字字符转英文字符
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public char IntToKeyChar(char a)
        {
            switch (a)
            {
                case '0':
                    return 'l';
                case '1':
                    return 'y';
                case '2':
                    return 'e';
                case '3':
                    return 's';
                case '4':
                    return 'a';
                case '5':
                    return 'w';
                case '6':
                    return 'c';
                case '7':
                    return 'q';
                case '8':
                    return 'b';
                case '9':
                    return 'j';
                default:
                    return 'n';
            }
        }

        /// <summary>
        /// 英文字符转数字字符
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public char CharToInt(char a)
        {
            switch (a)
            {
                case 'l':
                    return '0';
                case 'y':
                    return '1';
                case 'e':
                    return '2';
                case 's':
                    return '3';
                case 'a':
                    return '4';
                case 'w':
                    return '5';
                case 'c':
                    return '6';
                case 'q':
                    return '7';
                case 'b':
                    return '8';
                case 'j':
                    return '9';
                default:
                    return 'n';
            }
        }

        /// <summary>
        /// 微信计算签名
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="mchKey"></param>
        /// <returns></returns>
        public string GetWXSign(Dictionary<string, string> dict, string mchKey)
        {
            var signTemp = WXPayHelper.getParamSrc(dict) + "&key=" + mchKey;
            return MD5Helper.Instance.StrToMD5_UTF8(signTemp);
        }

        public string Keep2Decimal(string n)
        {
            var d = Convert.ToDecimal(n);
            int i = (int)(d * 100);
            d = (decimal)(i * 1.0) / 100;
            return d.ToString();
        }

        public string Keep2Decimal(decimal d)
        {
            //var d = Convert.ToDecimal(n);
            int i = (int)(d * 100);
            d = (decimal)(i * 1.0) / 100;
            return d.ToString();
        }


        public string GetContentHtml(string content)
        {
            return content.Replace("*gt;", ">").Replace("*lt;", "<").Replace("*amp", "&");
        }

        /// <summary>
        /// 传入秒数获得  x天x时x分x秒
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public string GetTimeStr(int seconds)
        {
            if (seconds == 0)
                return "";
            var str = "";
            if (seconds >= 86400)
                str += $"{seconds / 86400}天" + GetTimeStr(seconds % 86400);
            else if (seconds >= 3600)
                str += $"{seconds / 3600}时" + GetTimeStr(seconds % 3600);
            else if (seconds >= 60)
                str += $"{seconds / 60}分" + GetTimeStr(seconds % 60);
            else
                str += $"{seconds}秒";
            return str;
        }

        public string GetMinuteSecondStr(int seconds)
        {
            if (seconds == 0)
                return "00:00";
            var minute = seconds / 60;
            var sec = seconds % 60;
            return $"{GetIntStringWithZero(minute.ToString(), 2)}:{GetIntStringWithZero(sec.ToString(), 2)}";
        }

        /// <summary>
        /// 传72:33.得到72分33秒的总秒数
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public int GetSeconds(string time)
        {
            var arr = time.Split(':');
            if (arr[0] == "00")
                return Convert.ToInt32(arr[1]);
            return Convert.ToInt32(arr[0]) * 60 + Convert.ToInt32(arr[1]);
        }

        public string GetBMI(int weight, int height)
        {
            if (height == 0 || weight == 0)
                return "";
            //LogHelper.WriteLog(GetType(), "weight = " + weight);
            //LogHelper.WriteLog(GetType(), "height = " + height);
            return Keep2Decimal(weight / (height * height / 10000));
        }

        /// <summary>
        /// 计算燃脂心率 61次/分～82次/分
        /// </summary>
        public string GetFHR(DateTime dt)
        {
            var age = GetAge(dt);
            return GetFHR(age);
        }

        /// <summary>
        /// 计算燃脂心率 61次/分～82次/分
        /// </summary>
        public string GetFHR(int age)
        {
            return $"{ (220 - age) * 0.6}次/分~{ (220 - age) * 0.8}次/分";
        }

        public int GetAge(DateTime birthdate)
        {
            DateTime now = DateTime.Now;
            int age = now.Year - birthdate.Year;
            if (now.Month < birthdate.Month || (now.Month == birthdate.Month && now.Day < birthdate.Day))
            {
                age--;
            }
            return age < 0 ? 0 : age;

        }
    }
}
