using Application.Services;
using Application.Services.Auth.Commands;
using Application.Services.Common;
using Application.Services.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthCommandService, AuthCommandService>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddScoped<ITodosService, TodosService>();
        return services;
    }
}