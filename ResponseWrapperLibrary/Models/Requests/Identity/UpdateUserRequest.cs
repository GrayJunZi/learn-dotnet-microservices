namespace ResponseWrapperLibrary.Models.Requests.Identity;

public class UpdateUserRequest
{
    public string UserId { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
}
