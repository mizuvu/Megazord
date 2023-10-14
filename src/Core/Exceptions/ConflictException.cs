using System.Net;

namespace Zord.Exceptions
{
    public class ConflictException : ExceptionBase
    {
        public ConflictException(string message)
            : base(message, null, HttpStatusCode.Conflict)
        {
        }
    }
}