using System.Net;

namespace Zord.Exceptions
{
    public class UnauthorizedException : ExceptionBase
    {
        public UnauthorizedException(string message = "Unauthorized")
            : base(message, null, HttpStatusCode.Unauthorized)
        {
        }
    }
}