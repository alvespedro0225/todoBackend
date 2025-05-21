namespace API.Models.Request;

public sealed record LoginUserRequest(
    string Email,
    string Password);