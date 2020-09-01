using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
//using System.Web.Http.Filters;

namespace WebMVC.Filter
{
    //public class AuthorizationFilter : AuthorizationFilterAttribute
    //{
    //    /// <summary>
    //    /// 使用cookie
    //    /// </summary>
    //    /// <param name="actionContext"></param>
    //    public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
    //    {
    //        //获得当前要执行的Action上标注的CheckPermissionAttribute实例对象
    //        CheckPermissionAttribute[] permAtts =actionContext.ActionDescriptor.GetCustomAttributes<CheckPermissionAttribute>(false).ToArray();
    //        //没有标注任何的CheckPermissionAttribute，因此也就不需要检查是否登录
    //        if (permAtts.Length <= 0)
                                    
    //        {
    //            return;//登录等这些不要求有用户登录的功能
    //        }
    //        //得到当前登录用户的id
    //        //long? userId = (long?)actionContext.HttpContext.Session["LoginUserId"];
    //        string userId = actionContext.Request.Headers.GetCookies("LoginUserId").ToString();
    //        if (userId == null)//连登录都没有，就不能访问
    //        {
    //            // filterContext.HttpContext.Response.Write("没有登录");
    //            //filterContext.HttpContext.Response.Redirect();

    //            ////根据不同的请求，给予不同的返回格式。确保ajax请求，浏览器端也能收到json格式
    //            //if (actionContext.HttpContext.Request.IsAjaxRequest())
    //            //{
    //            //    AjaxResult ajaxResult = new AjaxResult();
    //            //    ajaxResult.Status = "redirect";
    //            //    ajaxResult.Data = "/Main/Login";
    //            //    ajaxResult.ErrorMsg = "没有登录";
    //            //    filterContext.Result = new JsonNetResult { Data = ajaxResult };
    //            //}
    //            //else
    //            //{
    //            //    actionContext.Result = new RedirectResult("~/Main/Login");
    //            //}
    //            //filterContext.Result = new ContentResult() { Content= "没有登录" };
    //            return;
    //        }
    //        //由于ZSZAuthorizeFilter不是被autofac创建，因此不会自动进行属性的注入
    //        //需要手动获取Service对象
    //        //IAdminUserService userService =
    //        //    DependencyResolver.Current.GetService<IAdminUserService>();

    //        //检查是否有权限
    //        //foreach (var permAtt in permAtts)
    //        //{
    //        //    //判断当前登录用户是否具有permAtt.Permission权限
    //        //    //(long)userId   userId.Value
    //        //    if (!userService.HasPermission(userId.Value, permAtt.Permission))
    //        //    {
    //        //        //只要碰到任何一个没有的权限，就禁止访问
    //        //        //在IAuthorizationFilter里面，只要修改filterContext.Result 
    //        //        //那么真正的Action方法就不会执行了
    //        //        if (actionContext.HttpContext.Request.IsAjaxRequest())
    //        //        {
    //        //            AjaxResult ajaxResult = new AjaxResult();
    //        //            ajaxResult.Status = "error";
    //        //            ajaxResult.ErrorMsg = "没有权限" + permAtt.Permission;
    //        //            filterContext.Result = new JsonNetResult { Data = ajaxResult };
    //        //        }
    //        //        else
    //        //        {
    //        //            filterContext.Result
    //        //           = new ContentResult { Content = "没有" + permAtt.Permission + "这个权限" };
    //        //        }
    //        //        return;
    //        //    }
    //        //}
    //    }
    //}
}
