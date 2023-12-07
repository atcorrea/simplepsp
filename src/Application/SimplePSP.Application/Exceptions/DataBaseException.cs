using SimplePSP.Domain.Common;

namespace SimplePSP.Application.Exceptions
{
    public class DataBaseException(Entity? value, string message, Exception? innerException) : Exception(message, innerException)
    {
        public Entity? Entity { get; } = value;

        public DataBaseException(string message, Exception? innerException) : this(null, message, innerException)
        {
        }
    }
}