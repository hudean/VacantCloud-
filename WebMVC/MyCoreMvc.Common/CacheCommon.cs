using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Caching.Memory;

namespace MyCoreMvc.Common
{
    /// <summary>
    /// 缓存帮助类
    /// wanglq
    /// 2018-9-30
    /// </summary>
    public class CacheCommon
    {
        /// <summary>
        /// 缓存操作类
        /// </summary>
        private static MemoryCache cache = new MemoryCache(new MemoryCacheOptions());
        /// <summary>
        /// 检查缓存是否存在
        /// </summary>
        /// <param name="key">Kay值</param>
        /// <returns></returns>
        public static bool Exists(string key)
        {
            if (!string.IsNullOrEmpty(key))
                return cache.TryGetValue(key, out object val);
            return false;
        }

        #region 获取缓存值
        /// <summary>
        /// 获取缓存值
        /// </summary>
        /// <param name="key">缓存的键</param>
        /// <returns>返回缓存的值</returns>
        public static object GetCache(string key)
        {
            if (key != null && cache.TryGetValue(key, out object val))
                return val;
            else
                return default(object);
        }
        #endregion
        #region 设置缓存值
        /// <summary>
        /// 设置缓存值
        /// </summary>
        /// <param name="key">缓存的键</param>
        /// <param name="value">缓存值</param>
        public static bool SetCache(string key, object value)
        {
            if (key != null)
            {
                cache.Set(key, value);
                return Exists(key);
            }
            return false;
        }
        /// <summary>
        /// 设置缓存（滑动过期）
        /// </summary>
        /// <param name="key">缓存的键</param>
        /// <param name="value">缓存值</param>
        /// <param name="expiresSliding">滑动过期时长（如果在过期时间内有操作，则以当前时间点延长过期时间）</param>
        /// <param name="expiressAbsoulte">绝对过期时长</param>
        /// <returns></returns>
        public static bool SetCache(string key, object value, TimeSpan expiresSliding, TimeSpan expiressAbsoulte)
        {
            if (!string.IsNullOrEmpty(key))
            {
                cache.Set(key, value,
                    new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(expiresSliding)
                    .SetAbsoluteExpiration(expiressAbsoulte)
                    );
                return Exists(key);
            }
            return false;
        }
        /// <summary>
        /// 设置缓存（过期时间，是否滑动过期）
        /// </summary>
        /// <param name="key">缓存的键</param>
        /// <param name="value">缓存值</param>
        /// <param name="expiresIn">缓存时长</param>
        /// <param name="isSliding">是否滑动过期（如果在过期时间内有操作，则以当前时间点延长过期时间）</param>
        /// <returns></returns>
        public static bool SetCache(string key, object value, TimeSpan expiresIn, bool isSliding = false)
        {
            if (!string.IsNullOrEmpty(key))
            {
                if (isSliding)
                    cache.Set(key, value,
                        new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(expiresIn)
                        );
                else
                    cache.Set(key, value,
                    new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(expiresIn)
                    );
                return Exists(key);
            }
            return false;
        }
        #endregion
    }
}
