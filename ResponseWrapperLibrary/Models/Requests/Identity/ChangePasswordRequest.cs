namespace ResponseWrapperLibrary.Models.Requests.Identity;

public class ChangePasswordRequest
{
    public string UserId { get; set; }
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
    public string ConfirmNewPassword { get; set; }
}