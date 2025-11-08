namespace DualTechTechnicalTest.Domain.Models;

public class Result<T>
{
    public bool Success { get; set; }

    public string Message { get; set; }

    public IEnumerable<string> Errors { get; set; }

    public T? Data { get; set; }

    private Result(string error, string message = "")
    {
        this.Success = false;
        this.Message = message;
        this.Errors = [error];
        this.Data = default(T);
    }

    private Result(T data, string message = "")
    {
        this.Success = true;
        this.Message = message;
        this.Data = data;
        this.Errors = [];
    }

    public static Result<T> SuccessResponse(T data, string message = "") => new(data, message);

    public static Result<T> FailureResponse(string error, string message = "") =>
        new(error, message);
}
