﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyCoreMvc.Entitys
{
    /// <summary>
    /// 用户角色表
    /// </summary>
    [Table("UserRoles")]
    public  class UserRole: BaseEntity
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        public int RoleId { get; set; }

        public virtual User User { get; set; }

        public virtual Role Role { get; set; }

    }
}
