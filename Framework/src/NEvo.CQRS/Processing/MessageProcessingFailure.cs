namespace NEvo.Processing;

public record MessageProcessingFailure(Type HandlerType, Exception Exception);
public record MessageProcessingSuccess(Type HandlerType);
public record MessageProcessingSuccess<TResult>(Type HandlerType, TResult Result);