using MediatR;
using ResponseWrapperLibrary.Wrappers;

namespace Application.Features.Users.Queries;

public class GetUserRolesQuery : IRequest<IResponseWrapper>
{
    public string UserId { get; set; }
}

public class GetUserRolesQueryHandler(IUserService userService) : IRequestHandler<GetUserRolesQuery, IResponseWrapper>
{
    public async Task<IResponseWrapper> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
    {
        return await userService.GetUserRolesAsync(request.UserId);
    }
}