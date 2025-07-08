using MediatR;
using ResponseWrapperLibrary.Models.Requests.Identity;
using ResponseWrapperLibrary.Wrappers;

namespace Application.Features.Token.Queries;

public class GetTokenQuery : IRequest<IResponseWrapper>
{
    public TokenRequest TokenRequest { get; set; }
}

public class GetTokenQueryHandler(ITokenService tokenService) : IRequestHandler<GetTokenQuery, IResponseWrapper>
{
    public async Task<IResponseWrapper> Handle(GetTokenQuery request, CancellationToken cancellationToken)
    {
        return await tokenService.GetTokenAsync(request.TokenRequest);
    }
}
