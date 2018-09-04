using BeIT.MemCached;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Common.Helper
{
    public static class MemCacheHelper
    {
        private static MemcachedClient Cache;
        private static object cacheLock = new object();

        private static MemcachedClient OtherCache;
        private static object OtherCacheLock = new object();

        public static MemcachedClient GetMyConfigInstance()
        {
            if (Cache == null)
            {
                lock (cacheLock)
                {
                    if (Cache == null)
                    {
                        Cache = MemcachedClient.GetInstance("MyConfigFileCache");
                        Cache.Duration = 3600;
                        Cache.SendReceiveTimeout = 5000;
                        Cache.ConnectTimeout = 5000;
                        Cache.MinPoolSize = 1;
                        Cache.MaxPoolSize = 5;
                        return Cache;
                    }
                    else
                    {
                        return Cache;
                    }
                }

            }
            else
            {
                return Cache;
            }
        }

        public static MemcachedClient GetMyOtherConfigInstance()
        {
            if (OtherCache == null)
            {
                lock (OtherCacheLock)
                {
                    if (OtherCache == null)
                    {
                        OtherCache = MemcachedClient.GetInstance("MyOtherConfigFileCache");
                        OtherCache.Duration = 3600;
                        OtherCache.SendReceiveTimeout = 5000;
                        OtherCache.ConnectTimeout = 5000;
                        OtherCache.MinPoolSize = 1;
                        OtherCache.MaxPoolSize = 5;
                        return OtherCache;
                    }
                    else
                    {
                        return OtherCache;
                    }
                }

            }
            else
            {
                return OtherCache;
            }
        }

    }
}
