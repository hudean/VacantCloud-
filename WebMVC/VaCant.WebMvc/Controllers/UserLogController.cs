using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyCoreMvc.Entitys;
using MyCoreMVC.Applications.IServices;

namespace VaCant.WebMvc.Controllers
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
