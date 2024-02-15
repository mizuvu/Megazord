using System.Net;

namespace Zord.Exceptions;

public class UnauthorizedException(string message = "Unauthorized") : ExceptionBase(message, HttpStatusCode.Unauthorized);