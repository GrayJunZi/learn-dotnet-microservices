using Application.Features.Token.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResponseWrapperLibrary.Models.Requests.Identity;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
public class TokenController : BaseController<TokenController>
{
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> LoginAsync([FromBody] TokenRequest tokenRequest)
    {
        var response = await Sender.Send(new GetTokenQuery { TokenRequest = tokenRequest });
        if (!response.IsSuccessful)
            return BadRequest(response);

        return Ok(response);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> GetRefreshTokenAsync([FromBody] RefreshTokenRequest refreshTokenRequest)
    {
        var response = await Sender.Send(new GetRefreshTokenQuery { RefreshTokenRequest = refreshTokenRequest });
        if (!response.IsSuccessful)
            return BadRequest(response);

        return Ok(response);
    }
}