using Application.Features.Users.Commands;
using Application.Features.Users.Queries;
using AuthLibrary.Attributes;
using AuthLibrary.Constants.Authentication;
using Microsoft.AspNetCore.Mvc;
using ResponseWrapperLibrary.Models.Requests.Identity;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
public class UsersController : BaseController<UsersController>
{
    [HttpPost("register")]
    [MustHavePermission(AppService.Identity, AppFeature.Users, AppAction.Create)]
    public async Task<IActionResult> RegisterUserAsync(UserRegistrationRequest userRegistration)
    {
        var response = await Sender.Send(new UserRegistrationCommand { UserRegistration = userRegistration });
        if (!response.IsSuccessful)
            return BadRequest(response);

        return Ok(response);
    }

    [HttpGet("{userId}")]
    [MustHavePermission(AppService.Identity, AppFeature.Users, AppAction.Read)]
    public async Task<IActionResult> GetUserByIdAsync(string userId)
    {
        var response = await Sender.Send(new GetUserByIdQuery { UserId = userId });
        if (!response.IsSuccessful)
            return NotFound(response);

        return Ok(response);
    }

    [HttpPut]
    [MustHavePermission(AppService.Identity, AppFeature.Users, AppAction.Update)]
    public async Task<IActionResult> UpdateUserDetailsAsync([FromBody] UpdateUserRequest updateUser)
    {
        var response = await Sender.Send(new UpdateUserCommand { UpdateUser = updateUser });
        if (!response.IsSuccessful)
            return NotFound(response);

        return Ok(response);
    }

    [HttpPut("change-password")]
    [MustHavePermission(AppService.Identity, AppFeature.Users, AppAction.Update)]
    public async Task<IActionResult> ChangeUserPasswordAsync([FromBody] ChangePasswordRequest changePassword)
    {
        var response = await Sender.Send(new ChangeUserPasswordCommand { ChangePassword = changePassword });
        if (!response.IsSuccessful)
            return NotFound(response);

        return Ok(response);
    }

    [HttpPut("change-status")]
    [MustHavePermission(AppService.Identity, AppFeature.Users, AppAction.Update)]
    public async Task<IActionResult> ChangeUserStatusAsync([FromBody] ChangeUserStatusRequest changeUserStatus)
    {
        var response = await Sender.Send(new ChangeUserStatusCommand { ChangeUserStatus = changeUserStatus });
        if (!response.IsSuccessful)
            return NotFound(response);

        return Ok(response);
    }
}