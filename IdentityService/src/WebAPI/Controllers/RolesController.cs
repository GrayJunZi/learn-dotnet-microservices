using Application.Features.Roles.Commands;
using Application.Features.Roles.Queries;
using AuthLibrary.Attributes;
using AuthLibrary.Constants.Authentication;
using Microsoft.AspNetCore.Mvc;
using ResponseWrapperLibrary.Models.Requests.Identity;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
public class RolesController : BaseController<RolesController>
{
    [HttpPost]
    [MustHavePermission(AppService.Identity, AppFeature.Roles, AppAction.Create)]
    public async Task<IActionResult> CreateRoleAsync([FromBody] CreateRoleRequest createRole)
    {
        var response = await Sender.Send(new CreateRoleCommand { CreateRole = createRole });
        if (!response.IsSuccessful)
            return BadRequest(response);

        return Ok(response);
    }

    [HttpPut]
    [MustHavePermission(AppService.Identity, AppFeature.Roles, AppAction.Update)]
    public async Task<IActionResult> UpdateRoleAsync([FromBody] UpdateRoleRequest updateRole)
    {
        var response = await Sender.Send(new UpdateRoleCommand { UpdateRole = updateRole });
        if (!response.IsSuccessful)
            return NotFound(response);

        return Ok(response);
    }

    [HttpDelete("{roleId}")]
    [MustHavePermission(AppService.Identity, AppFeature.Roles, AppAction.Delete)]
    public async Task<IActionResult> DeleteRoleAsync(string roleId)
    {
        var response = await Sender.Send(new DeleteRoleCommand { RoleId = roleId });
        if (!response.IsSuccessful)
            return NotFound(response);

        return Ok(response);
    }

    [HttpGet("all")]
    [MustHavePermission(AppService.Identity, AppFeature.Roles, AppAction.Read)]
    public async Task<IActionResult> GetRolesAsync()
    {
        var response = await Sender.Send(new GetRolesQuery());
        if (!response.IsSuccessful)
            return NotFound(response);

        return Ok(response);
    }

    [HttpGet("{userId}")]
    [MustHavePermission(AppService.Identity, AppFeature.Roles, AppAction.Read)]
    public async Task<IActionResult> GetRoleByIdAsync(string roleId)
    {
        var response = await Sender.Send(new GetRoleByIdQuery { RoleId = roleId });
        if (!response.IsSuccessful)
            return NotFound(response);

        return Ok(response);
    }

    [HttpGet("permissions/{roleId}")]
    [MustHavePermission(AppService.Identity, AppFeature.Roles, AppAction.Read)]
    public async Task<IActionResult> GetPermissionsAsync(string roleId)
    {
        var response = await Sender.Send(new GetPermissionsQuery { RoleId = roleId });
        if (!response.IsSuccessful)
            return NotFound(response);

        return Ok(response);
    }

    [HttpGet("update-permissions")]
    [MustHavePermission(AppService.Identity, AppFeature.Roles, AppAction.Update)]
    public async Task<IActionResult> UpdateRolePermissionsAsync([FromBody] UpdateRoleClaimsRequest updateRoleClaims)
    {
        var response = await Sender.Send(new UpdateRolePermissionCommand { UpdateRoleClaims = updateRoleClaims });
        if (!response.IsSuccessful)
            return NotFound(response);

        return Ok(response);
    }
}
