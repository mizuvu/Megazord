using System.Threading.Tasks;
using Zord.Result;

namespace Zord.Identity
{
    public interface ITokenService
    {
        /// <summary>
        /// Generate access token by UserName
        /// </summary>
        Task<IResult<TokenDto>> GetTokenByUserNameAsync(string userName);

        /// <summary>
        /// Generate access token from old token and refresh token
        /// </summary>
        Task<IResult<TokenDto>> RefreshTokenAsync(RefreshTokenRequest request);
    }
}