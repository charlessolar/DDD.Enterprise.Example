using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Queries
{
    public class PagedQuery : IBasicQuery
    {
        public Int32 Page { get; set; }
        public Int32 PageSize { get; set; }
    }
}