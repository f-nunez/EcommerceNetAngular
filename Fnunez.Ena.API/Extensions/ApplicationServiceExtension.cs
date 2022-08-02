using Fnunez.Ena.API.Errors;
using Fnunez.Ena.Core.Interfaces;
using Fnunez.Ena.Infrasctructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace Fnunez.Ena.API.Extensions;

public static class ApplicationServiceExtension
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IGenericRepository<>), (typeof(GenericRepository<>)));
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = actionContext =>
            {
                IEnumerable<string> errors = actionContext.ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .SelectMany(x => x.Value.Errors)
                    .Select(x => x.ErrorMessage);

                ApiValidationErrorResponse errorResponse = new()
                {
                    Errors = errors
                };

                return new BadRequestObjectResult(errorResponse);
            };
        });

        return services;
    }
}