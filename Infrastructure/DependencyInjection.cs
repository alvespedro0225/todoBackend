using Application.Common.Auth;
using Application.Common.Repositories;
using Infrastructure.Auth;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITodoItemRepository, TodosRepository>();
        return services;
    }
}