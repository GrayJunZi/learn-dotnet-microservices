using Application;
using Application.Features.Token;
using AuthLibrary.Constants.Authentication;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ResponseWrapperLibrary.Models.Requests.Identity;
using ResponseWrapperLibrary.Models.Responses.Identity;
using ResponseWrapperLibrary.Wrappers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Services;

public class TokenService(
    UserManager<ApplicationUser> userManager,
    RoleManager<ApplicationRole> roleManager,
    IOptions<TokenSettings> tokenSettings) : ITokenService
{
    public async Task<IResponseWrapper> GetTokenAsync(TokenRequest tokenRequest)
    {
        var user = await userManager.FindByNameAsync(tokenRequest.UserName);
        if (user == null)
            return await ResponseWrapper.FailAsync("Invalid Credentials.");

        if (!user.IsActive)
            return await ResponseWrapper.FailAsync("User not active. Please contact the administrator.");

        if (!user.EmailConfirmed)
            return await ResponseWrapper.FailAsync("Email not confirmed.");

        var isPasswordValid = await userManager.CheckPasswordAsync(user, tokenRequest.Password);
        if (!isPasswordValid)
            return await ResponseWrapper.FailAsync("Invalid Credentials.");

        user.RefreshToken = generateRefreshToken();
        user.RefreshTokenExpiryDate = DateTime.Now.AddDays(tokenSettings.Value.RefreshTokenExpiryInDays);

        await userManager.UpdateAsync(user);

        var token = await generateJwtAsync(user);

        var tokenResponse = new TokenResponse
        {
            Token = token,
            RefreshToken = user.RefreshToken,
            RefreshTokenExpiryDate = user.RefreshTokenExpiryDate,
        };

        return await ResponseWrapper<TokenResponse>.SuccessAsync(tokenResponse);
    }

    public async Task<IResponseWrapper> GetRefreshTokenAsync(RefreshTokenRequest refreshTokenRequest)
    {
        var userPricipal = getClaimsPrincipalFromExpiredToken(refreshTokenRequest.Token);
        var userEmail = userPricipal.FindFirstValue(ClaimTypes.Email);

        var user = await userManager.FindByEmailAsync(userEmail);
        if (user == null)
            return await ResponseWrapper.FailAsync("User does not exists.");

        if (user.RefreshToken != refreshTokenRequest.Token || user.RefreshTokenExpiryDate <= DateTime.Now)
            return await ResponseWrapper.FailAsync("Invalid token provided.");

        var token = generateEncryptedToken(getSigningCredentials(), await getClaimsAsync(user));
        user.RefreshToken = generateRefreshToken();
        user.RefreshTokenExpiryDate = DateTime.Now.AddDays(tokenSettings.Value.RefreshTokenExpiryInDays);
        await userManager.UpdateAsync(user);

        var refreshTokenResponse = new TokenResponse
        {
            Token = token,
            RefreshToken = user.RefreshToken,
            RefreshTokenExpiryDate = user.RefreshTokenExpiryDate,
        }
        ;

        return await ResponseWrapper<TokenResponse>.SuccessAsync(refreshTokenResponse);
    }


    private string generateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rnd = RandomNumberGenerator.Create();
        rnd.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private async Task<IEnumerable<Claim>> getClaimsAsync(ApplicationUser user)
    {
        var userClaim = await userManager.GetClaimsAsync(user);
        var roles = await userManager.GetRolesAsync(user);
        var roleClaims = new List<Claim>();
        var permissionClaims = new List<Claim>();

        foreach (var role in roles)
        {
            roleClaims.Add(new Claim(ClaimTypes.Role, role));

            var currentRole = await roleManager.FindByNameAsync(role);
            var allPermissionsForCurrentRole = await roleManager.GetClaimsAsync(currentRole);

            permissionClaims.AddRange(allPermissionsForCurrentRole);
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier,user.Id),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name,user.Name),
            new(ClaimTypes.MobilePhone,user.PhoneNumber),
        }
        .Union(userClaim)
        .Union(roleClaims)
        .Union(permissionClaims);

        return claims;
    }

    private SigningCredentials getSigningCredentials()
    {
        var secret = Encoding.UTF8.GetBytes(tokenSettings.Value.Secret);
        return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
    }

    private string generateEncryptedToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
    {
        var token = new JwtSecurityToken(
            issuer: AppClaim.Issuer,
            audience: AppClaim.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(tokenSettings.Value.TokenExpiryInMinutes),
            signingCredentials: signingCredentials);

        var tokenHandler = new JwtSecurityTokenHandler();
        var encryptedToken = tokenHandler.WriteToken(token);

        return encryptedToken;
    }

    private async Task<string> generateJwtAsync(ApplicationUser user)
    {
        var token = generateEncryptedToken(getSigningCredentials(), await getClaimsAsync(user));
        return token;
    }

    private ClaimsPrincipal getClaimsPrincipalFromExpiredToken(string expiredToken)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = AppClaim.Issuer,
            ValidAudience = AppClaim.Audience,
            RoleClaimType = ClaimTypes.Role,
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSettings.Value.Secret)),
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var pricipal = tokenHandler.ValidateToken(expiredToken, tokenValidationParameters, out var securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");

        return pricipal;
    }
}
