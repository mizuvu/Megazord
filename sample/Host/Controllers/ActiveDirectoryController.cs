using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Zord.DomainActiveDirectory.Options;

namespace Host.Controllers;

[Route("[controller]")]
[ApiController]
public class ActiveDirectoryController : ControllerBase
{
    private readonly DomainOptions _domain;
    private readonly LdapOptions _ldap;

    public ActiveDirectoryController(
        IOptions<DomainOptions> domain,
        IOptions<LdapOptions> ldap)
    {
        _domain = domain.Value;
        _ldap = ldap.Value;
    }

    [HttpGet("domain")]
    public IActionResult GetClaims()
    {
        return Ok(_domain);
    }

    [HttpGet("ldap")]
    public IActionResult GetJwt()
    {
        return Ok(_ldap);
    }
}