using AuthLibrary.Constants.Authentication;
using Infrastructure.Constants;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contexts;

public class ApplicationDbSeeder(
    UserManager<ApplicationUser> userManager,
    RoleManager<ApplicationRole> roleManager,
    ApplicationDbContext applicationDbContext)
{
    private async Task checkAndApplyPendingMigrationAsync()
    {
        if ((await applicationDbContext.Database.GetPendingMigrationsAsync())?.Any() ?? false)
        {
            await applicationDbContext.Database.MigrateAsync();
        }
    }

    public async Task SeedIdentityDatabaseAsync()
    {
        await checkAndApplyPendingMigrationAsync();
        await seedRolesAsync();
        await seedAdminUserAsync();
        await seedBasicUserAsync();
    }

    private async Task seedAdminUserAsync()
    {
        var user = new ApplicationUser
        {
            Name = "Admin",
            Email = AppCredentials.Email,
            UserName = AppCredentials.Email,
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            PhoneNumber = "0000000000",
            NormalizedEmail = AppCredentials.Email.ToUpperInvariant(),
            NormalizedUserName = AppCredentials.Email.ToUpperInvariant(),
            IsActive = true,
        };

        if (!await userManager.Users.AnyAsync(x => x.Email == AppCredentials.Email))
        {
            var password = new PasswordHasher<ApplicationUser>();
            user.PasswordHash = password.HashPassword(user, AppCredentials.Password);
            await userManager.CreateAsync(user);
        }

        user = await userManager.FindByEmailAsync(user.Email);

        if (!await userManager.IsInRoleAsync(user, AppRoles.Basic) && !await userManager.IsInRoleAsync(user, AppRoles.Admin))
        {
            await userManager.AddToRolesAsync(user, AppRoles.DefaultRoles);
        }
    }

    private async Task seedBasicUserAsync()
    {
        var email = "test@test.com";
        var user = new ApplicationUser
        {
            Name = "Test",
            Email = email,
            UserName = email,
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            PhoneNumber = "0000000000",
            NormalizedEmail = email.ToUpperInvariant(),
            NormalizedUserName = email.ToUpperInvariant(),
            IsActive = true,
        };

        if (!await userManager.Users.AnyAsync(x => x.Email == email))
        {
            var password = new PasswordHasher<ApplicationUser>();
            user.PasswordHash = password.HashPassword(user, AppCredentials.Password);
            await userManager.CreateAsync(user);
        }

        user = await userManager.FindByEmailAsync(email);

        if (!await userManager.IsInRoleAsync(user, AppRoles.Basic))
        {
            await userManager.AddToRoleAsync(user, AppRoles.Basic);
        }
    }

    private async Task seedRolesAsync()
    {
        foreach (var defaultRole in AppRoles.DefaultRoles)
        {
            var role = await roleManager.Roles.FirstOrDefaultAsync(x => x.Name == defaultRole);
            if (role == null)
            {
                role = new ApplicationRole
                {
                    Name = defaultRole,
                    Description = $"{defaultRole} Role.",
                    NormalizedName = defaultRole.ToUpperInvariant(),
                };
                await roleManager.CreateAsync(role);
            }

            if (defaultRole == AppRoles.Basic)
            {
                await assignedPermissionsToRoleAsync(role, AppPermissions.BasicPermissions);
            }
            else if (defaultRole == AppRoles.Admin)
            {
                await assignedPermissionsToRoleAsync(role, AppPermissions.AdminPermissions);
            }
        }
    }

    private async Task assignedPermissionsToRoleAsync(ApplicationRole role, IReadOnlyList<AppPermission> permissions)
    {
        var currentlyAssignedClaims = await roleManager.GetClaimsAsync(role);

        foreach (var permission in permissions)
        {
            if (!currentlyAssignedClaims.Any(x => x.Type == AppClaim.Permission && x.Value == permission.Name))
            {
                await applicationDbContext.RoleClaims.AddAsync(new ApplicationRoleClaim
                {
                    RoleId = role.Id,
                    ClaimType = AppClaim.Permission,
                    ClaimValue = permission.Name,
                    Description = permission.Description,
                    Group = permission.Group,
                });

                await applicationDbContext.SaveChangesAsync();
            }
        }
    }
}
