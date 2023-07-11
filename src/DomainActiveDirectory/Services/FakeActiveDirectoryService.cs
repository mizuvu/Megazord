using Zord.DomainActiveDirectory.Dtos;
using Zord.DomainActiveDirectory.Interfaces;

namespace Zord.DomainActiveDirectory.Services;

public class FakeActiveDirectoryService : IActiveDirectoryService
{
    public Task<IResult> CheckPasswordSignInAsync(string userName, string password)
    {
        IResult result = Result.Result.Unauthorized("Invalid credentials.");

        return Task.FromResult(result);
    }

    public Task<IResult<DomainUserDto>> GetByUserNameAsync(string userName)
    {
        IResult<DomainUserDto> result = Result<DomainUserDto>.ObjectNotFound("DomainUser", userName);

        return Task.FromResult(result);
    }
}
