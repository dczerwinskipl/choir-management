using NEvo.CQRS.Processing;

namespace NEvo.CQRS.Processing;

public interface IMessageProcessingFailures : IEnumerable<MessageProcessingFailure> { }
