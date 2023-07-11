namespace Zord.Result
{
    public interface IResult
    {
        bool Succeeded { get; }

        string Message { get; }
    }

    public interface IResult<out T> : IResult
    {
        T Data { get; }
    }
}