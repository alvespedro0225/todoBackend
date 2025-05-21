namespace API.Models.Request;

public sealed record RegisterUserRequest(
    string Name,
    string Email,
    string Password);