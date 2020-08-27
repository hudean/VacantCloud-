using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EntityFrameworkCore.Entitys
{
    /// <summary>
    /// 角色表
    /// </summary>
    [Table("Role")]
    public class Role: Entity
    {
        [Required]
        [DisplayName("名称")]
        public string Name { get; set; }
        //既可以一张表对应一个Entity，关系表也建立实体，也可以像这样直接让对象带属性，隐式的关系表
        //public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();

        //public ICollection<User> AdminUsers { get; set; } = new List<User>();
        public virtual ICollection<RolePermissions> RolePermissions { get; set; } = new List<RolePermissions>();
        public virtual ICollection<UserRoles> UserRoles { get; set; } = new List<UserRoles>();
    }
}
