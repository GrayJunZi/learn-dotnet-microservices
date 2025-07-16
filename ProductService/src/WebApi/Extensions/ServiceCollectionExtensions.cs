using Application;
using AuthLibrary.Constants.Authentication;
using AuthLibrary.Permissions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ResponseWrapperLibrary.Wrappers;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace WebApi.Extensions
{
    public static  class ServiceCollectionExtensions
    {
        internal static IServiceCollection AddIdentitySettings(this IServiceCollection services)
        {
            services
                .AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>()
                .AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

            return services;
        }

        internal static TokenSettings GetTokenSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var tokenSettingsSection = configuration.GetSection(nameof(TokenSettings));
            
            services.Configure<TokenSettings>(tokenSettingsSection);

            return tokenSettingsSection.Get<TokenSettings>();
        }

        internal static CacheSettings GetCacheSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var tokenSettingsSection = configuration.GetSection(nameof(CacheSettings));
            
            services.Configure<CacheSettings>(tokenSettingsSection);

            return tokenSettingsSection.Get<CacheSettings>();
        }
        internal static IServiceCollection AddJwtAuthentication(this IServiceCollection services, TokenSettings tokenSettings)
        {
            var secret = Encoding.UTF8.GetBytes(tokenSettings.Secret);

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

            services.AddAuthorization(options =>
            {
                var properties = typeof(AppPermissions).GetNestedTypes()
                    .SelectMany(x => x.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.FlattenHierarchy));

                foreach (var property in properties)
                {
                    var value = property.GetValue(null);
                    if (value == null)
                        continue;

                    options.AddPolicy(value.ToString(), policy => policy
                        .RequireClaim(AppClaim.Permission, value.ToString()));
                }
            });

            return services;

        }

        internal static IServiceCollection AddScalarDocumentation(this IServiceCollection services)
        {
            services.AddOpenApi(options =>
            {
                options.AddDocumentTransformer((document, context, cancellationToken) =>
                {
                    document.Components = new OpenApiComponents
                    {
                        SecuritySchemes = new Dictionary<string, OpenApiSecurityScheme>
                        {
                            ["Bearer"] = new OpenApiSecurityScheme
                            {
                                Name = "Authorization",
                                In = ParameterLocation.Header,
                                Type = SecuritySchemeType.ApiKey,
                                Scheme = "Bearer",
                                BearerFormat = "JWT",
                                Description = "Input your Bearer token in this format - Bearer {your token here} to access this API",
                            }
                        },
                    };

                    document.SecurityRequirements.Add(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Name = "Bearer",
                                Scheme = "Bearer",
                                In = ParameterLocation.Header,
                                Reference = new OpenApiReference
                                {
                                    Id = "Bearer",
                                    Type = ReferenceType.SecurityScheme,
                                },
                            },
                            new List<string>()
                        }
                    });
                    return Task.CompletedTask;
                });
            });
            return services;
        }
    }
}
