namespace API.Endpoints;

public static class Errors
{
    public static void MapErrorsEndpoints(this WebApplication app)
    {
        app.MapGet("error", Error);
    }

    public static IResult Error()
    {
        return Results.Problem();
    }
}