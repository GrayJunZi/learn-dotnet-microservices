using MediatR;
using ResponseWrapperLibrary.Wrappers;

namespace Application.Features.Roles.Commands;

public class DeleteRoleCommand : IRequest<IResponseWrapper>
{
    public string RoleId { get; set; }
}

public class DeleteRoleCommandHandler(IRoleService roleService) : IRequestHandler<DeleteRoleCommand, IResponseWrapper>
{
    public async Task<IResponseWrapper> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        return await roleService.DeleteRoleAsync(request.RoleId);
    }
}
