using Host.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Zord.DomainActiveDirectory.Interfaces;
using Zord.Identity;
using Zord.Identity.EntityFrameworkCore.Options;

namespace Host.Controllers;

[Route("[controller]")]
[ApiController]
public class TokenController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IActiveDirectoryService _activeDirectoryService;
    private readonly ITokenService _tokenService;
    private readonly ClaimTypeOptions _claimTypes;
    private readonly JwtOptions _jwtOptions;

    public TokenController(
        IUserService userService,
        IActiveDirectoryService activeDirectoryService,
        ITokenService tokenService,
        IOptions<ClaimTypeOptions> claimTypes,
        IOptions<JwtOptions> jwtConfig)
    {
        _userService = userService;
        _activeDirectoryService = activeDirectoryService;
        _tokenService = tokenService;
        _claimTypes = claimTypes.Value;
        _jwtOptions = jwtConfig.Value;
    }

    [HttpGet("claims")]
    public IActionResult GetClaims()
    {
        return Ok(_claimTypes);
    }

    [HttpGet("jwt")]
    public IActionResult GetJwt()
    {
        return Ok(_jwtOptions);
    }

    [HttpPost]
    public async Task<IActionResult> GetAsync([FromBody] GetTokenRequest request)
    {
        var user = await _userService.GetByUserNameAsync(request.ClientId);
        if (user.Succeeded is false)
            return Ok(user);

        if (user.Data.UseDomainPassword)
        {
            var domainLogin = await _activeDirectoryService.CheckPasswordSignInAsync(request.ClientId, request.ClientSecret);

            if (domainLogin.Succeeded is false)
                return Ok(domainLogin);
        }
        else
        {
            var localLogin = await _userService.CheckPasswordByUserNameAsync(request.ClientId, request.ClientSecret);

            if (localLogin.Succeeded is false)
                return Ok(localLogin);
        }

        return Ok(await _tokenService.GetTokenByUserNameAsync(request.ClientId));
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshAsync([FromBody] RefreshTokenRequest request)
    {
        return Ok(await _tokenService.RefreshTokenAsync(request));
    }
}