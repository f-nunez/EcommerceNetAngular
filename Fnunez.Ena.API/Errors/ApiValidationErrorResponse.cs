using System.Net;

namespace Fnunez.Ena.API.Errors;

public class ApiValidationErrorResponse : ApiResponse
{
    public IEnumerable<string> Errors { get; set; }

    public ApiValidationErrorResponse() : base((int)HttpStatusCode.BadRequest)
    {
    }
}