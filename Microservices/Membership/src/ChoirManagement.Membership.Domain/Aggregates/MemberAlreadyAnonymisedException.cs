using System.Runtime.Serialization;
using NEvo.DomainDrivenDesign;

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