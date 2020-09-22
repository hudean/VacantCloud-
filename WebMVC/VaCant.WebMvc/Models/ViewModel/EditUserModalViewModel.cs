using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VaCant.Applications.Dtos;

namespace VaCant.WebMvc.Models.ViewModel
{
    public class EditUserModalViewModel
    {
        public UserDto User { get; set; }

        public IReadOnlyList<RoleDto> Roles { get; set; }

        public bool UserIsInRole(RoleDto role)
        {
            return User.RoleNames != null && User.RoleNames.Any(r => r == role.RoleName);
        }
    }
}
