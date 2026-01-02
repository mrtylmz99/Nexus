using Microsoft.AspNetCore.Mvc;
using Nexus.Application.DTOs.User;
using Nexus.Application.Interfaces;

namespace Nexus.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateUserDto userDto)
    {
        var createdUser = await _userService.CreateUserAsync(userDto);
        return Ok(createdUser);
    }
}
