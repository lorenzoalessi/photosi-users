using Microsoft.AspNetCore.Mvc;
using PhotosiUsers.Service;

namespace PhotosiUsers.Controllers;

[Route("api/v1/user")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> Get() => Ok(await _userService.GetAsync());
}