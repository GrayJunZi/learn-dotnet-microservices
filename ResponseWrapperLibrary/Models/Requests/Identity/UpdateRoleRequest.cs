namespace ResponseWrapperLibrary.Models.Requests.Identity;

public class UpdateRoleRequest
{
    public string RoleId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}