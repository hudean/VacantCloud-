using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VaCant.Entitys;
using VaCant.Applications.IServices;
using VaCant.WebMvc.Filter;

namespace VaCant.WebMvc.Controllers
{
    /// <summary>
    /// 用户日志控制器
    /// </summary>
    [CheckPermission("Log_GetLogList")]
    public class UserLogController : BaseController
    {
        private readonly IUserLogService _userLogService;
        public UserLogController(IUserLogService userLogService)
        {
            _userLogService = userLogService;
        }


        public IActionResult GetUserLogList()
        {
            List<UserLog> list = new List<UserLog>()
            {
                new UserLog(){
                     CreationTime=DateTime.Now,
                      CreatorUserId=0,
                       Id=10,
                        IsDelete=false,
                         LogContent="测试日志",
                          LogType="错误日志"
                }, new UserLog(){
                     CreationTime=DateTime.Now,
                      CreatorUserId=0,
                       Id=10,
                        IsDelete=false,
                         LogContent="测试日志",
                          LogType="错误日志"
                }, new UserLog(){
                     CreationTime=DateTime.Now,
                      CreatorUserId=0,
                       Id=10,
                        IsDelete=false,
                         LogContent="测试日志",
                          LogType="错误日志"
                }, new UserLog(){
                     CreationTime=DateTime.Now,
                      CreatorUserId=0,
                       Id=10,
                        IsDelete=false,
                         LogContent="测试日志",
                          LogType="错误日志"
                }
            };
            return View(list);
        }

        public IActionResult GetUserLogTypeList()
        {
            return View();
        }
    }
}
