namespace ForkliftControlSystem.Application.DTOs;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public ApiError? Error { get; set; }

    public static ApiResponse<T> Ok(T data) => new()
    {
        Success = true,
        Data = data,
        Error = null
    };

    public static ApiResponse<T> Fail(string message) => new()
    {
        Success = false,
        Data = default,
        Error = new ApiError { Message = message }
    };
}

public class ApiError
{
    public string Message { get; set; } = string.Empty;
}
