using MediatR;
using ResponseWrapperLibrary.Models.Requests.Identity;
using ResponseWrapperLibrary.Wrappers;

namespace Application.Features.Users.Commands;

public class UpdateUserCommand : IRequest<IResponseWrapper>
{
    public UpdateUserRequest UpdateUser { get; set; }
}

public class UpdateUserCommandHandler(IUserService userService) : IRequestHandler<UpdateUserCommand, IResponseWrapper>
{
    public async Task<IResponseWrapper> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        return await userService.UpdateUserAsync(request.UpdateUser);
    }
}
