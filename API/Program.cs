using API;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureApiServices();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.ConfigureAppUses();
app.MapApiEndpoints();
app.Run();
