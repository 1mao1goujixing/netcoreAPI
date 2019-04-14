using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MynetCoreAPI.AOP
{
    public interface ICaching
    {
        object Get(string cacheKey);
        void Set(string cacheKey, object cacheValue);
    }
}
