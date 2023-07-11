using System;
using System.Collections.Generic;
using System.Linq;

namespace Zord.Result.Models
{
    public class PagedList<T>
    {
        public PagedList() { }

        public PagedList(IEnumerable<T> data, int page, int pageSize)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? 10 : pageSize;
            var count = data.Count();
            var list = data.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            Records = list;
            Page = page;
            PageSize = pageSize;
            TotalRecords = count;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        }

        public PagedList(IEnumerable<T> data, int page, int pageSize, int count)
        {
            Records = data;
            Page = page;
            PageSize = pageSize;
            TotalRecords = count;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        }

        public int Page { get; set; }

        public int PageSize { get; set; }

        public int TotalRecords { get; set; }

        public int TotalPages { get; set; }

        public bool HasPreviousPage => Page > 1;

        public bool HasNextPage => Page < TotalPages;

        public IEnumerable<T> Records { get; set; }
    }
}
