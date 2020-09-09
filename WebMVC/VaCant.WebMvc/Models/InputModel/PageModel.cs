using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VaCant.WebMvc.Models.InputModel
{
    public class PageModel
    {
        public int PageIndex { get; set; } = 1;

        public int PageSize { get; set; }
        public int PageCount { get; set; }
    }
}
