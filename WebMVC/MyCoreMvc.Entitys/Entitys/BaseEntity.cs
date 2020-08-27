using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EntityFrameworkCore.Entitys
{
    public abstract  class BaseEntity
    {
        [Key]
        [DisplayName("主键Id")]
        public long Id { get; set; }
        [DisplayName("创建时间")]
        public DateTime CreateTime { get; set; }
        [DisplayName("修改时间")]
        public DateTime UpdateTime { get; set; }
        [DisplayName("是否删除：0表示未删除1删除")]
        public int IsDelete { get; set; }
    }
}
