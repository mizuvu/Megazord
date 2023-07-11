using System.Collections.Generic;
using Zord.Result.Models;

namespace Zord.Result
{
    public static class ResultExtensions
    {
        public static PagedList<T> ToPagedList<T>(this IEnumerable<T> data, int page = 1, int pageSize = 10)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? 10 : pageSize;
            return new PagedList<T>(data, page, pageSize);
        }

        public static PagedResult<T> ToPagedResult<T>(this IEnumerable<T> data, int page = 1, int pageSize = 10)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? 10 : pageSize;
            return new PagedResult<T>(data, page, pageSize);
        }
    }
}