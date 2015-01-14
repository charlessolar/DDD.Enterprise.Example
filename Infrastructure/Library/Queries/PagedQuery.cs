using System;

namespace Demo.Library.Queries
{
    public class PagedQuery : BasicQuery
    {
        public Int32 Page { get; set; }

        public Int32 PageSize { get; set; }
    }
}