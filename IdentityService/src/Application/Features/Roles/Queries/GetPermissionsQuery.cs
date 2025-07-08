using MediatR;
using ResponseWrapperLibrary.Wrappers;

namespace Application.Features.Roles.Queries;

public class GetPermissionsQuery : IRequest<IResponseWrapper>
{
    public string RoleId { get; set; }
}

public class GetPermissionsQueryHandler(IRoleService roleService) : IRequestHandler<GetPermissionsQuery, IResponseWrapper>
{
    public async Task<IResponseWrapper> Handle(GetPermissionsQuery request, CancellationToken cancellationToken)
    {
        return await roleService.GetRolePermissionsAsync(request.RoleId);
    }
}