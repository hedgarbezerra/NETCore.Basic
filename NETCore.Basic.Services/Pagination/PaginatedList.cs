using System;
using System.Collections.Generic;
using System.Linq;

namespace NETCore.Basic.Services.Pagination
{

    public class PaginatedList<T>
    {

        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public int TotalPages { get; private set; }
        public Uri NextPage { get; private set; }
        public Uri PreviousPage { get; private set; }
        public List<T> Data { get; private set; }


        public PaginatedList(IQueryable<T> source, IUriService uriService, string route, int pageIndex, int? pageSize = 10)
        {
            PageIndex = pageIndex;
            PageSize = (int)pageSize;
            TotalCount = source.Count();
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);
            NextPage = HasNextPage ? uriService.GetPageUri(pageIndex + 1, PageSize, route) : null;
            PreviousPage = HasPreviousPage ? uriService.GetPageUri(pageIndex - 1, PageSize, route) : null;
            Data = source.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
        }
        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }
    }
}
