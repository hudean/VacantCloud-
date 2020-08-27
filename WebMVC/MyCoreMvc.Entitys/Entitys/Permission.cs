using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EntityFrameworkCore.Entitys
{
    /// <summary>
    /// 权限表
    /// </summary>
    [Table("Permission")]
    public class Permission: Entity
    {
        [DisplayName("描述")]
        public string Description { get; set; }
        [Required]
        [DisplayName("描述")]
        public string Name { get; set; }
        //public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
        public virtual ICollection<RolePermissions> RolePermissions { get; set; } = new List<RolePermissions>();
    }
}
