using System.Net;

namespace Zord.Exceptions;

public class ForbiddenException(string message) : ExceptionBase(message, HttpStatusCode.Forbidden);