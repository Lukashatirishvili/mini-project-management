namespace MiniProjectManagement.Api.Helpers;

public enum ServiceErrorType
{
    None = 0,
    NotFound = 1,
    BadRequest = 2,
    Conflict = 3,
    Validation = 4
}