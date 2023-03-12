using System.Runtime.Serialization;

namespace NEvo.DomainDrivenDesign
{
    public abstract class DomainException : Exception
    {
        protected DomainException()
        {
        }

        protected DomainException(string? message) : base(message)
        {
        }

        protected DomainException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        protected DomainException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
