namespace Zord.Result
{
    public enum ResultCode
    {
        Unknown = 0,
        Ok = 200,
        BadRequest = 400,
        Forbidden = 403,
        Unauthorized = 401,
        NotFound = 404,
        Conflict = 409,
        Error = 500,
    }
}
