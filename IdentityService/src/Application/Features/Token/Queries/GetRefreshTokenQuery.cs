using MediatR;
using ResponseWrapperLibrary.Models.Requests.Identity;
using ResponseWrapperLibrary.Wrappers;

namespace Application.Features.Token.Queries;

public class GetRefreshTokenQuery : IRequest<IResponseWrapper>
{
    public RefreshTokenRequest RefreshTokenRequest { get; set; }
}

public class GetRefreshTokenQueryHandler(ITokenService tokenService) : IRequestHandler<GetRefreshTokenQuery, IResponseWrapper>
{
    public async Task<IResponseWrapper> Handle(GetRefreshTokenQuery request, CancellationToken cancellationToken)
    {
        return await tokenService.GetRefreshTokenAsync(request.RefreshTokenRequest);
    }
}