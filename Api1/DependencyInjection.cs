using Api1.Endpoints;

using Api1.Utilities;

namespace Api1;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddExceptionHandling();
        services.ConfigureCors();
        return services;
    }

    public static void MapApiEndpoints(this WebApplication app)
    {
        app.MapAuthEndpoints();
        app.MapTodosEndpoints();
    }

    private static IServiceCollection AddExceptionHandling(this IServiceCollection services)
    {
        services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Instance =
                    $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
            };
        });
        
        services.AddExceptionHandler<GlobalExceptionHandler>();
        return services;
    }

    private static IServiceCollection ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy
                    .WithOrigins("http://localhost:4200")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
        return services;
    }
}