using Application.Features.Users.Commands;
using FluentValidation;
using ResponseWrapperLibrary.Models.Requests.Identity;

namespace Application.Features.Users.Validator;

public class UserRegistrationRequestValidator : AbstractValidator<UserRegistrationRequest>
{
    public UserRegistrationRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty();

        RuleFor(x => x.Email)
            .EmailAddress();
    }
}

public class UserRegistrationCommandValidator : AbstractValidator<UserRegistrationCommand>
{
    public UserRegistrationCommandValidator()
    {
        RuleFor(x => x.UserRegistration)
            .SetValidator(new UserRegistrationRequestValidator());
    }
}