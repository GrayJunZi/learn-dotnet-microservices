using MediatR;
using ResponseWrapperLibrary.Wrappers;

namespace Application.Features.Roles.Queries;

public class GetRolesQuery : IRequest<IResponseWrapper>
{

}

public class GetRolesQueryHandler (IRoleService roleService): IRequestHandler<GetRolesQuery, IResponseWrapper>
{
    public async Task<IResponseWrapper> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        return await roleService.GetRolesAsync();
    }
}
