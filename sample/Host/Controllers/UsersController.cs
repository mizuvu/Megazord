using Microsoft.AspNetCore.Mvc;
using Zord.Identity;

namespace Host.Controllers;

[Route("[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
    {
        return Ok(await _userService.GetAllAsync(cancellationToken));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync([FromRoute] string id)
    {
        return Ok(await _userService.GetByIdAsync(id));
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] CreateUserRequest request)
    {
        return Ok(await _userService.CreateAsync(request));
    }

    [HttpPut]
    public async Task<IActionResult> PutAsync([FromBody] UserDto request)
    {
        return Ok(await _userService.UpdateAsync(request));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(string id)
    {
        return Ok(await _userService.DeleteAsync(id));
    }

    [HttpPut("force-password")]
    public async Task<IActionResult> ForcePasswordAsync([FromBody] ForcePasswordRequest request)
    {
        return Ok(await _userService.ForcePasswordAsync(request));
    }
}