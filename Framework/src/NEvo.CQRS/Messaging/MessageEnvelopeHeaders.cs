using System.Collections.ObjectModel;

namespace NEvo.CQRS.Messaging
{
    public class MessageEnvelopeHeaders : ReadOnlyDictionary<string, string>
    {
        public MessageEnvelopeHeaders() : base(new Dictionary<string, string>()) { }
        public MessageEnvelopeHeaders(IDictionary<string, string> dictionary) : base(dictionary)
        {
        }
    }
}