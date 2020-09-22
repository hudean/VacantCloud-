using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VaCant.WebMvc.Filter
{
    /// <summary>
    /// 这个Attribute可以应用到控制器或者方法上，而且可以添加多个
    /// This attribute is used on an action of an MVC <see cref="Controller"/>
    /// to make that action usable only by authorized users. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class VaCantMvcAuthorizeAttribute : AuthorizeAttribute, IAbpAuthorizeAttribute
    {
        /// <inheritdoc/>
        public string[] Permissions { get; set; }

        /// <inheritdoc/>
        public bool RequireAllPermissions { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="VaCantMvcAuthorizeAttribute"/> class.
        /// </summary>
        /// <param name="permissions">A list of permissions to authorize</param>
        public VaCantMvcAuthorizeAttribute(params string[] permissions)
        {
            Permissions = permissions;
        }


      
    }


    /// <summary>
    /// Defines standard interface for authorization attributes.
    /// </summary>
    public interface IAbpAuthorizeAttribute
    {
        /// <summary>
        /// A list of permissions to authorize.
        /// </summary>
        string[] Permissions { get; }

        /// <summary>
        /// If this property is set to true, all of the <see cref="Permissions"/> must be granted.
        /// If it's false, at least one of the <see cref="Permissions"/> must be granted.
        /// Default: false.
        /// </summary>
        bool RequireAllPermissions { get; set; }
    }
}
