namespace ResponseWrapperLibrary.Models.Requests.Identity;

public class UserRegistrationRequest
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public string PhoneNumber { get; set; }
    public bool AutoConfirmEmail { get; set; }
    public bool ActivateUser { get; set; }
}
