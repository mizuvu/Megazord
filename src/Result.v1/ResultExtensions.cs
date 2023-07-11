using System.Collections.Generic;
using System.Linq;

namespace Zord.Result
{
    public static class ResultExtensions
    {
        public static PagedList<T> ToPagedList<T>(this IEnumerable<T> data, int page = 1, int pageSize = 10)
        {
            page = page <= 0 ? 1 : page;
            pageSize = pageSize <= 0 ? 10 : pageSize;
            var count = data.Count();
            var items = data.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return new PagedList<T>(items, count, page, pageSize);
        }
    }
}