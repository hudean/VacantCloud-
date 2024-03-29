﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using VaCant.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VaCant.WebMvc.Filter
{
    /// <summary>
    /// 登录校验权限过滤器
    /// </summary>
    public class CheckLoginAuthorizeFilter : Attribute, IAuthorizationFilter
    {
        //参考文章 https://www.cnblogs.com/yaopengfei/p/11232921.html

        /// <summary>
        /// 权限过滤器
        /// </summary>
        /// <param name="context"></param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //匿名标识 无需验证
            if (context.Filters.Any(e => (e as AllowAnonymous) != null))
                return;

            string userId = context.HttpContext.Session.GetString("LoginUserId");
            string userName = context.HttpContext.Session.GetString("LoginUserName");
           
            if (string.IsNullOrEmpty(userId))
            {
                if (IsAjaxRequest(context.HttpContext.Request))
                {
                    //是ajax请求
                    context.Result = new JsonResult(new { status = "error", message = "你没有登录" });
                }
                else
                {
                    var result = new RedirectResult("~/Account/Login");
                    context.Result = result;
                }
                return;
            }
            if (!string.IsNullOrEmpty(userName) && userName.Equals("admin"))
            {
                return;
            }

            //对应action方法或者Controller上若存在NonePermissionAttribute标识，即表示为管理员的默认权限,只要登录就有权限
            var isNone = context.Filters.Any(e => (e as NonePermissionAttribute) != null);
            if (isNone)
                return;

            //获取请求的区域，控制器，action名称
            var area = context.RouteData.DataTokens["area"]?.ToString();
            var controller = context.RouteData.Values["controller"]?.ToString();
            var action = context.RouteData.Values["action"]?.ToString();
            var isPermit = false;
            //校验权限
            //isPermit = ServiceFactory.CheckAdminPermit(adminInfo.Id, area, controller, action);
            if (isPermit)
                return;

            //此action方法的父辈权限判断，只要有此action对应的父辈权限，皆有权限访问
            var pAttrs = context.Filters.Where(e => (e as ParentPermissionAttribute) != null).ToList();
            if (pAttrs.Count > 0)
            {
                foreach (ParentPermissionAttribute pattr in pAttrs)
                {
                    if (!string.IsNullOrEmpty(pattr.Area))
                        area = pattr.Area;
                    //isPermit = ServiceFactory.CheckAdminPermit(adminInfo.Id, area, pattr.Controller, pattr.Action);
                    if (isPermit)
                        return;
                }
            }
            if (!isPermit)
            {
                context.Result = new ContentResult() { Content = "无权限访问" };
                return;
            }

            #region 另一种校验思路

            //string allowAttrName = typeof(AllowAnonymousAttribute).ToString();
            //string attrName = typeof(CheckPermissionAttribute).ToString();
            ////所有目标对象上所有特性
            //var data = context.ActionDescriptor.EndpointMetadata.ToList();
            //bool isHasAttr = false;
            ////循环比对是否含有skip特性
            ////如果action带有允许匿名访问的特性，则直接返回，不再进行安全认证
            //if (data.Any(r => r.ToString().Equals(allowAttrName, StringComparison.OrdinalIgnoreCase)))
            //{
            //    return;
            //}

            //data.ForEach(i => i.ToString().Equals(attrName, StringComparison.OrdinalIgnoreCase));
            //foreach (var item in data)
            //{
            //    if (data.ToString().Equals(attrName))
            //    {
            //        isHasAttr = true;
            //    }
            //}

            ////1. 校验是否标记跨过登录验证
            //if (isHasAttr)
            //{
            //    //表示该方法或控制器跨过登录验证
            //    //继续走控制器中的业务即可
            //}
            //else
            //{
            //    //2.判断是什么请求，进行响应的页面跳转
            //    //PS：根据request.Headers["X-Requested-With"]是否包含XMLHttpRequest来判断是不是ajax请求。
            //    if (IsAjaxRequest(context.HttpContext.Request))
            //    {
            //        //是ajax请求
            //        context.Result = new JsonResult(new { status = "error", message = "你没有权限" });
            //    }
            //    else
            //    {
            //        var result = new ViewResult { ViewName = "~/Views/Shared/Error.cshtml" };
            //        context.Result = result;
            //    }

            //}

            #endregion




            //获取区域、控制器、Action的名称
            //方法1
            //context.ActionDescriptor.RouteValues["area"].ToString();
            //context.ActionDescriptor.RouteValues["controller"].ToString();
            //context.ActionDescriptor.RouteValues["action"].ToString();
            //方法2
            //context.RouteData.Values["controller"].ToString();
            //context.RouteData.Values["action"].ToString();
            //string actionName = context.RouteData.Values["action"].ToString();
            //if (context.HttpContext.User.Identity.Name != "admin")
            //{
            //    //未通过验证则跳转到无权限提示页
            //    RedirectToActionResult content = new RedirectToActionResult("NoAuth", "Exception", null);
            //    context.Result = content;
            //}

            #region .net fraemwork 版本
            ////获得当前要执行的Action上标注的CheckPermissionAttribute实例对象
            //CheckPermissionAttribute[] permAtts = (CheckPermissionAttribute[])filterContext.ActionDescriptor.GetCustomAttributes(typeof(CheckPermissionAttribute), false);
            //if (permAtts.Length <= 0)//没有标注任何的CheckPermissionAttribute，因此也就不需要检查是否登录
            //                         //“无欲无求”
            //{
            //    return;//登录等这些不要求有用户登录的功能
            //}
            ////得到当前登录用户的id
            //long? userId = (long?)filterContext.HttpContext.Session["LoginUserId"];
            //if (userId == null)//连登录都没有，就不能访问
            //{
            //    // filterContext.HttpContext.Response.Write("没有登录");
            //    //filterContext.HttpContext.Response.Redirect();

            //    //根据不同的请求，给予不同的返回格式。确保ajax请求，浏览器端也能收到json格式
            //    if (filterContext.HttpContext.Request.IsAjaxRequest())
            //    {
            //        AjaxResult ajaxResult = new AjaxResult();
            //        ajaxResult.Status = "redirect";
            //        ajaxResult.Data = "/Main/Login";
            //        ajaxResult.ErrorMsg = "没有登录";
            //        filterContext.Result = new JsonNetResult { Data = ajaxResult };
            //    }
            //    else
            //    {
            //        filterContext.Result = new RedirectResult("~/Main/Login");
            //    }
            //    //filterContext.Result = new ContentResult() { Content= "没有登录" };
            //    return;
            //}
            ////由于ZSZAuthorizeFilter不是被autofac创建，因此不会自动进行属性的注入
            ////需要手动获取Service对象
            //IAdminUserService userService =
            //    DependencyResolver.Current.GetService<IAdminUserService>();

            ////检查是否有权限
            //foreach (var permAtt in permAtts)
            //{
            //    //判断当前登录用户是否具有permAtt.Permission权限
            //    //(long)userId   userId.Value
            //    if (!userService.HasPermission(userId.Value, permAtt.Permission))
            //    {
            //        //只要碰到任何一个没有的权限，就禁止访问
            //        //在IAuthorizationFilter里面，只要修改filterContext.Result 
            //        //那么真正的Action方法就不会执行了
            //        if (filterContext.HttpContext.Request.IsAjaxRequest())
            //        {
            //            AjaxResult ajaxResult = new AjaxResult();
            //            ajaxResult.Status = "error";
            //            ajaxResult.ErrorMsg = "没有权限" + permAtt.Permission;
            //            filterContext.Result = new JsonNetResult { Data = ajaxResult };
            //        }
            //        else
            //        {
            //            filterContext.Result
            //           = new ContentResult { Content = "没有" + permAtt.Permission + "这个权限" };
            //        }
            //        return;
            //    }
            //}
            #endregion
        }

        /// <summary>
        /// 判断是否是ajax请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private bool IsAjaxRequest(HttpRequest request)
        {
            string header = request.Headers["X-Requested-With"];
            return "XMLHttpRequest".Equals(header);
        }
    }



    /// <summary>
    /// 管理员的默认权限
    /// </summary>
    public class NonePermissionAttribute : Attribute, IFilterMetadata { }

    /// <summary>
    /// 匿名验证
    /// </summary>
    public class AllowAnonymous : Attribute, IFilterMetadata { }

    /// <summary>
    /// 长辈权限
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class ParentPermissionAttribute : Attribute, IFilterMetadata
    {
        /// <summary>
        /// 区域
        /// </summary>
        public string Area { get; set; }
        /// <summary>
        /// 控制器
        /// </summary>
        public string Controller { get; set; }
        /// <summary>
        /// Action名称
        /// </summary>
        public string Action { get; set; }

        public ParentPermissionAttribute(string area, string controller, string action)
        {
            this.Area = area;
            this.Controller = controller;
            this.Action = action;
        }

        public ParentPermissionAttribute(string controller, string action)
        {
            this.Controller = controller;
            this.Action = action;
        }
    }
}
