using System.Text;
using Application.Common.Auth;
using Application.Common.Repositories;
using Infrastructure.Auth;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITodoItemRepository, TodosRepository>();
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlite($"Data Source={Environment.GetEnvironmentVariable("Sqlite")}");
        });
        services.AddAuth(configuration);
        return services;
    }

    private static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var audience = configuration["Jwt:Audience"] 
                       ?? throw new NullReferenceException("Missing audience");
        
        var issuer = configuration["Jwt:Issuer"] 
                     ?? throw new NullReferenceException("Missing issuer");
        
        var securityKey = Environment.GetEnvironmentVariable("AccessSecret") 
                          ?? throw new NullReferenceException("Missing secret key");
        
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidAudience = audience,
                    ValidIssuer = issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true
                };
            });

        services.AddAuthorization();
        return services;
    }
}