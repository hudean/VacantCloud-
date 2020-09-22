using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace VaCant.Entitys
{
    /// <summary>
    /// 角色表
    /// </summary>
    [Table("Roles")]
    public class Role: BaseAllEntity<long>
    {
        [Required]
        [DisplayName("角色名称")]
        public string RoleName { get; set; }

        [DisplayName("描述")]
        public string Description { get; set; }

        [Required]
        [DisplayName("显示名称")]
        public string DisplayName { get; set; }

        [Required]
        [DisplayName("正式名称")]
        public string NormalizedName { get; set; }

        //既可以一张表对应一个Entity，关系表也建立实体，也可以像这样直接让对象带属性，隐式的关系表
        //public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();

        //public ICollection<User> AdminUsers { get; set; } = new List<User>();
        public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
