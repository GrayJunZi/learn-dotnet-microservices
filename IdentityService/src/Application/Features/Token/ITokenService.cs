using ResponseWrapperLibrary.Models.Requests.Identity;
using ResponseWrapperLibrary.Wrappers;

namespace Application.Features.Token;

public interface ITokenService
{
    Task<IResponseWrapper> GetTokenAsync(TokenRequest token);
    Task<IResponseWrapper> GetRefreshTokenAsync(RefreshTokenRequest refreshToken);
}
