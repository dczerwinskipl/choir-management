using NEvo.Core;
using NEvo.Core.Processing;
using NEvo.Messaging;

namespace NEvo.Processing.Registering;

/// <summary>
/// Message handler wrapper interface to unify message handling without result (TOOD: or should i use Void datatype?) and allow to add custom type of handlers as extension
/// </summary>
public interface IMessageHandlerWrapper
{
    MessageHandlerDescription Description { get; }
    Task<Either<MessageProcessingFailure, MessageProcessingSuccess>> Handle(IMessage message);
}

/// <summary>
/// Message handler wrapper interface to unify message handling with result and allow to add custom type of handlers as extension
/// </summary>
/// <typeparam name="TResult"></typeparam>
public interface IMessageHandlerWrapper<TResult>
{
    MessageHandlerDescription Description { get; }
    Task<Either<MessageProcessingFailure, MessageProcessingSuccess<TResult>>> Handle(IMessage message);
}