using Microsoft.AspNetCore.Mvc;
using VaCant.Applications.Dtos;
using VaCant.Applications.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VaCant.WebMvc.Models.InputModel;
using Microsoft.EntityFrameworkCore;
using VaCant.WebMvc.Models.ViewModel;
using System.Collections.Immutable;

namespace VaCant.WebMvc.Controllers
{
    /// <summary>
    /// 用户控制器
    /// </summary>
    public class UserController: BaseController
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
            var list =await _userService.GetAll().Take(50).ToListAsync();
            //var list = _userService.GetPageList();
            return View(list);
        }

        [HttpPost]
        public async Task<IActionResult> Index(UserInputModel model)
        {
            var list = await _userService.GetAll().Take(50).ToListAsync();
            return View(list);
        }

        /// <summary>
        /// 添加或修改用户
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> CreatOrEdit(int userId)
        {
            EditUserModalViewModel model =new EditUserModalViewModel();
            if (userId > 0)
            {
                model.User = await _userService.GetAsync(userId);
                model.Roles = (IReadOnlyList<RoleDto>)(_roleService.GetAll().ToList());
                return View(model);
            }
            return View();
        }

        public async Task<IActionResult> CreatOrEdit(UserCreatOrEditInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (model.Password != model.TruePassword)
            {
                return View();
            }
            UserDto dto = new UserDto()
            {
                Address = model.Address,
                CreatorUserId=0,
                IsDelete=false,
                CreationTime = DateTime.Now,
                Email = model.Email,
                Id = model.Id,
                UserName = model.Email,
                PhoneNum = model.PhoneNum,
                PasswordSalt = "",
                Password = model.Password,
                LoginErrorTimes = 0,
                LastLoginErrorDateTime = null
            };
            if (model.Id > 0)
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


        public async Task<IActionResult> Detail(int userid)
        {
           var model= await _userService.GetAsync(userid);
            return View(model);
        }



        /// <summary>
        /// 根据id删除用户
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
