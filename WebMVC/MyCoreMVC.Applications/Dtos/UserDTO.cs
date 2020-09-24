using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VaCant.Applications.Dtos
{
    public class UserDto : BaseDto
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// 用户邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// 用户电话
        /// </summary>
        public string PhoneNum { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// 密码盐
        /// </summary>
        public string PasswordSalt { get; set; }

        /// <summary>
        /// 用户地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 登入错误次数
        /// </summary>
        public int LoginErrorTimes { get; set; }

        /// <summary>
        /// 最后登入错误时间
        /// </summary>
        public DateTime? LastLoginErrorDateTime { get; set; }

        /// <summary>
        ///
        /// </summary>
        public List<long> RoleIds { get; set; }

        /// <summary>
        ///
        /// </summary>
        public List<string> RoleNames { get; set; }
    }
}