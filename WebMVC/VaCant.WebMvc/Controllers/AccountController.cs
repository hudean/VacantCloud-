using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore;
using VaCant.WebMvc.Models.InputModel;
using VaCant.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using VaCant.Applications.IServices;
using Microsoft.AspNetCore.Http;

namespace VaCant.WebMvc.Controllers
{
    /// <summary>
    /// 注册登录控制器
    /// </summary>
    public class AccountController : Controller
    {

        private readonly IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// 登入
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginInputModel model)
        {
            if (!ModelState.IsValid)
            {
               
            }
            string validateCode = (string)TempData["validateCode"];
            if (!model.ValidateCode.Equals(validateCode))
            {
                return View();
            }
            bool result = false; //_userService.CheckLogin(model.UserName, model.Password);
            var list = _userService.GetAll().ToList();
            long userId = _userService.GetAll().ToList().FirstOrDefault(r => r.UserName == model.UserName && r.Password == model.Password).Id;
            if (result)
            {
                HttpContext.Session.SetString("LoginUserId", userId.ToString());
                return RedirectToAction("Index", "Home");
            }
            return View();
        }



        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        public IActionResult Logout()
        {
            //销毁Session
            HttpContext.Session.Clear();
            return Redirect("Login");
        }



        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <returns></returns>
        public IActionResult CreateVerifyCode()
        {
            string validateCode;
            byte[] buffer = new ValidateCode().GetVerifyCode(out validateCode);
            TempData["validateCode"] = validateCode;
            return File(buffer, @"image/Gif");
        }

        #region Register/注册

        [HttpGet]
        [AllowAnonymousAttribute]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymousAttribute]
        public IActionResult Register(RegisterInputModel model)
        {
            return View();
        }


        #endregion
    }
}
