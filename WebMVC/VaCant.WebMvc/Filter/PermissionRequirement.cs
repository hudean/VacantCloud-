using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VaCant.WebMvc.Filter
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        /// <summary>
        /// 无权限action
        /// </summary>
        public string DeniedAction { get; set; } = "/Home/visitDeny";

        /// <summary>
        /// 认证授权类型
        /// </summary>
        public string ClaimType { internal get; set; }
        /// <summary>
        /// 默认登录页面
        /// </summary>
        public string LoginPath { get; set; } = "/Home/Login";
        /// <summary>
        /// 过期时间
        /// </summary>
        public TimeSpan Expiration { get; set; }
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="deniedAction"></param>
        /// <param name="claimType"></param>
        /// <param name="expiration"></param>
        public PermissionRequirement(string deniedAction, string claimType, TimeSpan expiration)
        {
            ClaimType = claimType;
            DeniedAction = deniedAction;
            Expiration = expiration;
        }
    }


    public class PermissionItem
    {
        /// <summary>
        /// 用户或角色或其他凭据名称
        /// </summary>
        public virtual string Role { get; set; }
        /// <summary>
        /// 配置的Controller名称
        /// </summary>
        public virtual string controllerName { get; set; }
        /// <summary>
        /// 配置的Action名称
        /// </summary>
        public virtual string actionName { get; set; }
    }
}
