using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EntityFrameworkCore.Entitys
{
    /// <summary>
    /// 后台用户表
    /// </summary>
    [Table("User")]
    public class User: Entity
    {
        [Required]
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNum { get; set; }
        [Required]
        public string Password { get; set; }
        /// <summary>
        /// 密码盐
        /// </summary>
        public string PasswordSalt { get; set; }
        public string  Address { get; set; }
        /// <summary>
        /// 登入错误次数
        /// </summary>
        public int LoginErrorTimes { get; set; }
        /// <summary>
        /// 最后登入错误时间
        /// </summary>
        public DateTime? LastLoginErrorDateTime { get; set; }

        public virtual ICollection<UserRoles> UserRoles { get; set; } = new List<UserRoles>();

        //public virtual ICollection <Role> Roles { get; set; } = new List<Role>();
    }
}
