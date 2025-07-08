using MediatR;
using ResponseWrapperLibrary.Wrappers;

namespace Application.Features.Roles.Queries;

public class GetRoleByIdQuery : IRequest<IResponseWrapper>
{
    public string RoleId { get; set; }
}

public class GetRoleByIdQueryHandler(IRoleService roleService) : IRequestHandler<GetRoleByIdQuery, IResponseWrapper>
{
    public async Task<IResponseWrapper> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        return await roleService.GetRoleByIdAsync(request.RoleId);
    }
}
