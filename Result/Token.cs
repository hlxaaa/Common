using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Result
{
    [Serializable]
    public class Token
    {
        private string secret = ConfigurationManager.AppSettings["secret"];
        public Token()
        {
            Header = new Header();
            Payload = new Payload();
            Header.alg = "MD5";
            Header.typ = "JWT";
        }

        public Token(string tokenString)
        {
            Header = new Header();
            Payload = new Payload();
            Header.alg = "MD5";
            Header.typ = "JWT";
            Validate(tokenString);
        }
        /// <summary>
        /// 头部
        /// </summary>
        private Header Header { get; set; }
        /// <summary>
        /// Token内容
        /// </summary>
        public Payload Payload { get; set; }
        /// <summary>
        /// 获取Token
        /// </summary>
        public string GetToken()
        {
            string result = null;
            if (Header == null || Payload == null)
            {
                return result;
            }
            var HerderString = Common.Helper.Base64Helper.EncodeBase64(JsonConvert.SerializeObject(Header));
            var PayloadString = Common.Helper.Base64Helper.EncodeBase64(JsonConvert.SerializeObject(Payload));
            result = HerderString + "." + PayloadString;
            var SignatureString = Common.Helper.MD5Helper.Instance.StrToMD5(result + secret);
            result = result + "." + SignatureString;
            return result;
        }
        /// <summary>
        /// 验证Token是否正常
        /// </summary>
        /// <param name="tokenString">Token</param>
        public bool Validate(string tokenString)
        {
            string[] Token_Part = tokenString.Split('.');
            if (Token_Part.Count() != 3)
            {
                return false;
            }
            else
            {
                var SignatureString = Common.Helper.MD5Helper.Instance.StrToMD5(Token_Part[0] + "." + Token_Part[1] + secret);
                if (!(SignatureString == Token_Part[2]))
                {
                    return false;
                }
                var header = JsonConvert.DeserializeObject<Header>(Common.Helper.Base64Helper.DecodeBase64(Token_Part[0]));
                var payload = JsonConvert.DeserializeObject<Payload>(Common.Helper.Base64Helper.DecodeBase64(Token_Part[1]));
                Header = header;
                Payload = payload;
            }
            return true;
        }

    }
    [Serializable]
    public class Header
    {
        /// <summary>
        /// Token类型
        /// </summary>
        public string typ { get; set; }
        /// <summary>
        /// 使用算法
        /// </summary>
        public string alg { get; set; }
    }
    [Serializable]
    public class Payload
    {
        /// <summary>
        /// 过期时间
        /// </summary>
        public string exp { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

    }
}
