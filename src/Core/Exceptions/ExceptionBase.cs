using System.Net;

namespace Zord.Exceptions
{
    public class ExceptionBase : Exception
    {
        public IEnumerable<string>? ErrorMessages { get; }

        public HttpStatusCode StatusCode { get; }

        public ExceptionBase(string message, IEnumerable<string>? errors = null, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
            : base(message)
        {
            ErrorMessages = errors;
            StatusCode = statusCode;
        }
    }
}