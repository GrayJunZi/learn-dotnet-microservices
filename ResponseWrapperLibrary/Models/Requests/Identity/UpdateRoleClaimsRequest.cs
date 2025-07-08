using ResponseWrapperLibrary.Models.Responses.Identity;

namespace ResponseWrapperLibrary.Models.Requests.Identity;

public class UpdateRoleClaimsRequest
{
    public string RoleId { get; set; }
    public List<RoleClaimViewModel> RoleClaims { get; set; }
}