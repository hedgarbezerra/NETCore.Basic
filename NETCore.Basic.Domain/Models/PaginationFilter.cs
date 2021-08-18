using System;
using System.Collections.Generic;
using System.Text;

namespace NETCore.Basic.Domain.Models
{
    public class PaginationFilter
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public PaginationFilter()
        {
            PageIndex = 1;
            PageSize = 10;
        }
        public PaginationFilter(int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
        }
    }
}
