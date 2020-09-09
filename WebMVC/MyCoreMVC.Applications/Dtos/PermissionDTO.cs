using System;
using System.Collections.Generic;
using System.Text;

namespace MyCoreMVC.Applications.Dtos
{
    /// <summary>
    /// 权限
    /// </summary>
    public class PermissionDto : BaseDto
    {
        public string Description { get; set; }
        public string PermissionName { get; set; }

        /// <summary>
        /// 权限值
        /// </summary>
        public string Value { get; set; }
    }
}
