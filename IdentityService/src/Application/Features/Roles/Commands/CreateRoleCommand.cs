using MediatR;
using ResponseWrapperLibrary.Models.Requests.Identity;
using ResponseWrapperLibrary.Wrappers;

namespace Application.Features.Roles.Commands;

public class CreateRoleCommand : IRequest<IResponseWrapper>
{
    public CreateRoleRequest CreateRole { get; set; }
}

public class CreateRoleCommandHandler(IRoleService roleService) : IRequestHandler<CreateRoleCommand, IResponseWrapper>
{
    public async Task<IResponseWrapper> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        return await roleService.CreateRoleAsync(request.CreateRole);
    }
}
