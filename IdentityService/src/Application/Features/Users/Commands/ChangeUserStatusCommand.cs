using MediatR;
using ResponseWrapperLibrary.Models.Requests.Identity;
using ResponseWrapperLibrary.Wrappers;

namespace Application.Features.Users.Commands;

public class ChangeUserStatusCommand : IRequest<IResponseWrapper>
{
    public ChangeUserStatusRequest ChangeUserStatus { get; set; }
}

public class ChangeUserStatusCommandHandler(IUserService userService) : IRequestHandler<ChangeUserStatusCommand, IResponseWrapper>
{
    public async Task<IResponseWrapper> Handle(ChangeUserStatusCommand request, CancellationToken cancellationToken)
    {
        return await userService.ChangeUserStatusAsync(request.ChangeUserStatus);
    }
}
