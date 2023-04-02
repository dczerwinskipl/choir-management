using System.Collections.ObjectModel;

namespace NEvo.CQRS.Processing;

public class MessageProcessingFailures : ReadOnlyCollection<MessageProcessingFailure>, IMessageProcessingFailures {
    public MessageProcessingFailures(MessageProcessingFailure failure) : base(new List<MessageProcessingFailure>() { failure }) { }
    public MessageProcessingFailures(IList<MessageProcessingFailure> failures) : base(failures) { }
}