using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VaCant.WebMvc.Models.InputModel
{
    public class UserCreatOrEditInputModel
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string PhoneNum { get; set; }

        public string Password { get; set; }

        public string TruePassword { get; set; }

        public string Address { get; set; }

    }
}
