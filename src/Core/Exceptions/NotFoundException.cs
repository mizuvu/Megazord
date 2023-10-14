using System.Net;

namespace Zord.Exceptions
{
    public class NotFoundException : ExceptionBase
    {
        public NotFoundException(string message)
            : base(message, null, HttpStatusCode.NotFound)
        {
        }

        public NotFoundException(string key, string objectName)
            : base($"Queried object {objectName} by {key} was not found", null, HttpStatusCode.NotFound)
        {
        }
    }
}