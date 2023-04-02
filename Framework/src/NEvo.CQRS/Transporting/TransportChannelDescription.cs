namespace NEvo.CQRS.Transporting;

public abstract record TransportChannelDescription()
{
    public record InternalTransportChannelDescription(bool UseAsync) : TransportChannelDescription;
    public record EndpointTransportChannelDescription(string EndpointName) : TransportChannelDescription;
    public record MessagePublisherTransportChannelDescription(string TopicName) : TransportChannelDescription;

}
