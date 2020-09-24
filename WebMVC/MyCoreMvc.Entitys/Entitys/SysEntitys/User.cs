using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VaCant.Entitys
{
    /// <summary>
    /// 后台用户表
    /// </summary>
    [Table("Users")]
    public class User : BaseEntity<long>
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        [Required]
        [DisplayName("用户名称")]
        public string UserName { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [DisplayName("是否启用")]
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// 用户邮箱
        /// </summary>
        [Required]
        [DisplayName("用户邮箱")]
        public string Email { get; set; }

        /// <summary>
        /// 用户电话
        /// </summary>
        [Required]
        [DisplayName("用户电话")]
        public string PhoneNum { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        [Required]
        [DisplayName("用户密码")]
        public string Password { get; set; }

        /// <summary>
        /// 密码盐
        /// </summary>
        public string PasswordSalt { get; set; }

        /// <summary>
        /// 用户地址
        /// </summary>
        [DisplayName("用户地址")]
        [Required]
        public string Address { get; set; }

        /// <summary>
        /// 登入错误次数
        /// </summary>
        public int LoginErrorTimes { get; set; }

        /// <summary>
        /// 最后登入错误时间
        /// </summary>
        public DateTime? LastLoginErrorDateTime { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        //public virtual ICollection <Role> Roles { get; set; } = new List<Role>();
    }
}