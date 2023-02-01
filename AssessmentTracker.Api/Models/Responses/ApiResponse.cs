namespace AssessmentTracker.Api.Models.Responses;

public class ApiResponse 
{
    public bool IsSuccessful { get; }
    public string Message { get; }
    public int? StatusCode { get; }
    public object? Properties { get; private set; }

    private ApiResponse(bool success, string message)
    {
        IsSuccessful = success;
        Message = message;
        StatusCode = success 
            ? StatusCodes.Status200OK 
            : StatusCodes.Status400BadRequest;
    }

    public static ApiResponse Success()
    {
        return new ApiResponse(true, "Successfully executed the request.");
    }

    public static ApiResponse Failure(string failureReason)
    {
        return new ApiResponse(false, failureReason);
    }

    public ApiResponse WithCustomProperties(object properties)
    {
        Properties = properties;

        return this;
    }
}