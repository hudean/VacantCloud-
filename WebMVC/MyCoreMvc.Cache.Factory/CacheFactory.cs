using MyCoreMvc.Cache.Interface;
using System;

namespace MyCoreMvc.Cache.Factory
{
    public class CacheFactory
    {
        private static ICache cache = null;
        private static readonly object lockHelper = new object();
    }
}
