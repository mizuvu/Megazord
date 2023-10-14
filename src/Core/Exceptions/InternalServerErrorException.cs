using System.Net;

namespace Zord.Exceptions
{
    public class InternalServerErrorException : ExceptionBase
    {
        public InternalServerErrorException(string message)
            : base(message, null, HttpStatusCode.InternalServerError)
        {
        }

        public InternalServerErrorException(IEnumerable<string> messages)
            : base("", messages, HttpStatusCode.InternalServerError)
        {
        }
    }
}