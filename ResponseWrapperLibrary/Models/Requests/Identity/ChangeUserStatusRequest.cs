namespace ResponseWrapperLibrary.Models.Requests.Identity;

public class ChangeUserStatusRequest
{
    public string UserId { get; set; }
    public bool ActivateOrDeactivate { get; set; }
}