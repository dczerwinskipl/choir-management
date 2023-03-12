using System.Collections.ObjectModel;

namespace NEvo.Messaging
{
    public class MessageEnvelopeHeaders : ReadOnlyDictionary<string, string>
    {
        public MessageEnvelopeHeaders() : base(new Dictionary<string, string>()) { }
        public MessageEnvelopeHeaders(IDictionary<string, string> dictionary) : base(dictionary)
        {
        }
    }
}