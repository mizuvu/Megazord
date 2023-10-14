using System.Net;

namespace Zord.Exceptions;

public class ValidationException : ExceptionBase
{
    public ValidationException(string message = "One or more validation errors occurred.")
        : base(message, null, HttpStatusCode.BadRequest)
    {
    }

    public ValidationException(List<string> messages)
        : base("", messages, HttpStatusCode.BadRequest)
    {
    }
}
