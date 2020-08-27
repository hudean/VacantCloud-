using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyCoreMvc.Entitys
{
    [Table("RolePermissions")]
    public class RolePermissions: Entity
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
