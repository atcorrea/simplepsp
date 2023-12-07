using SimplePSP.Domain.TransactionAggregate;

namespace SimplePSP.API.Extensions
{
    public static class ILoggerExtensions
    {
        public static IDisposable? GetLogScopeForTransaction<T>(this ILogger<T> logger, Transaction transaction)
        {
            var context = new Dictionary<string, string>()
            {
                { "transactionId", transaction.Id },
                { "storeId", transaction.StoreId },
            };

            return logger.BeginScope(context);
        }

        public static IDisposable? GetLogScopeWithProperty<T>(this ILogger<T> logger, string propKey, string propValue)
        {
            var context = new Dictionary<string, string>()
            {
                { propKey, propValue },
            };

            return logger.BeginScope(context);
        }
    }
}