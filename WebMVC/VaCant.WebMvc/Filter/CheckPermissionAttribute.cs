using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VaCant.WebMvc.Filter
{
    //这个Attribute可以应用到方法或类上，而且可以添加多个
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class CheckPermissionAttribute : Attribute
    {
        public string[] Permission { get; set; }
        public CheckPermissionAttribute(params string[] permission)
        {
            this.Permission = permission;
        }
    }
}
