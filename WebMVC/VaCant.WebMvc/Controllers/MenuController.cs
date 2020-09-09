using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace VaCant.WebMvc.Controllers
{
    /// <summary>
    /// 菜单控制器
    /// </summary>
    public class MenuController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
