using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyCoreMvc.Entitys
{

    public abstract class BaseEntity : BaseEntity<int>
    { 
    
    }

    public abstract  class BaseEntity<T>:Entity<T>
    {
        //[Key]
        //[DisplayName("主键Id")]
        //public long Id { get; set; }
        [DisplayName("创建时间")]
        public DateTime CreationTime { get; set; }
        [DisplayName("创建人用户id")]
        public long CreatorUserId { get; set; }
        [DisplayName("是否删除：0表示未删除1删除")]
        public bool IsDelete { get; set; }
    }

    public abstract class AllBaseEntity<T> : BaseEntity<T>
    {
        [DisplayName("删除时间")]
        public DateTime DeletionTime { get; set; }
        [DisplayName("删除人用户id")]
        public long DeleterUserId { get; set; }
        [DisplayName("最后修改时间")]
        public DateTime LastModificationTime { get; set; }

        [DisplayName("最后修改人用户id")]
        public long LastModifierUserId { get; set; }
    }

    public abstract class AllBaseEntity : AllBaseEntity<int>
    { 
        
    }


}
