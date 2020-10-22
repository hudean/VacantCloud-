using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VaCant.Applications.IServices;
using VaCant.WebMvc.Filter;

namespace VaCant.WebMvc.Controllers
{
    /// <summary>
    /// 系统日志控制器
    /// </summary>
    [CheckPermission("Log")]
    public class LogController : Controller
    {
        private readonly ILogService _logService;

        public LogController(ILogService logService)
        {
            _logService = logService;
        }

        [CheckPermission("Log_GetLogList")]
        public IActionResult GetLogList()
        {
            var list= _logService.GetAll().ToList();
            return View(list);
        }

        [CheckPermission("Log_GetLogLevelList")]
        public IActionResult GetLogLevelList()
        {
            return View();
        }
    }
}
