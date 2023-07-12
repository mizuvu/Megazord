using Microsoft.Extensions.Options;
using System.DirectoryServices.AccountManagement;
using System.Runtime.Versioning;
using Zord.DomainActiveDirectory.Dtos;
using Zord.DomainActiveDirectory.Interfaces;
using Zord.DomainActiveDirectory.Options;

namespace Zord.DomainActiveDirectory.Services;

[SupportedOSPlatform("windows")]
public class ActiveDirectoryService : IActiveDirectoryService
{
    private readonly DomainOptions _domain;

    public ActiveDirectoryService(IOptions<DomainOptions> domain)
    {
        _domain = domain.Value;
    }

    public Task<IResult> CheckPasswordSignInAsync(string userName, string password)
    {
        // Create a context that will allow you to connect to your Domain Controller
        using (var adContext = new PrincipalContext(ContextType.Domain, _domain.Name))
        {
            IResult result = Result.Result.Unauthorized("Invalid credentials.");

            // find a user
            UserPrincipal user = UserPrincipal.FindByIdentity(adContext, userName);
            if (user is not null && !user.IsAccountLockedOut())
            {
                //Check user is blocked
                var validate = adContext.ValidateCredentials(userName, password);
                if (validate)
                {
                    result = Result.Result.Success();
                }
            }

            return Task.FromResult(result);
        };
    }

    public Task<IResult<DomainUserDto>> GetByUserNameAsync(string userName)
    {
        using var adContext = new PrincipalContext(ContextType.Domain, _domain.Name);
        {
            var adUser = UserPrincipal.FindByIdentity(adContext, userName);

            IResult<DomainUserDto> result = Result<DomainUserDto>.ObjectNotFound("DomainUser", userName);

            if (adUser != null)
            {
                result = Result<DomainUserDto>.Object(new DomainUserDto
                {
                    UserName = adUser.Name,
                    FirstName = adUser.GivenName,
                    LastName = adUser.Surname,
                    PhoneNumber = adUser.VoiceTelephoneNumber,
                    Email = adUser.EmailAddress,
                });
            }

            return Task.FromResult(result);
        }
    }
}
