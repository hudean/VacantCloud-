using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebMVC.Filter
{
    /// <summary>
    /// 基于授权策略对请求的资源授权限制 使用AuthorizationHandler形式进行过滤
    /// </summary>
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        public IAuthenticationSchemeProvider Schemes;
        readonly IDistributedCache _cacheService;
        /// <summary>
        /// 构造函数注入
        /// </summary>
        public PermissionHandler(IAuthenticationSchemeProvider schemes, IDistributedCache cacheService)
        {
            Schemes = schemes;
            _cacheService = cacheService;
        }
        /// <summary>
        /// 重载异步处理程序
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            //从AuthorizationHandlerContext转成HttpContext，以便取出表求信息
            AuthorizationFilterContext filterContext = context.Resource as AuthorizationFilterContext;
            HttpContext httpContext = filterContext.HttpContext;
            AuthenticateResult result = await httpContext.AuthenticateAsync(Schemes.GetDefaultAuthenticateSchemeAsync().Result.Name);
            //如果没登录result.Succeeded为false
            if (result.Succeeded)
            {
                httpContext.User = result.Principal;
                //当前访问的Controller
                string controllerName = filterContext.RouteData.Values["Controller"].ToString();//通过ActionContext类的RouteData属性获取Controller的名称：Home
                //当前访问的Action
                string actionName = filterContext.RouteData.Values["Action"].ToString();//通过ActionContext类的RouteData属性获取Action的名称：Index
                string name = httpContext.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Name)?.Value;
                string perData = await _cacheService.GetStringAsync("perm" + name);
                List<PermissionItem> lst = JsonConvert.DeserializeObject<List<PermissionItem>>(perData);
                if (lst.Where(w => w.controllerName == controllerName && w.actionName == actionName).Count() > 0)
                {
                    //如果在配置的权限表里正常走
                    context.Succeed(requirement);
                }
                else
                {
                    //不在权限配置表里 做错误提示
                    //如果是AJAX请求 (包含了VUE等 的ajax)
                    string requestType = filterContext.HttpContext.Request.Headers["X-Requested-With"];
                    if (!string.IsNullOrEmpty(requestType) && requestType.Equals("XMLHttpRequest", StringComparison.CurrentCultureIgnoreCase))
                    {
                        //ajax 的错误返回
                        //filterContext.Result = new StatusCodeResult(499); //自定义错误号 ajax请求错误 可以用来错没有权限判断 也可以不写 用默认的
                        context.Fail();
                    }
                    else
                    {
                        //普通页面错误提示 就是跳转一个页面
                        //httpContext.Response.Redirect("/Home/visitDeny");//第一种方式跳转
                        filterContext.Result = new RedirectToActionResult("visitDeny", "Home", null);//第二种方式跳转
                        context.Fail();
                    }
                }
            }
            else
            {
                context.Fail();
            }
        }
    }
}
