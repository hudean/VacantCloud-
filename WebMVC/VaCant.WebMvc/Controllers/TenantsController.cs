using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VaCant.Core.Authorization;
using VaCant.WebMvc.Filter;

namespace VaCant.WebMvc.Controllers
{
    /// <summary>
    /// 租户控制器
    /// </summary>
    [VaCantMvcAuthorize(PermissionNames.Pages_Tenants)]
    public class TenantsController : Controller
    {
        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            return View();
        }
        public async Task<IActionResult> Detail()
        {
            return View();
        }

        public async Task<IActionResult> CreatOrEdit(long id)
        {
            return View();
        }
        public async Task Delete(long id)
        { 
        
        }

        public async Task BathDelete(long[] ids)
        {

        }
    }
}
