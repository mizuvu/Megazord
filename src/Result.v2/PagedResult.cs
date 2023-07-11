using System.Collections.Generic;
using Zord.Result.Models;

namespace Zord.Result
{
    public class PagedResult<T> : Result<PagedList<T>>
    {
        public PagedResult() { }

        public PagedResult(string message = "", IEnumerable<string> errors = default)
        {
            Code = ResultCode.Error;
            Message = message;
            Errors = errors;
        }

        public PagedResult(IEnumerable<T> data, int page, int pageSize)
        {
            if (data == null)
            {
                Code = ResultCode.NotFound;
            }
            else
            {
                Code = ResultCode.Ok;
                Data = new PagedList<T>(data, page, pageSize);
            }
        }

        public PagedResult(IEnumerable<T> data, int page, int pageSize, int count)
        {
            if (data == null)
            {
                Code = ResultCode.NotFound;
            }
            else
            {
                Code = ResultCode.Ok;
                Data = new PagedList<T>(data, page, pageSize, count);
            }
        }
    }
}
