using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyCoreMvc.Entitys
{
    [Table("Tenants")]
    public class Tenants : Entity<long>
    {
        public string Name { get; set; }

        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

    }
}
