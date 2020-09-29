using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VaCant.Applications.IServices;
using VaCant.WebMvc.Models.InputModel;

namespace VaCant.WebMvc.Controllers
{
    /// <summary>
    /// 权限控制器
    /// </summary>
    public class PermissionController : BaseController
    {
        private readonly IPermissionService _permissionService;
        public PermissionController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }


        /// <summary>
        /// 权限列表
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index(PermissionInputModel model)
        {
            var list= _permissionService.GetAll();
            return View();
        }


        /// <summary>
        /// 添加或修改权限
        /// </summary>
        /// <param name="permissionId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> CreatOrEdit(int permissionId)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreatOrEdit(PermissionCreatOrEdit model)
        {
            return View();
        }

        /// <summary>
        /// 根据id删除权限
        /// </summary>
        /// <param name="permissionId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Delete(int permissionId)
        {
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 批量删除权限
        /// </summary>
        /// <param name="permissionIds"></param>
        /// <returns></returns>
        public async Task<IActionResult> BatchDelete(List<int> permissionIds)
        {
            return RedirectToAction("Index");
        }
    }
}
