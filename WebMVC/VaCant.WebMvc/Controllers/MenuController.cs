using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VaCant.WebMvc.Filter;

namespace VaCant.WebMvc.Controllers
{
    /// <summary>
    /// 菜单控制器
    /// </summary>
    [CheckPermission("Log_GetLogList")]
    public class MenuController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
