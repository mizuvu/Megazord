using System.Collections.Generic;

namespace Zord
{
    public interface IResult
    {
        ResultCode Code { get; }

        bool Succeeded { get; }

        string Message { get; }

        IEnumerable<string> Errors { get; }
    }

    public interface IResult<out T> : IResult
    {
        T Data { get; }
    }
}