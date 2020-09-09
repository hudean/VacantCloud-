using Microsoft.AspNetCore.Mvc;
using MyCoreMVC.Applications.Dtos;
using MyCoreMVC.Applications.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMVC.Models.InputModel;

namespace WebMVC.Controllers
{
    public class UserController: Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        /// <summary>
        /// 用户列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var list = _userService.GetAll(null);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(UserInputModel model)
        {
            var list = _userService.GetAll();
            return View();
        }





        /// <summary>
        /// 添加或修改权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> CreatOrEdit(int userId)
        {
            if (userId > 0)
            {
                var model = _userService.Get(userId);
                return View(model);
            }
            return View();
        }

        public async Task<IActionResult> CreatOrEdit(UserCreatOrEditInputModel model)
        {
            UserDto dto = new UserDto()
            {
                Address = "",
                CreatorUserId=0,
                IsDelete=false,
                CreationTime = DateTime.Now,
                Email = "",
                Id = 0,
                UserName = "",
                PhoneNum = "",
                PasswordSalt = "",
                Password = "",
                LoginErrorTimes = 0,
                LastLoginErrorDateTime = null
            };
            if (model.id > 0)
            {
                //修改
              
                _userService.Update(dto);
            }
            else
            {
                _userService.Add(dto);
            }
            return View();
        }



        /// <summary>
        /// 根据id删除权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Delete(int userId)
        {
            if (userId <= 0)
            {
                return View("error");
            }
            _userService.Delete(userId);

            return RedirectToAction("Index");
        }

        /// <summary>
        /// 批量删除权限
        /// </summary>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public async Task<IActionResult> BatchDelete(List<int> roleIds)
        {
            return RedirectToAction("Index");
        }
    }
}
