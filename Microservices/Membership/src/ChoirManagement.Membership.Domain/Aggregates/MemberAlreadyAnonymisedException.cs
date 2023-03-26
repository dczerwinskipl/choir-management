using NEvo.DomainDrivenDesign;
using System.Runtime.Serialization;

namespace ChoirManagement.Membership.Domain.Aggregates
{
    [Serializable]
    internal class MemberAlreadyAnonymisedException : DomainException
    {
        public MemberAlreadyAnonymisedException()
        {
        }

        public MemberAlreadyAnonymisedException(string? message) : base(message)
        {
        }

        public MemberAlreadyAnonymisedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected MemberAlreadyAnonymisedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}