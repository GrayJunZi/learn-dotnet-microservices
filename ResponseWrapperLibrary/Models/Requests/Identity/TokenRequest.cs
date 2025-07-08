namespace ResponseWrapperLibrary.Models.Requests.Identity;

public class TokenRequest
{
    public string UserName { get; set; }
    public string Password { get; set; }
}