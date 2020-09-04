using ServiceStack.Host;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;

namespace WebApi.Filter
{
    /// <summary>
    /// 基于http basic认证   
    /// </summary>
    public class AuthorizationFilter : AuthorizationFilterAttribute
    {
        //重写OnAuthorization 方法

        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {            //如果action带有允许匿名访问的特性，则直接返回，不再进行安全认证
            if (actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any())
            {
                return;
            }
            if (actionContext.Request.Headers.Authorization != null)
            {
                if (actionContext.Request.Headers.Authorization.Scheme != "Basic")
                {
                    actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, new HttpException("no token"));
                }
                else
                {
                    string base64Para = actionContext.Request.Headers.Authorization.Parameter;
                    //解码base64字符串
                    byte[] buffer = Convert.FromBase64String(base64Para);
                    string decodeBase64 = Encoding.UTF8.GetString(buffer);
                    if (!string.IsNullOrEmpty(decodeBase64))
                    {
                        string[] paras = decodeBase64.Split(':');
                        if (paras.Length > 0)
                        {
                            string userName = paras[0]; string pwd = paras[1]; if (userName == "wolfy" && pwd == "123456")
                            {
                            }
                            else
                            {
                                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, new HttpException("userName or pwd is error."));
                            }
                        }
                        else
                        {
                            actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, new HttpException("no token"));
                        }

                    }
                    else
                    {
                        actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, new HttpException("no token"));
                    }
                }

            }
            else
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized,
                 new HttpException("no Authorization header"));
            }
            base.OnAuthorization(actionContext);
        }
    }
}
