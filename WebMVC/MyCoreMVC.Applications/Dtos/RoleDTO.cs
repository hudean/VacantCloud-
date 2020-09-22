using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace VaCant.Applications.Dtos
{
    /// <summary>
    /// 角色
    /// </summary>
    public class RoleDto : BaseDto
    {
        public string RoleName { get; set; }

        public List<string> PermissionNames { get; set; }

        public List<long> PermissionIds { get; set; }

    }
}
