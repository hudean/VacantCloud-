using System;
using System.Collections.Generic;
using System.Text;

namespace EFCore.Sharding.Primitives
{
    /// <summary>
    /// 读写模式
    /// </summary>
    [Flags]
    public enum ReadWriteType
    {
        /// <summary>
        /// 只读
        /// </summary>
        Read = 1,

        /// <summary>
        /// 只写
        /// </summary>
        Write = 2
    }
}
