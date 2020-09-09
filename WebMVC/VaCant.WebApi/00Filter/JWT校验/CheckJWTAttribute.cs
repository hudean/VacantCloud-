using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VaCant.WebApi.Filter.JWT校验
{
    /// <summary>
    /// JWT校验
    /// </summary>
    public class CheckJWTAttribute : BaseActionFilterAsync
    {
        private static readonly int _errorCode = 401;

        public override async Task OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ContainsFilter<NoCheckJWTAttribute>())
                return;

            try
            {
                var req = context.HttpContext.Request;

                string token = req.GetToken();
                if (token.IsNullOrEmpty())
                {
                    context.Result = Error("缺少token", _errorCode);
                    return;
                }

                //if (!JWTHelper.CheckToken(token, JWTHelper.JWTSecret))
                //{
                //    context.Result = Error("token校验失败!", _errorCode);
                //    return;
                //}

                //var payload = JWTHelper.GetPayload<JWTPayload>(token);
                //if (payload.Expire < DateTime.Now)
                //{
                //    context.Result = Error("token过期!", _errorCode);
                //    return;
                //}
            }
            catch (Exception ex)
            {
                context.Result = Error(ex.Message, _errorCode);
            }

            await Task.CompletedTask;
        }
    }


    /// <summary>
    /// 拓展类
    /// </summary>
    public static partial class Extention
    {
        /// <summary>
        /// 是否拥有某过滤器
        /// </summary>
        /// <typeparam name="T">过滤器类型</typeparam>
        /// <param name="actionExecutingContext">上下文</param>
        /// <returns></returns>
        public static bool ContainsFilter<T>(this FilterContext actionExecutingContext)
        {
            return actionExecutingContext.Filters.Any(x => x.GetType() == typeof(T));
        }

        /// <summary>
        /// 获取Token
        /// </summary>
        /// <param name="req">请求</param>
        /// <returns></returns>
        public static string GetToken(this HttpRequest req)
        {
            string tokenHeader = req.Headers["Authorization"].ToString();
            if (tokenHeader.IsNullOrEmpty())
                return null;

            string pattern = "^Bearer (.*?)$";
            if (!Regex.IsMatch(tokenHeader, pattern))
                throw new Exception("token格式不对!格式为:Bearer {token}");

            string token = Regex.Match(tokenHeader, pattern).Groups[1]?.ToString();
            if (token.IsNullOrEmpty())
                throw new Exception("token不能为空!");

            return token;
        }

        /// <summary>
        /// 判断是否为Null或者空
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this object obj)
        {
            if (obj == null)
                return true;
            else
            {
                string objStr = obj.ToString();
                return string.IsNullOrEmpty(objStr);
            }
        }
    }
}
