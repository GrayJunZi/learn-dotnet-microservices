using MediatR;
using ResponseWrapperLibrary.Wrappers;

namespace Application.Features.Users.Queries;

public class GetAllUsersQuery : IRequest<IResponseWrapper>
{

}

public class GetAllUsersQueryHandler(IUserService userService) : IRequestHandler<GetAllUsersQuery, IResponseWrapper>
{
    public async Task<IResponseWrapper> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        return await userService.GetAllUsersAsync();
    }
}