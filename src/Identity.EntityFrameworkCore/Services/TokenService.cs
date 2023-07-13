﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Zord.Identity.EntityFrameworkCore.Abstractions;
using Zord.Identity.EntityFrameworkCore.Options;

namespace Zord.Identity.EntityFrameworkCore.Services;

public class TokenService : ITokenService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly ClaimTypeOptions _claimTypes;
    private readonly JwtOptions _jwtOptions;
    private readonly IIdentityDbContext _db;

    public TokenService(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        IOptions<ClaimTypeOptions> claimTypes,
        IOptions<JwtOptions> jwtOptions,
        IIdentityDbContext db)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _claimTypes = claimTypes.Value;
        _jwtOptions = jwtOptions.Value;
        _db = db;
    }

    private async Task<IEnumerable<Claim>> GetClaimsAsync(ApplicationUser user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);
        var roleClaims = new List<Claim>();
        var permissionClaims = new List<Claim>();

        foreach (var role in roles)
        {
            roleClaims.Add(new Claim(ClaimTypes.Role, role));

            var thisRole = await _roleManager.FindByNameAsync(role);
            if (thisRole is null)
                continue;

            var allClaimsForThisRoles = await _roleManager.GetClaimsAsync(thisRole);

            permissionClaims.AddRange(allClaimsForThisRoles);
        }

        var claims = new List<Claim>
        {
            new(_claimTypes.UserId, user.Id ?? string.Empty),
            new(_claimTypes.UserName, user.UserName ?? string.Empty),
            new(_claimTypes.FirstName, user.FirstName ?? string.Empty),
            new(_claimTypes.LastName, user.LastName ?? string.Empty),
            new(_claimTypes.PhoneNumber, user.PhoneNumber ?? string.Empty),
            new(_claimTypes.Email, user.Email ?? string.Empty),
        }
        .Union(userClaims)
        .Union(roleClaims)
        .Union(permissionClaims);
        return claims;
    }

    private async Task<string> GenerateJwtAsync(ApplicationUser user)
    {
        var secret = Encoding.UTF8.GetBytes(_jwtOptions.SecretKey);

        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);

        var claims = await GetClaimsAsync(user);

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            claims: claims,
            expires: DateTime.Now.AddSeconds(_jwtOptions.ExpiresIn),
            signingCredentials: signingCredentials);

        var tokenHandler = new JwtSecurityTokenHandler();

        var encryptedToken = tokenHandler.WriteToken(token);

        return encryptedToken;
    }

    private async Task SaveTokenAsync(string userId,
        string token, DateTimeOffset tokenExpiryTime,
        string refreshToken, DateTimeOffset refreshTokenExpiryTime)
    {
        var entry = await _db.JwtTokens.FindAsync(userId);

        if (entry is not null)
        {
            entry.Token = token;
            entry.TokenExpiryTime = tokenExpiryTime;
            entry.RefreshToken = refreshToken;
            entry.RefreshTokenExpiryTime = refreshTokenExpiryTime;

            await _db.SaveChangesAsync();
        }
        else
        {
            var entity = new JwtToken
            {
                UserId = userId,
                Token = token,
                TokenExpiryTime = tokenExpiryTime,
                RefreshToken = refreshToken,
                RefreshTokenExpiryTime = refreshTokenExpiryTime,
            };

            await _db.JwtTokens.AddAsync(entity);
            await _db.SaveChangesAsync();
        }
    }

    private async Task<JwtToken?> GetTokenAsync(string userId, string token)
    {
        return await _db.JwtTokens.FirstOrDefaultAsync(x => x.UserId == userId && x.Token == token);
    }

    public async Task<IResult<TokenDto>> GetTokenAsync(ApplicationUser? user)
    {
        if (user == null
            || user.Status.IsActive is false
            || user.IsDeleted)
            return Result<TokenDto>.Unauthorized("Invalid credentials.");

        var token = await GenerateJwtAsync(user);
        var tokenExpiryTime = _jwtOptions.ExpiresIn;
        var refreshToken = JwtHelper.GenerateRefreshToken();
        var refreshTokenExpiryTime = _jwtOptions.RefreshTokenExpiresIn;

        await SaveTokenAsync(
            user.Id,
            token, DateTimeOffset.Now.AddSeconds(tokenExpiryTime),
            refreshToken, DateTimeOffset.Now.AddSeconds(refreshTokenExpiryTime));

        var dto = new TokenDto
        {
            AccessToken = token,
            ExpiresIn = _jwtOptions.ExpiresIn,
            RefreshToken = refreshToken,
            RefreshTokenExpiresIn = refreshTokenExpiryTime,
        };

        return Result<TokenDto>.Object(dto);
    }

    public async Task<IResult<TokenDto>> GetTokenByIdAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        return await GetTokenAsync(user);
    }

    public async Task<IResult<TokenDto>> GetTokenByUserNameAsync(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);

        return await GetTokenAsync(user);
    }

    public async Task<IResult<TokenDto>> RefreshTokenAsync(RefreshTokenRequest request)
    {
        // get UserPrincipal from expired token
        var userPrincipal = JwtHelper.GetPrincipalFromExpiredToken(
            request.AccessToken,
            _jwtOptions.SecretKey,
            _jwtOptions.Issuer);

        // get userID from UserPrincipal
        var userId = userPrincipal.FindFirstValue(_claimTypes.UserId);
        if (string.IsNullOrEmpty(userId))
            return Result<TokenDto>.Unauthorized("Invalid token.");

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null || user.Status.IsActive is false)
            return Result<TokenDto>.Unauthorized("Invalid token.");

        // check refresh token is exist or not out of lifetime
        var userToken = await GetTokenAsync(userId, request.AccessToken);
        if (userToken is null
            || userToken.RefreshToken != request.RefreshToken
            || userToken.RefreshTokenExpiryTime <= DateTime.Now)
            return Result<TokenDto>.Unauthorized("Invalid token.");

        var token = await GenerateJwtAsync(user);
        var tokenExpiryTime = _jwtOptions.ExpiresIn;
        var refreshToken = JwtHelper.GenerateRefreshToken();
        var refreshTokenExpiryTime = _jwtOptions.RefreshTokenExpiresIn;

        // update user login info
        await SaveTokenAsync(
            user.Id,
            token, DateTimeOffset.Now.AddSeconds(tokenExpiryTime),
            refreshToken, DateTimeOffset.Now.AddSeconds(refreshTokenExpiryTime));

        var dto = new TokenDto
        {
            AccessToken = token,
            ExpiresIn = _jwtOptions.ExpiresIn,
            RefreshToken = refreshToken,
            RefreshTokenExpiresIn = refreshTokenExpiryTime,
        };

        return Result<TokenDto>.Object(dto);
    }
}
