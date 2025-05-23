using System.Text;
using API.Endpoints;
using API.Utilities;
using Application;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace API;

public static class DependencyInjection
{
    public static void ConfigureApiServices(this IHostApplicationBuilder builder)
    {
        var audience = builder.Configuration["Jwt:Audience"] 
                       ?? throw new NullReferenceException("Missing audience");
        var issuer = builder.Configuration["Jwt:Issuer"] 
                     ?? throw new NullReferenceException("Missing issuer");
        var securityKey = builder.Configuration["Jwt:Secret"] 
                          ?? throw new NullReferenceException("Missing secret key");

        builder.Services.AddOpenApi();

        builder.Services
            .AddApplication()
            .AddInfrastructure();

        builder.Services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidAudience = audience,
                    ValidIssuer = issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey))
                };
            });

        builder.Services.AddAuthorization();

        builder.Services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Instance =
                    $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
            };
        });

        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    }

    public static void ConfigureAppUses(this WebApplication app)
    {
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseExceptionHandler();
    }

    public static void MapApiEndpoints(this WebApplication app)
    {
        app.MapAuthEndpoints();
        app.MapTodosEndpoints();
    }
}