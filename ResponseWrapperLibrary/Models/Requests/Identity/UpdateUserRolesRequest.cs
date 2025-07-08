using ResponseWrapperLibrary.Models.Responses.Identity;

namespace ResponseWrapperLibrary.Models.Requests.Identity;

public class UpdateUserRolesRequest
{
    public string UserId { get; set; }
    public List<UserRoleViewModel> Roles { get; set; }
}