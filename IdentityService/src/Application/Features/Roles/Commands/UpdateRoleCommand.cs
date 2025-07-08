using MediatR;
using ResponseWrapperLibrary.Models.Requests.Identity;
using ResponseWrapperLibrary.Wrappers;

namespace Application.Features.Roles.Commands;

public class UpdateRoleCommand : IRequest<IResponseWrapper>
{
    public UpdateRoleRequest UpdateRole { get; set; }
}

public class UpdateRoleCommandHandler(IRoleService roleService) : IRequestHandler<UpdateRoleCommand, IResponseWrapper>
{
    public async Task<IResponseWrapper> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        return await roleService.UpdateRoleAsync(request.UpdateRole);
    }
}