using MediatR;
using ResponseWrapperLibrary.Models.Requests.Identity;
using ResponseWrapperLibrary.Wrappers;

namespace Application.Features.Users.Commands;

public class ChangeUserPasswordCommand : IRequest<IResponseWrapper>
{
    public ChangePasswordRequest ChangePassword { get; set; }
}

public class ChangeUserPasswordCommandHandler(IUserService userService) : IRequestHandler<ChangeUserPasswordCommand, IResponseWrapper>
{
    public async Task<IResponseWrapper> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
    {
        return await userService.ChangeUserPasswordAsync(request.ChangePassword);
    }
}
