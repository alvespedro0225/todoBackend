using Application.Services;
using Application.Services.Auth.Commands;
using Application.Services.Auth.Queries;
using Application.Services.Common;
using Application.Services.Todos;
using Application.Services.Todos.Commands;
using Application.Services.Todos.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddScoped<IAuthCommandService, AuthCommandService>();
        services.AddScoped<IAuthQueryService, AuthQueryService>();
        services.AddScoped<ITodosCommandService, TodosCommandService>();
        services.AddScoped<ITodosQueryService, TodosQueryService>();
        return services;
    }
}