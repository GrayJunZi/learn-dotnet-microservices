using System.Collections.ObjectModel;

namespace AuthLibrary.Constants.Authentication;

public record AppPermission(string Service, string Feature, string Action,
    string Group, string Description, bool IsBasic = false)
{
    public string Name => NameFor(Service, Feature, Action);

    public static string NameFor(string service, string feature, string action)
    {
        return $"Permission.{service}.{feature}.{action}";
    }
}

public class AppPermissions
{
    private static readonly AppPermission[] _all =
    [
        new (AppService.Identity,AppFeature.Users,AppAction.Create,AppRoleGroup.SystemAccess,"Create Users"),
        new (AppService.Identity,AppFeature.Users,AppAction.Read,AppRoleGroup.SystemAccess,"Read Users"),
        new (AppService.Identity,AppFeature.Users,AppAction.Update,AppRoleGroup.SystemAccess,"Update Users"),
        new (AppService.Identity,AppFeature.Users,AppAction.Delete,AppRoleGroup.SystemAccess,"Delete Users"),

        new (AppService.Identity,AppFeature.Roles,AppAction.Create,AppRoleGroup.SystemAccess,"Create Roles"),
        new (AppService.Identity,AppFeature.Roles,AppAction.Read,AppRoleGroup.SystemAccess,"Read Roles"),
        new (AppService.Identity,AppFeature.Roles,AppAction.Update,AppRoleGroup.SystemAccess,"Update Roles"),
        new (AppService.Identity,AppFeature.Roles,AppAction.Delete,AppRoleGroup.SystemAccess,"Delete Roles"),

        new (AppService.Identity,AppFeature.UserRoles,AppAction.Read,AppRoleGroup.SystemAccess,"Read User Roles"),
        new (AppService.Identity,AppFeature.UserRoles,AppAction.Update,AppRoleGroup.SystemAccess,"Update User Roles"),

        new (AppService.Identity,AppFeature.RoleClaims,AppAction.Read,AppRoleGroup.SystemAccess,"Read Role Claims/Permissions"),
        new (AppService.Identity,AppFeature.RoleClaims,AppAction.Update,AppRoleGroup.SystemAccess,"Update Role Claims/Permissions"),
    ];

    public static IReadOnlyList<AppPermission> AllPermissions { get; } = 
        new ReadOnlyCollection<AppPermission>(_all);

    public static IReadOnlyList<AppPermission> AdminPermissions { get; } =
        new ReadOnlyCollection<AppPermission>([.. _all.Where(x => !x.IsBasic)]);

    public static IReadOnlyList<AppPermission> BasicPermissions { get; } =
        new ReadOnlyCollection<AppPermission>([.. _all.Where(x => x.IsBasic)]);
}
