﻿using Microsoft.AspNetCore.Mvc;
using VaCant.Applications.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VaCant.WebMvc.Models.InputModel;
using VaCant.WebMvc.Filter;

namespace VaCant.WebMvc.Controllers
{
    /// <summary>
    /// 角色控制器
    /// </summary>
    [CheckPermission("Log_GetLogList")]
    public class RolesController : BaseController
    {
        private readonly IRoleService _roleService;
        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }


        /// <summary>
        /// 角色列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var list = _roleService.GetAll();
            return View(list);
        }

        [HttpPost]
        public async Task<IActionResult> Index(RoleInputModel model)
        {
            var list = _roleService.GetAll();
            return View();
        }



        /// <summary>
        /// 添加或修改角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> CreatOrEdit(int roleId)
        {
            var role = _roleService.Get(roleId);
            return View(role);
        }

        [HttpPost]
        public async Task<IActionResult> CreatOrEdit(RoleCreatOrEditInputModel model)
        {
            return View();
        }


        /// <summary>
        /// 根据id删除角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Delete(int roleId)
        {
           await  _roleService.DeleteAsync(roleId);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 批量删除角色
        /// </summary>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> BatchDelete(List<int> roleIds)
        {
            return RedirectToAction("Index");
        }

    }
}
