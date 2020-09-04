using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Filter
{

    public class BaseActionFilter : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            throw new NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            throw new NotImplementedException();
        }
    }


    public class BaseActionFilterAsync : Attribute, IAsyncActionFilter
    {
        public async virtual Task OnActionExecuting(ActionExecutingContext context)
        {
            await Task.CompletedTask;
        }

        public async virtual Task OnActionExecuted(ActionExecutedContext context)
        {
            await Task.CompletedTask;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await OnActionExecuting(context);
            if (context.Result == null)
            {
                var nextContext = await next();
                await OnActionExecuted(nextContext);
            }
        }

        /// <summary>
        /// 返回JSON
        /// </summary>
        /// <param name="json">json字符串</param>
        /// <returns></returns>
        public ContentResult JsonContent(string json)
        {
            return new ContentResult { Content = json, StatusCode = 200, ContentType = "application/json; charset=utf-8" };
        }

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <returns></returns>
        public ContentResult Success()
        {
            AjaxResult res = new AjaxResult
            {
                Success = true,
                Msg = "请求成功！"
            };

            return JsonContent(res.ToJson());
        }

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        public ContentResult Success(string msg)
        {
            AjaxResult res = new AjaxResult
            {
                Success = true,
                Msg = msg
            };

            return JsonContent(res.ToJson());
        }

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="data">返回的数据</param>
        /// <returns></returns>
        public ContentResult Success<T>(T data)
        {
            AjaxResult<T> res = new AjaxResult<T>
            {
                Success = true,
                Msg = "请求成功！",
                Data = data
            };

            return JsonContent(res.ToJson());
        }

        /// <summary>
        /// 返回错误
        /// </summary>
        /// <returns></returns>
        public ContentResult Error()
        {
            AjaxResult res = new AjaxResult
            {
                Success = false,
                Msg = "请求失败！"
            };

            return JsonContent(res.ToJson());
        }

        /// <summary>
        /// 返回错误
        /// </summary>
        /// <param name="msg">错误提示</param>
        /// <returns></returns>
        public ContentResult Error(string msg)
        {
            AjaxResult res = new AjaxResult
            {
                Success = false,
                Msg = msg,
            };

            return JsonContent(res.ToJson());
        }

        /// <summary>
        /// 返回错误
        /// </summary>
        /// <param name="msg">错误提示</param>
        /// <param name="errorCode">错误代码</param>
        /// <returns></returns>
        public ContentResult Error(string msg, int errorCode)
        {
            AjaxResult res = new AjaxResult
            {
                Success = false,
                Msg = msg,
                ErrorCode = errorCode
            };

            return JsonContent(res.ToJson());
        }
    }


    /// <summary>
    /// Ajax请求结果
    /// </summary>
    public class AjaxResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; } = true;

        /// <summary>
        /// 错误代码
        /// </summary>
        public int ErrorCode { get; set; }

        /// <summary>
        /// 返回消息
        /// </summary>
        public string Msg { get; set; }
    }


    /// <summary>
    /// Ajax请求结果
    /// </summary>
    public class AjaxResult<T> : AjaxResult
    {
        /// <summary>
        /// 返回数据
        /// </summary>
        public T Data { get; set; }
    }
}
