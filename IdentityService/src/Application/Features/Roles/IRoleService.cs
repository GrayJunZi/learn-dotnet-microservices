using ResponseWrapperLibrary.Models.Requests.Identity;
using ResponseWrapperLibrary.Wrappers;

namespace Application.Features.Roles;

public interface IRoleService
{
    /// <summary>
    /// 创建角色
    /// </summary>
    /// <param name="createRole"></param>
    /// <returns></returns>
    Task<IResponseWrapper> CreateRoleAsync(CreateRoleRequest createRole);
    /// <summary>
    /// 更新角色
    /// </summary>
    /// <param name="updateRole"></param>
    /// <returns></returns>
    Task<IResponseWrapper> UpdateRoleAsync(UpdateRoleRequest updateRole);
    /// <summary>
    /// 删除角色
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    Task<IResponseWrapper> DeleteRoleAsync(string roleId);
    /// <summary>
    /// 获取所有角色
    /// </summary>
    /// <returns></returns>
    Task<IResponseWrapper> GetRolesAsync();
    /// <summary>
    /// 根据角色Id获取角色信息
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    Task<IResponseWrapper> GetRoleByIdAsync(string roleId);
    /// <summary>
    /// 获取角色权限
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    Task<IResponseWrapper> GetRolePermissionsAsync(string roleId);
    /// <summary>
    /// 更新角色权限
    /// </summary>
    /// <param name="updateRoleClaims"></param>
    /// <returns></returns>
    Task<IResponseWrapper> UpdateRolePermissionsAsync(UpdateRoleClaimsRequest updateRoleClaims);
}
