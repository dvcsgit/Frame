using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;

namespace PFG.Library.Extension
{
    public static class CacheExtension
    {
        public static T GetOrInsert<T>(this Cache Cache, string key, Func<T> generator)
        {
            var result = Cache[key];
            if (result != null)
                return (T)result;
            result = (generator != null) ? generator() : default(T);
            Cache.Insert(key, result);
            return (T)result;
        }
    }
}
