using System.ComponentModel.DataAnnotations.Schema;

namespace VaCant.Entitys
{
    /// <summary>
    /// 用户角色表
    /// </summary>
    [Table("UserRoles")]
    public class UserRole : Entity<long>
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        public long RoleId { get; set; }

        public virtual User User { get; set; }

        public virtual Role Role { get; set; }
    }
}