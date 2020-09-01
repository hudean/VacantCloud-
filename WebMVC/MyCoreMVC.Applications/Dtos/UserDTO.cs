using System;
using System.Collections.Generic;
using System.Text;

namespace MyCoreMVC.Applications.Dtos
{
    public class UserDTO : BaseDTO
    {
        public string PhoneNum { get; set; }
        public int LoginErrorTimes { get; set; }
        public DateTime? LastLoginErrorDateTime { get; set; }
        public long? CityId { get; set; }
    }
}
