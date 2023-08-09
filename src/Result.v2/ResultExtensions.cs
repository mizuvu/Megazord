﻿using System.Collections.Generic;
using System.Linq;

namespace Zord.Result
{
    public static class ResultExtensions
    {
        public static PagedResult<T> ToPagedResult<T>(this IEnumerable<T> list, int page = 1, int pageSize = 10)
        {
            if (list == null)
            {
                return new PagedResult<T> { Code = ResultCode.Error };
            }

            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? 10 : pageSize;
            var count = list.Count();
            var data = list.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return new PagedResult<T>(data, page, pageSize, count);
        }
    }
}