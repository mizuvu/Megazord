using System.Net;

namespace Zord.Exceptions;

public class InternalServerErrorException(string message) : ExceptionBase(message, HttpStatusCode.InternalServerError);