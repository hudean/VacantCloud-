using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyCoreMvc.Entitys
{
    /// <summary>
    /// 权限表
    /// </summary>
    [Table("Permissions")]
    public class Permission: Entity
    {
        [DisplayName("描述")]
        public string Description { get; set; }
        [Required]
        [DisplayName("权限名称")]
        public string PermissionName { get; set; }

        /// <summary>
        /// 权限值
        /// </summary>
        public string Value { get; set; }
        //public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
        public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}
