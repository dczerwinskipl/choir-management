namespace NEvo.Core.Processing;

public interface IMessageProcessingResult
{

}

public interface IMessageProcessingResult<TResult>
{
    TResult Result { get; }
}
