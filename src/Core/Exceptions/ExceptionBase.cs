using System.Net;

namespace Zord.Exceptions;

public abstract class ExceptionBase(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError) : Exception(message)
{
    public HttpStatusCode StatusCode { get; } = statusCode;
}