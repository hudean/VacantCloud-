using log4net;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VaCant.WebMvc.Filter
{
    /// <summary>
    /// 捕捉异常过滤器
    /// </summary>
    public class MyExceptionFilter : IExceptionFilter
    {
        private static ILog log = LogManager.GetLogger(typeof(MyExceptionFilter));
        public void OnException(ExceptionContext context)
        {
            log.Error("出现未处理异常", context.Exception);
            // throw new NotImplementedException();
        }
    }
}
