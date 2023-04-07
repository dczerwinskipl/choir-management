using Microsoft.Extensions.Options;
using NEvo.Core;
using NEvo.Core.Reflection;
using NEvo.CQRS.Messaging;
using NEvo.CQRS.Processing;
using NEvo.CQRS.Routing;
using NEvo.CQRS.Transporting;
using NEvo.Monads;
using static NEvo.CQRS.Transporting.TransportChannelDescription;

namespace NEvo.CQRS.Tests.Transporting;

public class TransportChannelFactoryTests
{
    [Fact]
    public void CreateInternalChannel_ShouldReturnInternalChannel()
    {
        // arrange
        var options = new TransportChannelFactoryOptions();
        options.InternalTransportChannelFactory = new MockChannelFactory();
        var channelFactory = new TransportChannelFactory(
            Substitute.For<ITypeResolver>(),
            Substitute.For<IRoutingTopologyProvider>(),
            Options.Create(options));
        var description = new InternalTransportChannelDescription(false);

        // act
        var channel = channelFactory.CreateChannel(Substitute.For<IServiceProvider>(), description);

        // assert
        channel
            .Should()
            .BeOfType<InternalTransportChannel>();
    }

    [Fact]
    public void CreateEndpointChannel_WhenThereIsFactory_ShouldReturnCorrectChannel()
    {
        // arrange
        var options = new TransportChannelFactoryOptions();
        options.EndpointTransportChannelFactories.Add(new MockChannelFactory());

        var typeResolver = Substitute.For<ITypeResolver>();
        typeResolver.GetType(Arg.Any<string>()).Returns(typeof(EndpointChannelA));

        var routingTopologyProvider = Substitute.For<IRoutingTopologyProvider>();
        routingTopologyProvider.GetEndpointDescription(Arg.Any<string>()).Returns(new EndpointTopologyDescription());

        var channelFactory = new TransportChannelFactory(
            typeResolver,
            routingTopologyProvider,
            Options.Create(options));
        var description = new EndpointTransportChannelDescription("EndpointA");

        // act
        var channel = channelFactory.CreateChannel(Substitute.For<IServiceProvider>(), description);

        // assert
        channel
            .Should()
            .BeOfType<EndpointChannelA>();
    }

    [Fact]
    public void CreateEndpointChannel_WhenThereIsNoFactory_ShouldThrowException()
    {
        // arrange
        var options = new TransportChannelFactoryOptions();
        options.EndpointTransportChannelFactories.Add(new MockChannelFactory());

        var typeResolver = Substitute.For<ITypeResolver>();
        typeResolver.GetType(Arg.Any<string>()).Returns(typeof(InternalTransportChannel));

        var routingTopologyProvider = Substitute.For<IRoutingTopologyProvider>();
        routingTopologyProvider.GetEndpointDescription(Arg.Any<string>()).Returns(new EndpointTopologyDescription());

        var channelFactory = new TransportChannelFactory(
            typeResolver,
            routingTopologyProvider,
            Options.Create(options));

        var description = new EndpointTransportChannelDescription("InternalTransportChannel");

        // act
        var act = () => channelFactory.CreateChannel(Substitute.For<IServiceProvider>(), description);

        // assert
        act
            .Should()
            .Throw<ArgumentException>();
    }

    [Fact]
    public void CreateTopicChannel_WhenThereIsFactory_ShouldReturnCorrectChannel()
    {
        // arrange
        var options = new TransportChannelFactoryOptions();
        options.MessagePublisherTransportChannelFactories.Add(new MockChannelFactory());

        var typeResolver = Substitute.For<ITypeResolver>();
        typeResolver.GetType(Arg.Any<string>()).Returns(typeof(MessagePublisherChannelA));

        var routingTopologyProvider = Substitute.For<IRoutingTopologyProvider>();
        routingTopologyProvider.GetTopicDescription(Arg.Any<string>()).Returns(new TopicTopologyDescription());

        var channelFactory = new TransportChannelFactory(
            typeResolver,
            routingTopologyProvider,
            Options.Create(options));
        var description = new MessagePublisherTransportChannelDescription("EndpointA");

        // act
        var channel = channelFactory.CreateChannel(Substitute.For<IServiceProvider>(), description);

        // assert
        channel
            .Should()
            .BeOfType<MessagePublisherChannelA>();
    }

    [Fact]
    public void CreateTopicChannel_WhenThereIsNoFactory_ShouldThrowException()
    {
        // arrange
        var options = new TransportChannelFactoryOptions();
        options.MessagePublisherTransportChannelFactories.Add(new MockChannelFactory());

        var typeResolver = Substitute.For<ITypeResolver>();
        typeResolver.GetType(Arg.Any<string>()).Returns(typeof(InternalTransportChannel));

        var routingTopologyProvider = Substitute.For<IRoutingTopologyProvider>();
        routingTopologyProvider.GetTopicDescription(Arg.Any<string>()).Returns(new TopicTopologyDescription());

        var channelFactory = new TransportChannelFactory(
            typeResolver,
            routingTopologyProvider,
            Options.Create(options));

        var description = new MessagePublisherTransportChannelDescription("InternalTransportChannel");

        // act
        var act = () => channelFactory.CreateChannel(Substitute.For<IServiceProvider>(), description);

        // assert
        act
            .Should()
            .Throw<ArgumentException>();
    }
}


#region Mock types
public class MockChannelFactory :
    ITransportChannelFactory<InternalTransportChannel, InternalTransportChannelDescription, InternalChannelTopologyDescription>,
    ITransportChannelFactory<EndpointChannelA, EndpointTransportChannelDescription, EndpointTopologyDescription>,
    ITransportChannelFactory<EndpointChannelB, EndpointTransportChannelDescription, EndpointTopologyDescription>,
    ITransportChannelFactory<MessagePublisherChannelA, MessagePublisherTransportChannelDescription, TopicTopologyDescription>,
    ITransportChannelFactory<MessagePublisherChannelB, MessagePublisherTransportChannelDescription, TopicTopologyDescription>
{
    public InternalTransportChannel CreateChannel(IServiceProvider serviceProvider, InternalTransportChannelDescription channelDescription, InternalChannelTopologyDescription topicTopologyDescription)
        => new InternalTransportChannel(Substitute.For<IMessageProcessor>(), channelDescription);


    public EndpointChannelA CreateChannel(IServiceProvider serviceProvider, EndpointTransportChannelDescription channelDescription, EndpointTopologyDescription topicTopologyDescription)
        => new EndpointChannelA();

    public MessagePublisherChannelA CreateChannel(IServiceProvider serviceProvider, MessagePublisherTransportChannelDescription channelDescription, TopicTopologyDescription topicTopologyDescription)
        => new MessagePublisherChannelA();

    EndpointChannelB ITransportChannelFactory<EndpointChannelB, EndpointTransportChannelDescription, EndpointTopologyDescription>.CreateChannel(IServiceProvider serviceProvider, EndpointTransportChannelDescription channelDescription, EndpointTopologyDescription topicTopologyDescription)
        => new EndpointChannelB();

    MessagePublisherChannelB ITransportChannelFactory<MessagePublisherChannelB, MessagePublisherTransportChannelDescription, TopicTopologyDescription>.CreateChannel(IServiceProvider serviceProvider, MessagePublisherTransportChannelDescription channelDescription, TopicTopologyDescription topicTopologyDescription)
        => new MessagePublisherChannelB();
}

public class EndpointChannelA : ITransportChannel
{
    public Task<Either<Exception, TResult>> DispatchMessageAsync<TMessage, TResult>(MessageEnvelope<TMessage> messsageEnvelope) where TMessage : IMessage<TResult>
    {
        throw new NotImplementedException();
    }

    public Task<Either<Exception, Unit>> PublishMessageAsync<TMessage>(MessageEnvelope<TMessage> messsageEnvelope) where TMessage : IMessage<Unit>
    {
        throw new NotImplementedException();
    }
}

public class EndpointChannelB : ITransportChannel
{
    public Task<Either<Exception, TResult>> DispatchMessageAsync<TMessage, TResult>(MessageEnvelope<TMessage> messsageEnvelope) where TMessage : IMessage<TResult>
    {
        throw new NotImplementedException();
    }

    public Task<Either<Exception, Unit>> PublishMessageAsync<TMessage>(MessageEnvelope<TMessage> messsageEnvelope) where TMessage : IMessage<Unit>
    {
        throw new NotImplementedException();
    }
}

public class MessagePublisherChannelA : ITransportChannel
{
    public Task<Either<Exception, TResult>> DispatchMessageAsync<TMessage, TResult>(MessageEnvelope<TMessage> messsageEnvelope) where TMessage : IMessage<TResult>
    {
        throw new NotImplementedException();
    }

    public Task<Either<Exception, Unit>> PublishMessageAsync<TMessage>(MessageEnvelope<TMessage> messsageEnvelope) where TMessage : IMessage<Unit>
    {
        throw new NotImplementedException();
    }
}

public class MessagePublisherChannelB : ITransportChannel
{
    public Task<Either<Exception, TResult>> DispatchMessageAsync<TMessage, TResult>(MessageEnvelope<TMessage> messsageEnvelope) where TMessage : IMessage<TResult>
    {
        throw new NotImplementedException();
    }

    public Task<Either<Exception, Unit>> PublishMessageAsync<TMessage>(MessageEnvelope<TMessage> messsageEnvelope) where TMessage : IMessage<Unit>
    {
        throw new NotImplementedException();
    }
}
#endregion