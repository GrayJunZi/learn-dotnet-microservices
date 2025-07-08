using MediatR;
using ResponseWrapperLibrary.Wrappers;

namespace Application.Features.Users.Queries;

public class GetUserByIdQuery : IRequest<IResponseWrapper>
{
    public string UserId { get; set; }
}

public class GetUserByIdQueryHandler(IUserService userService) : IRequestHandler<GetUserByIdQuery, IResponseWrapper>
{
    public async Task<IResponseWrapper> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        return await userService.GetUserByIdAsync(request.UserId);
    }
}
