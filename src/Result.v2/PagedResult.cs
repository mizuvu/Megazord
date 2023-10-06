using System;
using System.Collections.Generic;
using System.Linq;

namespace Zord.Result
{
    public class PagedResult<T> : IResult<IEnumerable<T>>
    {
        public PagedResult() { }

        public PagedResult(IEnumerable<T> data, int page, int pageSize, int count)
        {
            if (data == null)
            {
                Code = ResultCode.NotFound;
            }
            else
            {
                Code = ResultCode.Ok;

                var pagedInfo = new PagedInfo(page, pageSize, count);
                PagedInfo = pagedInfo;

                Data = data;
            }
        }

        public PagedResult(IEnumerable<T> data)
        {
            if (data == null)
            {
                Code = ResultCode.NotFound;
            }
            else
            {
                Code = ResultCode.Ok;

                var page = 1;
                var count = data.Count();

                var pagedInfo = new PagedInfo(page, count, count);
                PagedInfo = pagedInfo;

                Data = data;
            }
        }

        public ResultCode Code { get; set; }

        public bool Succeeded => Code == ResultCode.Ok && Data != null;

        public string Message { get; set; } = "";

        public IEnumerable<string> Errors { get; set; } = Array.Empty<string>();

        public IPagedInfo PagedInfo { get; set; }

        public IEnumerable<T> Data { get; set; } = new List<T>();
    }
}
