using System.Net;

namespace Fnunez.Ena.API.Errors;

public class ApiResponse
{
    public string Message { get; set; }
    public int StatusCode { get; set; }

    public ApiResponse(int statusCode, string message = null)
    {
        StatusCode = statusCode;
        Message = message ?? GetDefaultMessageForStatusCode(statusCode);
    }

    private string GetDefaultMessageForStatusCode(int statusCode)
    {
        return (HttpStatusCode)statusCode switch
        {
            HttpStatusCode.BadRequest => nameof(HttpStatusCode.BadRequest),
            HttpStatusCode.Unauthorized => nameof(HttpStatusCode.Unauthorized),
            HttpStatusCode.NotFound => nameof(HttpStatusCode.NotFound),
            HttpStatusCode.InternalServerError => nameof(HttpStatusCode.InternalServerError),
            _ => null
        };
    }
}