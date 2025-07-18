﻿namespace ResponseWrapperLibrary.Models.Responses.Identity;

public class UserResponse
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public bool IsActive { get; set; }
    public bool EmailConfirmed { get; set; }
    public string PhoneNumber { get; set; }
}
