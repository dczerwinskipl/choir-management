namespace NEvo.CQRS.Processing;

public interface IMessageProcessingResult
{

}

public interface IMessageProcessingResult<TResult>
{
    TResult Result { get; }
}
