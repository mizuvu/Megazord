using System.Collections.Generic;

namespace Zord.Result
{
    public class Result : Result<object>
    {
        public static new Result Success()
        {
            return new Result { Succeeded = true };
        }

        public static new Result Success(string message)
        {
            return new Result { Succeeded = true, Message = message };
        }

        public static new Result Error()
        {
            return new Result();
        }

        public static new Result Error(string message, IEnumerable<string> errors = default)
        {
            return new Result { Message = message, Errors = errors };
        }

        public static new Result Error(IEnumerable<string> errors)
        {
            return new Result { Errors = errors };
        }
    }

    public class Result<T> : IResult<T>
    {
        public bool Succeeded { get; set; }

        public string Message { get; set; }

        public IEnumerable<string> Errors { get; set; }

        public T Data { get; set; }

        public static Result<T> Success()
        {
            return new Result<T> { Succeeded = true };
        }

        public static Result<T> Success(string message)
        {
            return new Result<T> { Succeeded = true, Message = message };
        }

        public static Result<T> Success(T data)
        {
            return new Result<T> { Succeeded = true, Data = data };
        }

        public static Result<T> Error()
        {
            return new Result<T>();
        }

        public static Result<T> Error(string message, IEnumerable<string> errors = default)
        {
            return new Result<T> { Message = message, Errors = errors };
        }

        public static Result<T> Error(IEnumerable<string> errors)
        {
            return new Result<T> { Errors = errors };
        }
    }
}