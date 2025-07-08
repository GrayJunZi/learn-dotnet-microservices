using Application.Features.Roles;
using AuthLibrary.Constants.Authentication;
using Infrastructure.Constants;
using Infrastructure.Contexts;
using Infrastructure.Models;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ResponseWrapperLibrary.Models.Requests.Identity;
using ResponseWrapperLibrary.Models.Responses.Identity;
using ResponseWrapperLibrary.Wrappers;

namespace Infrastructure.Services;

public class RoleService(
    RoleManager<ApplicationRole> roleManager,
    UserManager<ApplicationUser> userManager,
    ApplicationDbContext applicationDbContext) : IRoleService
{

    /// <summary>
    /// 创建角色
    /// </summary>
    /// <param name="createRole"></param>
    /// <returns></returns>
    public async Task<IResponseWrapper> CreateRoleAsync(CreateRoleRequest createRole)
    {
        var role = await roleManager.FindByNameAsync(createRole.Name);
        if (role != null)
            return await ResponseWrapper.FailAsync("Role already exists.");

        var newRole = new ApplicationRole
        {
            Name = createRole.Name,
            Description = createRole.Description,
        };

        var identityResult = await roleManager.CreateAsync(newRole);
        if (!identityResult.Succeeded)
            return await ResponseWrapper.FailAsync(identityResult.GetIdentityResultErrorDescriptions());

        return await ResponseWrapper.SuccessAsync("Role created successfully.");
    }

    /// <summary>
    /// 更新角色
    /// </summary>
    /// <param name="updateRole"></param>
    /// <returns></returns>
    public async Task<IResponseWrapper> UpdateRoleAsync(UpdateRoleRequest updateRole)
    {
        var role = await roleManager.FindByIdAsync(updateRole.RoleId);
        if (role == null)
            return await ResponseWrapper.FailAsync("Role does not exists.");

        if (role.Name == AppRoles.Admin)
            return await ResponseWrapper.FailAsync("Cannot update Admin role.");

        role.Name = updateRole.Name;
        role.Description = updateRole.Description;
        var identityResult = await roleManager.UpdateAsync(role);
        if (!identityResult.Succeeded)
            return await ResponseWrapper.FailAsync(identityResult.GetIdentityResultErrorDescriptions());

        return await ResponseWrapper.SuccessAsync("Role updated successfully.");
    }

    /// <summary>
    /// 删除角色
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    public async Task<IResponseWrapper> DeleteRoleAsync(string roleId)
    {
        var role = await roleManager.FindByIdAsync(roleId);
        if (role == null)
            return await ResponseWrapper.FailAsync("Role does not exists.");

        if (role.Name == AppRoles.Admin)
            return await ResponseWrapper.FailAsync("Cannot delete Admin role.");


        var users = await userManager.Users.ToListAsync();
        foreach (var user in users)
        {
            if (await userManager.IsInRoleAsync(user, role.Name))
            {
                return await ResponseWrapper.FailAsync($"Role: {role.Name} is currently assigned to a user.");
            }
        }

        var identityResult = await roleManager.DeleteAsync(role);
        if (!identityResult.Succeeded)
            return await ResponseWrapper.FailAsync(identityResult.GetIdentityResultErrorDescriptions());

        return await ResponseWrapper.SuccessAsync("Role deleted successfully.");
    }

    /// <summary>
    /// 获取所有角色
    /// </summary>
    /// <returns></returns>
    public async Task<IResponseWrapper> GetRolesAsync()
    {
        var roles = await roleManager.Roles.ToListAsync();
        if (roles == null || roles.Count == 0)
            return await ResponseWrapper.FailAsync("No roles were found.");

        var mappedRoles = roles.Adapt<List<RoleResponse>>();
        return await ResponseWrapper<List<RoleResponse>>.SuccessAsync(mappedRoles);
    }

    /// <summary>
    /// 根据角色Id获取角色信息
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    public async Task<IResponseWrapper> GetRoleByIdAsync(string roleId)
    {
        var role = await roleManager.FindByIdAsync(roleId);
        if (role == null)
            return await ResponseWrapper.FailAsync("Role does not exists.");

        var mappedRole = role.Adapt<RoleResponse>();
        return await ResponseWrapper<RoleResponse>.SuccessAsync(mappedRole);
    }

    /// <summary>
    /// 获取角色权限
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    public async Task<IResponseWrapper> GetRolePermissionsAsync(string roleId)
    {
        var role = await roleManager.FindByIdAsync(roleId);
        if (role == null)
            return await ResponseWrapper.FailAsync("Role does not exists.");

        var allPermissions = AppPermissions.AllPermissions;
        var allPermissionNames = allPermissions.Select(x => x.Name);

        var roleClaimResponse = new RoleClaimResponse
        {
            Role = new RoleResponse
            {
                Id = roleId,
                Name = role.Name,
                Description = role.Description,
            },
            RoleClaims = [],
        };

        var currentlyAssignedClaims = await GetAllClaimsForRolesAsync(roleId);

        var currentlyAssignedClaimsValues = currentlyAssignedClaims
            .Select(x => x.ClaimValue);

        var currentlyAssignedRoleClaimsNames = allPermissionNames.Intersect(currentlyAssignedClaimsValues);

        foreach (var permission in allPermissions)
        {
            roleClaimResponse.RoleClaims.Add(new RoleClaimViewModel
            {
                RoleId = roleId,
                ClaimType = AppClaim.Permission,
                ClaimValue = permission.Name,
                Description = permission.Description,
                Group = permission.Group,
                IsAssisgnedToRole = currentlyAssignedRoleClaimsNames.Any(x => x == permission.Name),
            });
        }

        return await ResponseWrapper<RoleClaimResponse>.SuccessAsync(roleClaimResponse);
    }

    /// <summary>
    /// 更新角色权限
    /// </summary>
    /// <param name="updateRoleClaims"></param>
    /// <returns></returns>
    public async Task<IResponseWrapper> UpdateRolePermissionsAsync(UpdateRoleClaimsRequest updateRoleClaims)
    {
        var role = await roleManager.FindByIdAsync(updateRoleClaims.RoleId);
        if (role == null)
            return await ResponseWrapper.FailAsync("Role does not exists.");

        if (role.Name == AppRoles.Admin)
            return await ResponseWrapper.FailAsync("Cannot change permissions for this role.");

        var toBeAssignedPermissions = updateRoleClaims.RoleClaims
            .Where(x => x.IsAssisgnedToRole)
            .ToList();

        var currentlyAssignedPermissions = await roleManager.GetClaimsAsync(role);
        foreach (var claim in currentlyAssignedPermissions)
        {
            await roleManager.RemoveClaimAsync(role, claim);
        }

        var mappedRoleClaims = toBeAssignedPermissions.Adapt<List<ApplicationRoleClaim>>();
        await applicationDbContext.RoleClaims.AddRangeAsync(mappedRoleClaims);
        await applicationDbContext.SaveChangesAsync();

        return await ResponseWrapper.SuccessAsync("Role permissions updated successfully.");
    }


    private async Task<List<RoleClaimViewModel>> GetAllClaimsForRolesAsync(string roleId)
    {
        var roleClaims = await applicationDbContext.RoleClaims
            .Where(x => x.RoleId == roleId)
            .ToListAsync();

        if (roleClaims == null || roleClaims.Count == 0)
            return [];

        var mappedRoleClaims = roleClaims.Adapt<List<RoleClaimViewModel>>();
        return mappedRoleClaims;
    }
}
