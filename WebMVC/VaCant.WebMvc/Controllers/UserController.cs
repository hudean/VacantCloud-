using Microsoft.AspNetCore.Mvc;
using MyCoreMVC.Applications.Dtos;
using MyCoreMVC.Applications.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VaCant.WebMvc.Models.InputModel;

namespace VaCant.WebMvc.Controllers
{
    public class UserController: Controller
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        public UserController(IUserService userService, IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }


        /// <summary>
        /// 用户列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
           // var list = _userService.GetAll();
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
                var model = await _userService.GetAsync(userId);
                var roles = _roleService.GetAll();
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
              
               await _userService.UpdateAsync(dto);
            }
            else
            {
               await  _userService.AddAsync(dto);
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
          await  _userService.DeleteAsync(userId);

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
