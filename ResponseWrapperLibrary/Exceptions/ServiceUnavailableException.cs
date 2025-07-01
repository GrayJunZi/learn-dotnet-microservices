using System.Net;

namespace ResponseWrapperLibrary.Exceptions;

public class ServiceUnavailableException : Exception
{
    public List<string> ErrorMessages { get; set; }

    public string FriendlyErrorMessage { get; set; }

    public HttpStatusCode StatusCode { get; set; }

    public ServiceUnavailableException(string friendlyErrorMessage,
        List<string> errorMessages = default,
        HttpStatusCode statusCode = HttpStatusCode.ServiceUnavailable)
    {
        FriendlyErrorMessage = friendlyErrorMessage;
        ErrorMessages = errorMessages;
        StatusCode = statusCode;
    }
}
