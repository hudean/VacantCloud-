using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VaCant.Applications.IServices;

namespace VaCant.WebMvc.Controllers
{
    /// <summary>
    /// 系统日志控制器
    /// </summary>
    public class LogController : Controller
    {
        private readonly ILogService _logService;

        public LogController(ILogService logService)
        {
            _logService = logService;
        }

        public IActionResult GetLogList()
        {
            var list= _logService.GetAll().ToList();
            return View(list);
        }

        public IActionResult GetLogLevelList()
        {
            return View();
        }
    }
}
