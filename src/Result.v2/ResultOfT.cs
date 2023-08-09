using System;
using System.Collections.Generic;

namespace Zord.Result
{
    public class Result<T> : IResult<T>
    {
        public Result()
        {
        }

        protected internal Result(T data, string message)
        {
            if (data == null)
            {
                Code = ResultCode.NotFound;
                Message = string.IsNullOrEmpty(message) ? $"Null value object {typeof(T).Name}" : message;
            }
            else
            {
                Code = ResultCode.Ok;
                Message = message;
                Data = data;
            }
        }

        protected internal Result(ResultCode code, string message, IEnumerable<string> errors)
        {
            Code = code;
            Message = message;

            if (errors != null)
            {
                Errors = errors;
            }
        }

        public ResultCode Code { get; set; }

        public bool Succeeded => Code == ResultCode.Ok && Data != null;

        public string Message { get; set; } = "";

        public IEnumerable<string> Errors { get; set; } = Array.Empty<string>();

        public T Data { get; set; }

        public static Result<T> Object(T data, string message = "")
            => new Result<T>(data, message);

        public static Result<T> ObjectNotFound(string objectName, object queryValue)
        {
            var message = $"Query object {objectName} by {queryValue} not found";

            return new Result<T>(ResultCode.NotFound, message, default);
        }

        public static Result<T> ObjectNotFound<TObject>(object queryValue)
        {
            var message = $"Query object {typeof(TObject).Name} by {queryValue} not found";

            return new Result<T>(ResultCode.NotFound, message, default);
        }

        public static Result<T> BadRequest(string message = "", IEnumerable<string> errors = default)
            => new Result<T>(ResultCode.BadRequest, message, errors);

        public static Result<T> Error(string message = "", IEnumerable<string> errors = default)
            => new Result<T>(ResultCode.Error, message, errors);
    }
}