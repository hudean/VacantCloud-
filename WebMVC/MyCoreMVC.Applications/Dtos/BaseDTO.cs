using System;
using System.Collections.Generic;
using System.Text;

namespace MyCoreMVC.Applications.Dtos
{
    [Serializable]
    public abstract class BaseDTO
    {
        public long Id { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}
