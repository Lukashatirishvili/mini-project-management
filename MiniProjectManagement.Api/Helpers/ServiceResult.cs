namespace MiniProjectManagement.Api.Helpers;

public class ServiceResult<T>
{
    public bool Succeeded { get; set; }
    public T? Data { get; set; }
    public ServiceErrorType ErrorType { get; set; }
    public string? ErrorMessage { get; set; }

    private ServiceResult(
        bool succeeded,
        T? data,
        ServiceErrorType errorType,
        string? errorMessage)
    {
        Succeeded = succeeded;
        Data = data;
        ErrorType = errorType;
        ErrorMessage = errorMessage;
    }

    public static ServiceResult<T> Success(T data)
    {
        return new ServiceResult<T>(true, data, ServiceErrorType.None, null);
    }

    public static ServiceResult<T> NotFound(string message)
    {
        return new ServiceResult<T>(false, default, ServiceErrorType.NotFound, message);
    }

    public static ServiceResult<T> BadRequest(string message)
    {
        return new ServiceResult<T>(false, default, ServiceErrorType.BadRequest, message);
    }
    
    public static ServiceResult<T> Conflict(string message)
    {
        return new ServiceResult<T>(false, default, ServiceErrorType.Conflict, message);
    }

    public static ServiceResult<T> Validation(string message)
    {
        return new ServiceResult<T>(false, default, ServiceErrorType.Validation, message);
    }
}