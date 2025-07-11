using AuthLibrary.Constants.Authentication;
using AuthLibrary.Permissions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using ResponseWrapperLibrary.Wrappers;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Gateway.Extensions
{
    public static class ServiceCollectionExtensions
    {
        internal static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var secret = Encoding.UTF8.GetBytes(configuration["Secret"]);

            services
                .AddAuthentication(configureOptions =>
                {
                    configureOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    configureOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(configureOptions =>
                {
                    configureOptions.RequireHttpsMetadata = false;
                    configureOptions.SaveToken = true;
                    configureOptions.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = AppClaim.Issuer,
                        ValidAudience = AppClaim.Audience,
                        RoleClaimType = ClaimTypes.Role,
                        ClockSkew = TimeSpan.Zero,
                        IssuerSigningKey = new SymmetricSecurityKey(secret),
                    };

                    configureOptions.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            IResponseWrapper result;
                            if (context.Exception is SecurityTokenExpiredException)
                            {
                                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                                context.Response.ContentType = "application/json";
                                result = ResponseWrapper.Fail("The token is expired.");
                            }
                            else
                            {
                                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                context.Response.ContentType = "application/json";
                                result = ResponseWrapper.Fail("An unhandled error has occured.");
                            }
                            return context.Response.WriteAsJsonAsync(result);
                        },
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            if (!context.Response.HasStarted)
                            {
                                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                                context.Response.ContentType = "application/json";
                                return context.Response.WriteAsJsonAsync(ResponseWrapper.Fail("You are not Authorized."));
                            }

                            return Task.CompletedTask;
                        },
                        OnForbidden = context =>
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                            context.Response.ContentType = "application/json";
                            return context.Response.WriteAsJsonAsync(ResponseWrapper.Fail("You are not authorized to access this resources."));
                        }
                    };
                });

            return services;
        }

        internal static IServiceCollection AddIdentitySettings(this IServiceCollection services)
        {
            services
                .AddSingleton<IAuthorizationPolicyProvider,PermissionPolicyProvider>()
                .AddScoped<IAuthorizationHandler,PermissionAuthorizationHandler>();
            return services;
        }
    }
}
