using System;

namespace MyCoreMvc.Cache
{
    public class CacheFactory
    {
        private static ICache cache = null;
        private static readonly object lockHelper = new object();
    }
}
