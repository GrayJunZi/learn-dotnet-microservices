using System.Net;

namespace ResponseWrapperLibrary.Exceptions;

public class CustomValidationException : Exception
{
    public List<string> ErrorMessages { get; set; }

    public string FriendlyErrorMessage { get; set; }

    public HttpStatusCode StatusCode { get; set; }

    public CustomValidationException(string friendlyErrorMessage,
        List<string> errorMessages = default,
        HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        FriendlyErrorMessage = friendlyErrorMessage;
        ErrorMessages = errorMessages;
        StatusCode = statusCode;
    }
}
