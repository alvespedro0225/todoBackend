namespace Domain.Constants;

public static class DefaultErrorMessages
{
    public const string TodoNotFoundError = "Todo not found";
    public const string TodoNotFoundMessage = "Make sure the todo exists and the ID is correct";
    public const string UserNotFoundError = "User not found";
    public const string UserNotFoundMessage = "Make sure this user is registered";
    public const string ForbiddenError = "You do not have access to this resource";
    public const string ForbiddenTodoMessage = "You must be the owner of this todo to access it";
    public const string UnauthorizedMissingClaimError = "Missing Name Identifier Claim.";
    public const string UnauthorizedMissingClaimMessage = "Make sure you are logged in and this is a registered user.";
    public const string UnauthorizedInvalidClaimError = "Invalid Name Identifier Claim";
    public const string UnauthorizedInvalidClaimMessage = "Make sure user has a valid Guid registered";
}