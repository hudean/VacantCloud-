using System.ComponentModel.DataAnnotations.Schema;

namespace VaCant.Entitys
{
    /// <summary>
    /// 角色权限表
    /// </summary>
    [Table("RolePermissions")]
    public class RolePermission : Entity
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public long RoleId { get; set; }

        /// <summary>
        /// 权限id
        /// </summary>
        public long PermissionId { get; set; }

        public virtual Role Role { get; set; }

        public virtual Permission Permission { get; set; }
    }
}