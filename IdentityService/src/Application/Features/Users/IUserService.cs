using ResponseWrapperLibrary.Models.Requests.Identity;
using ResponseWrapperLibrary.Wrappers;

namespace Application.Features.Users;

public interface IUserService
{
    /// <summary>
    /// 注册用户
    /// </summary>
    /// <param name="userRegistration"></param>
    /// <returns></returns>
    Task<IResponseWrapper> RegisterUserAsync(UserRegistrationRequest userRegistration);
    /// <summary>
    /// 更新用户
    /// </summary>
    /// <param name="updateUser"></param>
    /// <returns></returns>
    Task<IResponseWrapper> UpdateUserAsync(UpdateUserRequest updateUser);
    /// <summary>
    /// 根据用户Id获取用户信息
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<IResponseWrapper> GetUserByIdAsync(string userId);
    /// <summary>
    /// 获取所有用户信息
    /// </summary>
    /// <returns></returns>
    Task<IResponseWrapper> GetAllUsersAsync();
    /// <summary>
    /// 修改用户密码
    /// </summary>
    /// <param name="changePassword"></param>
    /// <returns></returns>
    Task<IResponseWrapper> ChangeUserPasswordAsync(ChangePasswordRequest changePassword);
    /// <summary>
    /// 修改用户状态
    /// </summary>
    /// <param name="changeUserStatus"></param>
    /// <returns></returns>
    Task<IResponseWrapper> ChangeUserStatusAsync(ChangeUserStatusRequest changeUserStatus);
    /// <summary>
    /// 获取用户角色
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<IResponseWrapper> GetUserRolesAsync(string userId);
    /// <summary>
    /// 更新用户角色
    /// </summary>
    /// <param name="updateUserRoles"></param>
    /// <returns></returns>
    Task<IResponseWrapper> UpdateUserRolesAsync(UpdateUserRolesRequest updateUserRoles);
    /// <summary>
    /// 根据邮箱获取用户信息
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    Task<IResponseWrapper> GetUserByEmailAsync(string email);
}
