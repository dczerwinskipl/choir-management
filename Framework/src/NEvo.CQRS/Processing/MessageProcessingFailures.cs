using System.Collections.ObjectModel;

namespace NEvo.Processing;

public class MessageProcessingFailures : ReadOnlyCollection<MessageProcessingFailure>, IMessageProcessingFailures {
    public MessageProcessingFailures(MessageProcessingFailure failure) : base(new List<MessageProcessingFailure>() { failure }) { }
    public MessageProcessingFailures(IList<MessageProcessingFailure> failures) : base(failures) { }
}