using System.Runtime.Serialization;

namespace Perfi.Core.Accounts.Exceptions
{
    public class TransactionNotBalancedException : Exception
    {
        public TransactionNotBalancedException()
        {
        }

        public TransactionNotBalancedException(string? message) : base(message)
        {
        }

        public TransactionNotBalancedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected TransactionNotBalancedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
