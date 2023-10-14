using System.Net;

namespace Zord.Exceptions
{
    public class ForbiddenException : ExceptionBase
    {
        public ForbiddenException(string message)
            : base(message, null, HttpStatusCode.Forbidden)
        {
        }
    }
}