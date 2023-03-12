using System.Runtime.Serialization;

namespace NEvo.DomainDrivenDesign
{
    /// <summary>
    /// TODO: aggregate name/id, etc. better names
    /// </summary>
    public class DomainRuleValidationException : DomainException
    {
        public DomainRuleValidationException()
        {
        }

        public DomainRuleValidationException(string? ruleName) : base($"Validation of rule failed: {ruleName}")
        {
        }

        public DomainRuleValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public DomainRuleValidationException(string? ruleName, Exception? innerException) : base($"Validation of rule failed: {ruleName}", innerException)
        {
        }
    }
}
