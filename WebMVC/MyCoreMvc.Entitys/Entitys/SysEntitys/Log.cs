using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace VaCant.Entitys
{
    /// <summary>
    /// 系统日志
    /// </summary>
    [Table("Logs")]
    public class Log : Entity
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 日志级别
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 日志内容
        /// </summary>
        public string LogContent { get; set; }
    }
}