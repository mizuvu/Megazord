using Novell.Directory.Ldap;
using System.DirectoryServices;
using System.Runtime.Versioning;
using Zord.DomainActiveDirectory.Configurations;
using Zord.DomainActiveDirectory.Dtos;
using Zord.DomainActiveDirectory.Interfaces;

namespace Zord.DomainActiveDirectory.Services;

[SupportedOSPlatform("windows")]
public class LDAPService : IActiveDirectoryService
{
    private readonly LdapConfiguration _settings;

    public LDAPService(LdapConfiguration settings)
    {
        _settings = settings;
    }

    [SupportedOSPlatform("windows")]
    public Task<IResult> CheckPasswordSignInAsync(string userName, string password)
    {
        IResult result = Result.Result.Unauthorized("Invalid credentials.");

        try
        {
            if (string.IsNullOrEmpty(password.Trim()))
            {
                return Task.FromResult(result);
            }
            // create LDAP connection
            var ldapConn = new LdapConnection() { SecureSocketLayer = false };

            // create socket connect to server
            ldapConn.Connect(_settings.IpServer, _settings.Port);

            // bind user dm & password
            ldapConn.Bind(userName + _settings.DomainUser, password);

            result = Result.Result.Success();
        }
        catch (Exception ex)
        {
            result = Result.Result.Error(ex.Message);
        }

        return Task.FromResult(result);
    }

    public IResult ChangePasswordAsync(string userName, string oldPassword, string newPassword)
    {
        var sPath = _settings.Connection; //This is if your domain was my.domain.com
        var de = new DirectoryEntry(sPath, _settings.UserName, _settings.Password, AuthenticationTypes.Secure);
        var ds = new DirectorySearcher(de);
        string qry = string.Format("(&(objectCategory=person)(objectClass=user)(sAMAccountName={0}))", userName);
        ds.Filter = qry;
        try
        {
            var sr = ds.FindOne();
            if (sr is null)
            {
                return Result.Result.Error("User not found on domain.");
            }

            DirectoryEntry user = sr.GetDirectoryEntry();
            user.Invoke("SetPassword", new object[] { newPassword });
            user.CommitChanges();

            return Result.Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Result.Error(ex.Message);
        }
    }

    public Task<IResult<DomainUserDto>> GetByUserNameAsync(string userName)
    {
        throw new NotImplementedException();
    }
}