using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VaCant.Core.Authorization;
using VaCant.WebMvc.Filter;
using VaCant.WebMvc.Models;

namespace VaCant.WebMvc.Controllers
{
    /// <summary>
    /// 首页控制器
    /// </summary>
    //[ServiceFilter(typeof(VaCantAuthorizationFilter))]
    [ServiceFilter(typeof(VaCantAuthorizationFilter))]
    [CheckPermission("Home")]
   // [VaCantMvcAuthorize(PermissionNames.Pages_Tenants)]
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        [CheckPermission("Home_Index")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// 统一跳转错误页
        /// </summary>
        /// <returns></returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
