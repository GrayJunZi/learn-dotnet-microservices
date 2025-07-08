using Application.Pipelines;
using MediatR;
using ResponseWrapperLibrary.Models.Requests.Identity;
using ResponseWrapperLibrary.Wrappers;

namespace Application.Features.Users.Commands;

public class UserRegistrationCommand : IRequest<IResponseWrapper>, IValidateSelf
{
    public UserRegistrationRequest UserRegistration { get; set; }
}

public class UserRegistrationCommandHandler(IUserService userService) : IRequestHandler<UserRegistrationCommand, IResponseWrapper>
{
    public async Task<IResponseWrapper> Handle(UserRegistrationCommand request, CancellationToken cancellationToken)
    {
        return await userService.RegisterUserAsync(request.UserRegistration);
    }
}
