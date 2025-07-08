using MediatR;
using ResponseWrapperLibrary.Models.Requests.Identity;
using ResponseWrapperLibrary.Wrappers;

namespace Application.Features.Roles.Commands;

public class UpdateRolePermissionCommand : IRequest<IResponseWrapper>
{
    public UpdateRoleClaimsRequest UpdateRoleClaims { get; set; }
}

public class UpdateRolePermissionCommandHandler(IRoleService roleService) : IRequestHandler<UpdateRolePermissionCommand, IResponseWrapper>
{
    public async Task<IResponseWrapper> Handle(UpdateRolePermissionCommand request, CancellationToken cancellationToken)
    {
        return await roleService.UpdateRolePermissionsAsync(request.UpdateRoleClaims);
    }
}
