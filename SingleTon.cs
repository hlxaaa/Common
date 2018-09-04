using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// 单例模式
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SingleTon<T> where T : class, new()
    {
        private static object obj = new object();
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (obj)
                    {
                        if (_instance == null)
                        {
                            _instance = new T();
                            return _instance;
                        }
                        else
                        {
                            return _instance;
                        }
                    }
                }
                else
                {
                    return _instance;
                }
            }
        }
    }
}
