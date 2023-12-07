namespace SimplePSP.Application.Models
{
    public class ValueResult<T>
    {
        private ValueResult(bool success, T value)
        {
            Success = success;
            Value = value;
        }

        private ValueResult(bool success, string errorMessage)
        {
            Success = success;
            ErrorMessage = errorMessage;
        }

        public bool Success { get; private set; }
        public T? Value { get; private set; }
        public string? ErrorMessage { get; private set; }

        public static ValueResult<T> CreateSuccessful(T value) => new(true, value);

        public static ValueResult<T> CreateFailed(string errorMessage) => new(false, errorMessage);
    }
}