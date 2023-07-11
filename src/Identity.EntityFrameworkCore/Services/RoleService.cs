using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Zord.Identity.EntityFrameworkCore.Services;

public class RoleService : IRoleService
{
    private readonly RoleManager<ApplicationRole> _roleManager;

    public RoleService(RoleManager<ApplicationRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<IResult<IEnumerable<RoleDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var roles = await _roleManager.Roles
            .AsNoTracking()
            .MapToDto()
            .ToListAsync(cancellationToken);

        return Result<IEnumerable<RoleDto>>.Object(roles);
    }

    public async Task<IResult<RoleDto>> GetByIdAsync(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);

        if (role == null)
            return Result<RoleDto>.ObjectNotFound("Role", id);

        var result = role.MapToDto();

        var claims = await _roleManager.GetClaimsAsync(role);

        result.Claims = claims.Select(s => new ClaimDto
        {
            Type = s.Type,
            Value = s.Value
        });

        return Result<RoleDto>.Object(result);
    }

    public async Task<IResult<RoleDto>> GetByNameAsync(string id)
    {
        var role = await _roleManager.FindByNameAsync(id);

        if (role == null)
            return Result<RoleDto>.ObjectNotFound("Role", id);

        var result = role.MapToDto();

        var claims = await _roleManager.GetClaimsAsync(role);

        result.Claims = claims.Select(s => new ClaimDto
        {
            Type = s.Type,
            Value = s.Value
        });

        return Result<RoleDto>.Object(result);
    }

    public async Task<IResult<string>> CreateAsync(CreateRoleRequest request)
    {
        var role = new ApplicationRole
        {
            Name = request.Name.Trim().ToLower(),
            Description = request.Description,
        };

        var result = await _roleManager.CreateAsync(role);

        return result.ToResult(role.Id);
    }

    public async Task<IResult> UpdateAsync(RoleDto request)
    {
        var role = await _roleManager.FindByIdAsync(request.Id);

        if (role == null)
            return Result.Result.ObjectNotFound("Role", request.Id);

        role.Name = request.Name.Trim().ToLower();
        role.Description = request.Description;

        var result = await _roleManager.UpdateAsync(role);

        // get claims of role
        var roleClaims = await _roleManager.GetClaimsAsync(role);

        // remove claims not in request list
        foreach (var roleClaim in roleClaims)
        {
            // hold this claim if claim contains in request update list
            var hold = request.Claims.Any(a => a.Type == roleClaim.Type && a.Value == roleClaim.Value);

            if (hold) continue;

            // remove claim if not in request update list
            await _roleManager.RemoveClaimAsync(role, roleClaim);
        }

        // add new claims in request list & skip exist claims
        foreach (var claim in request.Claims)
        {
            var owned = roleClaims.Any(a => a.Type == claim.Type && a.Value == claim.Value);

            if (owned) continue;

            // add new claim if role has not own it
            var newClaim = new Claim(claim.Type, claim.Value);
            await _roleManager.AddClaimAsync(role, newClaim);
        }

        return result.ToResult();
    }

    public async Task<IResult> DeleteAsync(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);

        if (role == null)
            return Result.Result.ObjectNotFound("Role", id);

        //Check claim exist for role
        var claimsByRole = await _roleManager.GetClaimsAsync(role);

        if (claimsByRole.Any())
            return Result.Result.Error("Role has already setup claim.");

        var result = await _roleManager.DeleteAsync(role);

        return result.ToResult();
    }

    /*
    public async Task<IEnumerable<UserDto>> GetUsersAsync(string roleName,
        CancellationToken cancellationToken = default)
    {
        var role = await _roleManager.FindByNameAsync(roleName);

        if (role == null)
            return new Result<IEnumerable<UserDto>>.ObjectNotFound();

        var userIdsInRole = await _db.ApplicationUserRoles
            .Where(x => x.RoleId == role.Id)
            .Select(x => x.UserId)
            .ToListAsync(cancellationToken);

        return await _userManager.Users
            .Where(x => userIdsInRole.Contains(x.Id))
            .MapToDto()
            .ToListResultAsync(cancellationToken);
    }
    */
}
