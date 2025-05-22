using System.Text;
using API.Endpoints;
using Application;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var audience = builder.Configuration["Jwt:Audience"]  ?? throw new NullReferenceException("Missing audience");
var issuer = builder.Configuration["Jwt:Issuer"]  ?? throw new NullReferenceException("Missing issuer");
var securityKey = builder.Configuration["Jwt:Secret"] ?? throw new NullReferenceException("Missing secret key");

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
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapAuthEndpoints();
app.MapTodosEndpoints();
app.Run();
