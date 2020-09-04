using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMVC.Filter
{
    public class MyActionFilter : IActionFilter
    {
        /// <summary>
        ///  Action执行之后执行
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///  Action执行之前执行
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            string userId = context.HttpContext.Session.GetString("LoginUserId");
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
            //判断权限

            //获取action名字
            var controller = context.RouteData.Values["controller"]?.ToString();
            var action = context.RouteData.Values["action"]?.ToString();

            //查询该账号的对应权限没有


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
}
