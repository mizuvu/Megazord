using System;
using System.Collections.Generic;

namespace Zord.Result
{
    public class Result : IResult
    {
        public Result() { }

        protected internal Result(string message)
        {
            Code = ResultCode.Unknown;
            Message = message;
        }

        protected internal Result(ResultCode code)
        {
            Code = code;
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

        public bool Succeeded => Code == ResultCode.Ok;

        public string Message { get; set; } = "";

        public IEnumerable<string> Errors { get; set; } = Array.Empty<string>();

        public static Result ObjectNotFound(string objectName, object value)
        {
            var message = $"Query object {objectName} by {value} not found";

            return new Result(ResultCode.NotFound, message, default);
        }

        public static Result ObjectNotFound<TObject>(object queryValue)
        {
            var message = $"Query object {typeof(TObject).Name} by {queryValue} not found";

            return new Result(ResultCode.NotFound, message, default);
        }

        public static Result Success(string message = "") => new Result(ResultCode.Ok, message, default);

        public static Result BadRequest(string message = "", IEnumerable<string> errors = default) =>
            new Result(ResultCode.BadRequest, message, errors);

        public static Result Forbidden(string message = "", IEnumerable<string> errors = default) =>
            new Result(ResultCode.Forbidden, message, errors);

        public static Result Unauthorized(string message = "", IEnumerable<string> errors = default) =>
            new Result(ResultCode.Unauthorized, message, errors);

        public static Result NotFound(string message = "", IEnumerable<string> errors = default) =>
            new Result(ResultCode.NotFound, message, errors);

        public static Result Conflict(string message = "", IEnumerable<string> errors = default) =>
            new Result(ResultCode.Conflict, message, errors);

        public static Result Error(string message = "", IEnumerable<string> errors = default) =>
            new Result(ResultCode.Error, message, errors);
    }
}