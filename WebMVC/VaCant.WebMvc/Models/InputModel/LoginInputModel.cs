using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VaCant.WebMvc.Models.InputModel
{
    public class LoginInputModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public string ValidateCode { get; set; }

    }
}
