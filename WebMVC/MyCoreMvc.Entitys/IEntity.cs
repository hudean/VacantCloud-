using System;
using System.Collections.Generic;
using System.Text;

namespace MyCoreMvc.Entitys
{
    /// <summary>
    /// A shortcut of <see cref="IEntity{TPrimaryKey}"/> for most used primary key type (<see cref="long"/>).
    /// </summary>
    public interface IEntity : IEntity<long>
    {

    }
}
