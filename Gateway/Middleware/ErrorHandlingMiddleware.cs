using Ocelot.Configuration;
using ResponseWrapperLibrary.Exceptions;
using ResponseWrapperLibrary.Wrappers;
using System.Text.Json;

namespace Gateway.Middleware
{
    public class ErrorHandlingMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);

                if (context.Response.StatusCode == StatusCodes.Status502BadGateway)
                {
                    var downstreamDomain = getDownstreamDomain(context);
                    throw new ServiceUnavailableException($"{downstreamDomain} - Service Unavailable");
                }
            }
            catch (Exception ex)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                var result = await ResponseWrapper.FailAsync();

                switch (ex)
                {
                    case ServiceUnavailableException serviceUnavailableException:
                        response.StatusCode = (int)serviceUnavailableException.StatusCode;
                        result.Messages = [serviceUnavailableException.FriendlyErrorMessage];
                        break;
                    default:
                        response.StatusCode = StatusCodes.Status500InternalServerError;
                        result.Messages = [ex.Message];
                        break;
                }

                await response.WriteAsJsonAsync(result);
            }
        }

        private static string getDownstreamDomain(HttpContext context)
        {
            if (context.Items.TryGetValue("DownstreamRoute", out var value)
                && value is DownstreamRoute downstreamRoute)
            {
                var scheme = downstreamRoute.DownstreamScheme;
                var hostAndPort = downstreamRoute.DownstreamAddresses[0];

                return $"{scheme}://{hostAndPort.Host}:{hostAndPort.Port}";
            }

            return "UnknownService";
        }
    }
}