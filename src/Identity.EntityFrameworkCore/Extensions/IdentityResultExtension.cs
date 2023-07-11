using Microsoft.AspNetCore.Identity;

namespace Zord.Identity.EntityFrameworkCore.Extensions;

public static class IdentityResultExtensions
{
    public static Result.Result ToResult(this IdentityResult result)
    {
        return result.Succeeded
            ? Result.Result.Success()
            : Result.Result.Error(errors: result.Errors.Select(e => e.Description));
    }

    public static Result<T> ToResult<T>(this IdentityResult result, T data)
    {
        return result.Succeeded
            ? Result<T>.Object(data: data)
            : Result<T>.Error(errors: result.Errors.Select(e => e.Description));
    }
}
