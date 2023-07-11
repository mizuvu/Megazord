using System;
using System.Collections.Generic;

namespace Zord.Result
{
    public class PagedList
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        public bool HasPreviousPage => Page > 1;
        public bool HasNextPage => Page < TotalPages;
    }


    public class PagedList<T> : PagedList
    {
        public PagedList() { }

        public PagedList(List<T> data, int count = 0, int page = 1, int pageSize = 10)
        {
            Data = data;
            Page = page;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalRecords = count;
        }

        public IEnumerable<T> Data { get; set; }
    }
}
