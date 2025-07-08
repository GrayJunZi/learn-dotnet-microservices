using MediatR;
using ResponseWrapperLibrary.Models.Requests.Identity;
using ResponseWrapperLibrary.Wrappers;

namespace Application.Features.Users.Commands;

public class UpdateUserRolesCommand : IRequest<IResponseWrapper>
{
    public UpdateUserRolesRequest UpdateUserRoles { get; set; }
}

public class UpdateUserRolesCommandHandler(IUserService userService) : IRequestHandler<UpdateUserRolesCommand, IResponseWrapper>
{
    public async Task<IResponseWrapper> Handle(UpdateUserRolesCommand request, CancellationToken cancellationToken)
    {
        return await userService.UpdateUserRolesAsync(request.UpdateUserRoles);
    }
}
