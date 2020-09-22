using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace VaCant.Entitys
{
    /// <summary>
    /// 用户日志
    /// </summary>
    [Table("UserLogs")]
    public class UserLog:BaseEntity<long>
    {
        /// <summary>
        /// 日志类型
        /// </summary>
        public String LogType { get; set; }

        /// <summary>
        /// 日志内容
        /// </summary>
        public String LogContent { get; set; }
    }
}
