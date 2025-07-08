using Application.Features.Users;
using Infrastructure.Constants;
using Infrastructure.Models;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ResponseWrapperLibrary.Models.Requests.Identity;
using ResponseWrapperLibrary.Models.Responses.Identity;
using ResponseWrapperLibrary.Wrappers;

namespace Infrastructure.Services;

public class UserService(
    UserManager<ApplicationUser> userManager,
    RoleManager<ApplicationRole> roleManager) : IUserService
{
    /// <summary>
    /// 注册用户
    /// </summary>
    /// <param name="userRegistration"></param>
    /// <returns></returns>
    public async Task<IResponseWrapper> RegisterUserAsync(UserRegistrationRequest userRegistration)
    {
        var user = await userManager.FindByEmailAsync(userRegistration.Email);
        if (user is not null)
            return await ResponseWrapper.FailAsync("Email address already taken.");

        var newUser = new ApplicationUser
        {
            Name = userRegistration.Name,
            Email = userRegistration.Email,
            UserName = userRegistration.UserName,
            PhoneNumber = userRegistration.PhoneNumber,
            IsActive = userRegistration.ActivateUser,
            EmailConfirmed = userRegistration.AutoConfirmEmail,
        };

        // 生成密码
        var password = new PasswordHasher<ApplicationUser>();
        newUser.PasswordHash = password.HashPassword(newUser, userRegistration.Password);

        // 创建用户
        var identityResult = await userManager.CreateAsync(newUser);
        if (!identityResult.Succeeded)
            return await ResponseWrapper.FailAsync(identityResult.GetIdentityResultErrorDescriptions());

        // 为用户添加角色
        identityResult = await userManager.AddToRoleAsync(newUser, AppRoles.Basic);
        if (!identityResult.Succeeded)
            return await ResponseWrapper.FailAsync(identityResult.GetIdentityResultErrorDescriptions());

        // 注册成功
        return await ResponseWrapper.SuccessAsync("User registered successfully.");
    }


    /// <summary>
    /// 更新用户
    /// </summary>
    /// <param name="updateUser"></param>
    /// <returns></returns>
    public async Task<IResponseWrapper> UpdateUserAsync(UpdateUserRequest updateUser)
    {
        // 获取用户信息
        var user = await userManager.FindByIdAsync(updateUser.UserId);
        if (user == null)
            return await ResponseWrapper.FailAsync("User does not exists");

        user.Name = updateUser.Name;
        user.PhoneNumber = updateUser.PhoneNumber;
        var identityUser = await userManager.UpdateAsync(user);
        if (!identityUser.Succeeded)
            return await ResponseWrapper.FailAsync(identityUser.GetIdentityResultErrorDescriptions());

        return await ResponseWrapper.SuccessAsync("User updated successfully.");
    }

    /// <summary>
    /// 根据用户Id获取用户信息
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<IResponseWrapper> GetUserByIdAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
            return await ResponseWrapper.FailAsync("User does not exists");

        var mappedUser = user.Adapt<UserResponse>();
        return await ResponseWrapper<UserResponse>.SuccessAsync(mappedUser);
    }

    /// <summary>
    /// 获取所有用户信息
    /// </summary>
    /// <returns></returns>
    public async Task<IResponseWrapper> GetAllUsersAsync()
    {
        var users = await userManager.Users.ToListAsync();
        if (users == null || users.Count == 0)
            return await ResponseWrapper.FailAsync("No Users were found.");

        var mappedUsers = users.Adapt<List<UserResponse>>();
        return await ResponseWrapper<List<UserResponse>>.SuccessAsync(mappedUsers);
    }

    /// <summary>
    /// 修改用户密码
    /// </summary>
    /// <param name="changePassword"></param>
    /// <returns></returns>
    public async Task<IResponseWrapper> ChangeUserPasswordAsync(ChangePasswordRequest changePassword)
    {
        var user = await userManager.FindByIdAsync(changePassword.UserId);
        if (user == null)
            return await ResponseWrapper.FailAsync("User does not exist.");

        var identityResult = await userManager.ChangePasswordAsync(user, changePassword.OldPassword, changePassword.NewPassword);
        if (!identityResult.Succeeded)
            return await ResponseWrapper.FailAsync(identityResult.GetIdentityResultErrorDescriptions());

        return await ResponseWrapper.SuccessAsync("User password updated.");
    }

    /// <summary>
    /// 修改用户状态
    /// </summary>
    /// <param name="changeUserStatus"></param>
    /// <returns></returns>
    public async Task<IResponseWrapper> ChangeUserStatusAsync(ChangeUserStatusRequest changeUserStatus)
    {
        var user = await userManager.FindByIdAsync(changeUserStatus.UserId);
        if (user == null)
            return await ResponseWrapper.FailAsync("User does not exist.");

        user.IsActive = changeUserStatus.ActivateOrDeactivate;
        var identityResult = await userManager.UpdateAsync(user);

        if (!identityResult.Succeeded)
            return await ResponseWrapper.FailAsync(identityResult.GetIdentityResultErrorDescriptions());

        return await ResponseWrapper.SuccessAsync(changeUserStatus.ActivateOrDeactivate
            ? "User activated successfully."
            : "User de-activated successfully.");
    }

    /// <summary>
    /// 获取用户角色
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<IResponseWrapper> GetUserRolesAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
            return await ResponseWrapper.FailAsync("User does not exist.");

        var userRolesViewModel = new List<UserRoleViewModel>();
        var roles = await roleManager.Roles.ToListAsync();
        foreach (var role in roles)
        {
            var userRoleViewModel = new UserRoleViewModel
            {
                RoleName = role.Name,
                RoleDescription = role.Description,
                IsAssignedToUser = await userManager.IsInRoleAsync(user, role.Name)
            };
            userRolesViewModel.Add(userRoleViewModel);
        }

        return await ResponseWrapper<List<UserRoleViewModel>>.SuccessAsync(userRolesViewModel);
    }

    /// <summary>
    /// 更新用户角色
    /// </summary>
    /// <param name="updateUserRoles"></param>
    /// <returns></returns>
    public async Task<IResponseWrapper> UpdateUserRolesAsync(UpdateUserRolesRequest updateUserRoles)
    {
        var user = await userManager.FindByIdAsync(updateUserRoles.UserId);
        if (user == null)
            return await ResponseWrapper.FailAsync("User does not exist.");

        if (user.Email == AppCredentials.Email)
            return await ResponseWrapper.FailAsync("User Roles update not permitted.");

        var currentAssignedRoles = await userManager.GetRolesAsync(user);

        var rolesToBeAssigned = updateUserRoles.Roles
            .Where(x => x.IsAssignedToUser)
            .ToList();

        var identityResult = await userManager.RemoveFromRolesAsync(user, currentAssignedRoles);
        if (!identityResult.Succeeded)
            return await ResponseWrapper.FailAsync(identityResult.GetIdentityResultErrorDescriptions());

        identityResult = await userManager.AddToRolesAsync(user, rolesToBeAssigned.Select(x => x.RoleName));
        if (!identityResult.Succeeded)
            return await ResponseWrapper.FailAsync(identityResult.GetIdentityResultErrorDescriptions());

        return await ResponseWrapper.SuccessAsync("Updated user roles successfully.");
    }

    /// <summary>
    /// 根据邮箱获取用户信息
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public async Task<IResponseWrapper> GetUserByEmailAsync(string email)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
            return await ResponseWrapper.FailAsync("User does not exist.");

        var mappedUser = user.Adapt<UserResponse>();
        return await ResponseWrapper<UserResponse>.SuccessAsync(mappedUser);
    }

}
