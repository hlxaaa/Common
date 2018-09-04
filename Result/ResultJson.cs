using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Common.Result
{
    public class ResultJson
    {
        public ResultJson()
        {
            HttpCode = 200;
        }
        public ResultJson(string msg)
        {
            HttpCode = 200;
            Message = msg;
        }
        /// <summary>
        /// 编码
        /// </summary>
        public int HttpCode { get; set; }
        /// <summary>
        /// 信息
        /// </summary>
        public string Message { get; set; }
    }

    public class ResultJson<T1>
    {
        public ResultJson()
        {
            HttpCode = 200;
            ListData = new List<T1>();
        }

        public ResultJson(string msg)
        {
            HttpCode = 200;
            ListData = new List<T1>();
            Message = msg;
        }
        /// <summary>
        /// 编码
        /// </summary>
        public int HttpCode { get; set; }
        /// <summary>
        /// 信息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 数据列表1
        /// </summary>
        public List<T1> ListData { get; set; }
        //public int index { get; set; }
        //public int pages { get; set; }
        //public List<string> sort { get; set; }
        //public string lat { get; set; }
        //public string lng { get; set; }
        //public int level { get; set; }
    }

    public class ResultWeb<T1>
    {
        public ResultWeb()
        {
            HttpCode = 200;
            ListData = new List<T1>();
        }
        /// <summary>
        /// 编码
        /// </summary>
        public int HttpCode { get; set; }
        /// <summary>
        /// 信息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 数据列表1
        /// </summary>
        public List<T1> ListData { get; set; }
        public int index { get; set; }
        public int pages { get; set; }
        //public List<string> sort { get; set; }
        //public string lat { get; set; }
        //public string lng { get; set; }
        //public int level { get; set; }
    }

    public class ResultJson<T1, T2>
    {
        public ResultJson()
        {
            ListData = new List<T1>();
            ListData2 = new List<T2>();
        }
        /// <summary>
        /// 编码
        /// </summary>
        public int HttpCode { get; set; }
        /// <summary>
        /// 信息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 数据列表1
        /// </summary>
        public List<T1> ListData { get; set; }
        /// <summary>
        /// 数据列表2
        /// </summary>
        public List<T2> ListData2 { get; set; }
    }

    public class ResultJson<T1, T2, T3>
    {
        public ResultJson()
        {
            ListData = new List<T1>();
            ListData2 = new List<T2>();
            ListData3 = new List<T3>();
        }
        /// <summary>
        /// 编码
        /// </summary>
        public int HttpCode { get; set; }
        /// <summary>
        /// 信息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 数据列表1
        /// </summary>
        public List<T1> ListData { get; set; }
        /// <summary>
        /// 数据列表2
        /// </summary>
        public List<T2> ListData2 { get; set; }
        /// <summary>
        /// 数据列表3
        /// </summary>
        public List<T3> ListData3 { get; set; }
    }

    public class ResultJson<T1, T2, T3, T4>
    {
        public ResultJson()
        {
            ListData = new List<T1>();
            ListData2 = new List<T2>();
            ListData3 = new List<T3>();
            ListData4 = new List<T4>();
        }
        /// <summary>
        /// 编码
        /// </summary>
        public int HttpCode { get; set; }
        /// <summary>
        /// 信息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 数据列表1
        /// </summary>
        public List<T1> ListData { get; set; }
        /// <summary>
        /// 数据列表2
        /// </summary>
        public List<T2> ListData2 { get; set; }
        /// <summary>
        /// 数据列表3
        /// </summary>
        public List<T3> ListData3 { get; set; }
        /// <summary>
        /// 数据列表4
        /// </summary>
        public List<T4> ListData4 { get; set; }
    }

    public class ResultJsonModel
    {
        /// <summary>
        /// 编码
        /// </summary>
        public int HttpCode { get; set; }
        /// <summary>
        /// 信息
        /// </summary>
        public string Message { get; set; }
    }

    public class ResultJsonModel<T1>
    {
        /// <summary>
        /// 编码
        /// </summary>
        public int HttpCode { get; set; }
        /// <summary>
        /// 信息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 模型1
        /// </summary>
        public T1 Model1 { get; set; }
    }

    public class ResultJsonModel<T1, T2>
    {
        /// <summary>
        /// 编码
        /// </summary>
        public int HttpCode { get; set; }
        /// <summary>
        /// 信息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 模型1
        /// </summary>
        public T1 Model1 { get; set; }
        /// <summary>
        /// 模型2
        /// </summary>
        public T2 Model2 { get; set; }
    }

    public class ResultJsonModel<T1, T2, T3>
    {
        /// <summary>
        /// 编码
        /// </summary>
        public int HttpCode { get; set; }
        /// <summary>
        /// 信息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 模型1
        /// </summary>
        public T1 Model1 { get; set; }
        /// <summary>
        /// 模型2
        /// </summary>
        public T2 Model2 { get; set; }
        /// <summary>
        /// 模型3
        /// </summary>
        public T3 Model3 { get; set; }
    }

    public class ResultJsonModel<T1, T2, T3, T4>
    {
        /// <summary>
        /// 编码
        /// </summary>
        public int HttpCode { get; set; }
        /// <summary>
        /// 信息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 模型1
        /// </summary>
        public T1 Model1 { get; set; }
        /// <summary>
        /// 模型2
        /// </summary>
        public T2 Model2 { get; set; }
        /// <summary>
        /// 模型3
        /// </summary>
        public T3 Model3 { get; set; }
        /// <summary>
        /// 模型4
        /// </summary>
        public T4 Model4 { get; set; }
    }

    public class ErrorTips
    {
        public string errorClass { get; set; }
        public string tips { get; set; }
    }


    /// <summary>
    /// 路径枚举
    /// </summary>
    public enum Enum_URL
    {
        /// <summary>
        /// 用户图片路径
        /// </summary>
        UserImage = 0,
    }

    public class Fieid
    {
        public Fieid()
        {
            Primary123 = Enum_URL.UserImage;
        }
        /// <summary>
        /// 字段名称
        /// </summary>
        public string Fieid_Name { get; set; }

        /// <summary>
        /// 字段长度
        /// </summary>
        public int? Fieid_Length { get; set; }

        /// <summary>
        /// 是否允许数据库为空
        /// </summary>
        public bool? AllowDBNull { get; set; }

        /// <summary>
        /// 字段类型
        /// </summary>
        public string Fieid_Type { get; set; }

        /// <summary>
        /// 是否是主键
        /// </summary>
        public bool? PrimaryKey { get; set; }

        /// <summary>
        /// 是否是主键
        /// </summary>
        public Enum_URL? Primary123 { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }
    }

    public class Field_Remark
    {
        public string field { get; set; }
        public string remark { get; set; }
    }
}
