using System;
using System.Collections.Generic;
using System.Text;

namespace VaCant.Common
{
    /// <summary>
    /// Guid帮助类
    /// </summary>
    public class GuidHelper
    {
        public static string GetGuidString()
        {
           Guid guid = Guid.NewGuid();
           return guid.ToString();
        }

        public static string GetGuidString(int length)
        {
            Guid guid = Guid.NewGuid();
            return guid.ToString().Substring(0, length);
        }
    }
}
