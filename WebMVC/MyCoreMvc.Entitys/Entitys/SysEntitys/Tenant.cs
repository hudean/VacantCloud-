using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace VaCant.Entitys
{
    /// <summary>
    /// 租户表
    /// </summary>
    [Table("Tenants")]
    public class Tenant : Entity<long>
    {
        /// <summary>
        /// 租户名称
        /// </summary>
        public string TenantName { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// 是否删除 0否1是
        /// </summary>
        public bool IsDeleted { get; set; }

    }
}
