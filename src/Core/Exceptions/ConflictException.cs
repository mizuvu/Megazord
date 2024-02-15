using System.Net;

namespace Zord.Exceptions;

public class ConflictException(string message) : ExceptionBase(message, HttpStatusCode.Conflict);