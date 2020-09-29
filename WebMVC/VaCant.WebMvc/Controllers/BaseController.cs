using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net.Core;
using Microsoft.AspNetCore.Mvc;
using VaCant.WebMvc.Filter;

namespace VaCant.WebMvc.Controllers
{
    /// <summary>
    /// 给这个特性[ServiceFilter(typeof(VaCantAuthorizationFilter))]就会使用过滤器
    /// </summary>
   // [ServiceFilter(typeof(VaCantAuthorizationFilter))]
    [CheckPermission("SSS")]
    public class BaseController : Controller
    {
        //日志主要用来记录每个用户的访问和操作记录
        public ILogger Logger { get; set; }

        protected BaseController()
        { 
            
        }

    }
}
