using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyCoreMVC.Applications.IServices;

namespace WebMVC.Controllers
{
    /// <summary>
    /// 用户日志控制器
    /// </summary>
    public class UserLogController : Controller
    {
        private readonly IUserLogService _userLogService;
        public UserLogController(IUserLogService userLogService)
        {
            _userLogService = userLogService;
        }


        public IActionResult GetUserLogList()
        {
            return View();
        }

        public IActionResult GetUserLogTypeList()
        {
            return View();
        }
    }
}
