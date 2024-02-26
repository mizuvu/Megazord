using Microsoft.AspNetCore.Mvc;
using Zord;

namespace Sample.Extensions
{
    public static class ApiResultExtensions
    {
        public static ObjectResult ToResponse(this Zord.IResult result)
        {
            if (result.Succeeded)
            {
                return new ObjectResult(default)
                {
                    StatusCode = (int)result.Code,
                    Value = result.Message
                };
            }

            return new ObjectResult(default)
            {
                StatusCode = (int)result.Code,
                Value = result
            };
        }

        public static ObjectResult ToResponse<T>(this IResult<T> result)
        {
            if (result.Succeeded)
            {
                return new ObjectResult(default)
                {
                    StatusCode = (int)result.Code,
                    Value = !string.IsNullOrEmpty(result.Message) ? result : result.Data
                };
            }

            return new ObjectResult(default)
            {
                StatusCode = (int)result.Code,
                Value = result
            };
        }
    }
}
