using Microsoft.AspNetCore.Identity;

namespace Zord.Identity.EntityFrameworkCore.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    private async Task<IResult> CheckPasswordAsync(ApplicationUser user, string password)
    {
        var checkPassword = await _userManager.CheckPasswordAsync(user, password);

        if (checkPassword)
            return Result.Result.Success();

        return Result.Result.Error("Invalid credentials");
    }

    public async Task<IResult<IEnumerable<UserDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var users = await _userManager.Users
            .AsNoTracking()
            .OrderBy(x => x.CreatedOn)
            .ThenBy(x => x.UserName)
            .MapToDto()
            .ToListAsync(cancellationToken);

        return Result<IEnumerable<UserDto>>.Object(users);
    }

    public async Task<IResult<UserDto>> GetByIdAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user == null)
            return Result<UserDto>.ObjectNotFound("User", id);

        var dto = user.MapToDto();
        dto.Roles = await _userManager.GetRolesAsync(user);

        return Result<UserDto>.Object(dto);
    }

    public async Task<IResult<UserDto>> GetByUserNameAsync(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);

        if (user == null)
            return Result<UserDto>.ObjectNotFound("User", userName);

        var dto = user.MapToDto();
        dto.Roles = await _userManager.GetRolesAsync(user);

        return Result<UserDto>.Object(dto);
    }

    public async Task<IResult> CheckPasswordAsync(string id, string password)
    {
        var user = await _userManager.FindByNameAsync(id);

        if (user == null)
            return Result.Result.ObjectNotFound("User", id);

        return await CheckPasswordAsync(user, password);
    }

    public async Task<IResult> CheckPasswordByUserNameAsync(string userName, string password)
    {
        var user = await _userManager.FindByNameAsync(userName);

        if (user == null)
            return Result.Result.ObjectNotFound("User", userName);

        return await CheckPasswordAsync(user, password);
    }

    public async Task<IResult<string>> CreateAsync(CreateUserRequest newUser)
    {
        var entity = new ApplicationUser
        {
            //Id = _authData.NewId(),
            UserName = newUser.UserName,
            Email = newUser.Email,
            PhoneNumber = newUser.PhoneNumber,
            FirstName = newUser.FirstName,
            LastName = newUser.LastName,
            UseDomainPassword = newUser.UseDomainPassword,
        };

        var identityResult = await _userManager.CreateAsync(entity, newUser.Password);

        return identityResult.ToResult(entity.Id);
    }

    public async Task<IResult> UpdateAsync(UserDto updateUser)
    {
        var user = await _userManager.FindByIdAsync(updateUser.Id);

        if (user == null)
            return Result.Result.ObjectNotFound("User", updateUser.Id);

        user.FirstName = updateUser.FirstName;
        user.LastName = updateUser.LastName;
        user.PhoneNumber = updateUser.PhoneNumber;
        user.Email = updateUser.Email;
        user.UseDomainPassword = updateUser.UseDomainPassword;
        user.IsDeleted = updateUser.IsDeleted;
        user.Status.Update(updateUser.Status);

        var updatedResult = await _userManager.UpdateAsync(user);
        if (!updatedResult.Succeeded)
            return updatedResult.ToResult();

        var currentRoles = await _userManager.GetRolesAsync(user);
        var addRoles = updateUser.Roles.Except(currentRoles);
        var removeRoles = currentRoles.Except(updateUser.Roles);

        if (addRoles.Any())
        {
            var addRole = await _userManager.AddToRolesAsync(user, addRoles);
            if (!addRole.Succeeded)
                return addRole.ToResult();
        }

        if (removeRoles.Any())
        {
            var removeRole = await _userManager.RemoveFromRolesAsync(user, removeRoles);
            if (!removeRole.Succeeded)
                return removeRole.ToResult();
        }

        return Result.Result.Success();
    }

    public async Task<IResult> DeleteAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user == null)
            return Result.Result.ObjectNotFound("User", id);

        user.Status.Update(Core.Enums.ActiveStatus.locked);

        var identityResult = await _userManager.DeleteAsync(user);

        return identityResult.ToResult();
    }

    public async Task<IResult> ForcePasswordAsync(ForcePasswordRequest forcePassword)
    {
        var user = await _userManager.FindByIdAsync(forcePassword.Id);

        if (user == null)
            return Result.Result.ObjectNotFound("User", forcePassword.Id);

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        var identityResult = await _userManager.ResetPasswordAsync(user, token, forcePassword.Password);

        return identityResult.ToResult();
    }
}
