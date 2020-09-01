using System;
using System.Collections.Generic;
using System.Text;

namespace MyCoreMVC.Applications.Dtos
{
    /// <summary>
    /// 权限
    /// </summary>
    public class PermissionDTO : BaseDTO
    {
        public String Name { get; set; }
        public String Description { get; set; }
    }
}
