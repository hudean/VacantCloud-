using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyCoreMvc.Entitys
{
    [Table("UserRoles")]
    public  class UserRoles: Entity
    {
        public long UserId { get; set; }
        public int RoleId { get; set; }

        public DateTime CreationTime { get; set; }

        public long CreatorUserId { get; set; }
    }
}
