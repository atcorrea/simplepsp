namespace SimplePSP.Application.Models
{
    public class Result
    {
        private Result(bool success, string? errorMessage)
        {
            Success = success;
            ErrorMessage = errorMessage;
        }

        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }

        public static Result CreateSuccessful() => new(true, null);

        public static Result CreateFailed(string errorMessage) => new(false, errorMessage);
    }
}