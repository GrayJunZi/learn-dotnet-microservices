using System.Text.Json;
using System.Text.Json.Serialization;

namespace ResponseWrapperLibrary.Wrappers;

public static class ResponseWrapperExtension
{
    public static Lazy<JsonSerializerOptions> _jsonSerializerOptions
        => new(() => new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            ReferenceHandler = ReferenceHandler.Preserve,
        });

    public static async Task<ResponseWrapper<T>> ToResponse<T>(this HttpResponseMessage httpResponseMessage)
    {
        var response = await httpResponseMessage.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ResponseWrapper<T>>(response, _jsonSerializerOptions.Value);

        return result;
    }

    public static async Task<ResponseWrapper> ToResponse(this HttpResponseMessage httpResponseMessage)
    {
        var response = await httpResponseMessage.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ResponseWrapper>(response, _jsonSerializerOptions.Value);

        return result;
    }
}