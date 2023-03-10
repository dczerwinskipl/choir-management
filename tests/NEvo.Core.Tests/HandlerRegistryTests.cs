using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.DependencyInjection;
using NEvo.Core.Processing.Commands;
using NEvo.Messaging.Commands;
using NEvo.Processing.Commands;
using NEvo.Processing.Registering;

namespace NEvo.Core.Tests.Processing.Registering;

public class HandlerRegistryTests
{
    public HandlerRegistry CreateHandlerRegistry() 
    {
        var serviceProvider = Substitute.For<IServiceProvider>();
        return new HandlerRegistry(serviceProvider, CommandHandlerWrapperFactory.MessageHandlerOptions);
    }

    #region UnitTests
    [Fact]
    public void CreatingInstance_ShouldNotThrow()
    {
        // arrange / act
        var act = CreateHandlerRegistry;

        // assert
        act.Should().NotThrow();
    }

    [Fact]
    public void RegisteringCommandHandler_ShouldNotThrow()
    {
        var handlerRegistry = CreateHandlerRegistry();

        // act
        var act = handlerRegistry.Register<CommandAHandler>;

        // assert
        act.Should().NotThrow();
    }

    [Fact]
    public void GettingHandlersForCommandA_WithoutRegisteredCommandHandler_ShouldThrow()
    {
        // arrange
        var handlerRegistry = CreateHandlerRegistry();
        // act
        var act = () => handlerRegistry.GetHandlers(new CommandA());

        // assert
        act.Should().Throw<HandlerNotFoundException>();
    }

    [Fact]
    public void GettingHandlersForCommandA_WhenRegisteredCommandHandler_ShouldReturnSingleHandler()
    {
        // arrange
        var handlerRegistry = CreateHandlerRegistry();
        handlerRegistry.Register<CommandAHandler>();

        // act
        var handlerDescriptions = handlerRegistry.GetHandlers(new CommandA());

        // assert
        handlerDescriptions.Should()
            .ContainSingle();
    }

    [Fact]
    public void GettingHandlersForCommandA_WhenRegisteredCommandHandler_ShouldReturnHandlerDescriptionWithRegisteredHandlerInterfaceMessageAndMethod()
    {
        // arrange
        var handlerRegistry = CreateHandlerRegistry();
        handlerRegistry.Register<CommandAHandler>();

        // act
        var handlerWrapper = handlerRegistry.GetHandler(new CommandA());

        // assert
        using (new AssertionScope())
        {
            handlerWrapper.Description.HandlerType.Should().Be(typeof(CommandAHandler));
            handlerWrapper.Description.InterfaceType.Should().Be(typeof(ICommandHandler<CommandA>));
            handlerWrapper.Description.MessageClass.Should().Be(typeof(CommandA));
            handlerWrapper.Description.Method.Should().Match(method => method.Name == nameof(CommandAHandler.HandleAsync) && method.DeclaringType == typeof(CommandAHandler));
        }
    }

    [Fact]
    public void RegisteringTwoCommandHandlerForSameMessage_ShouldThrow()
    {
        // arrange
        var handlerRegistry = CreateHandlerRegistry();
        handlerRegistry.Register<CommandAHandler>();

        // act
        var act = handlerRegistry.Register<CommandAHandler>;

        // assert
        act.Should().Throw<HandlerAlreadyRegisterdException>();
    }
    #endregion

    #region Integration tests with wrapper
    [Fact]
    public async Task RunningHandlerWrapperForCommandA_ShouldExecuteCommandHandlerAMethod()
    {
        // arrange

        // to create correct implementation, we need full implementation of service porvider and scopes
        var serviceCollection = new ServiceCollection();
        var dependencyMock = Substitute.For<CommandAHandler.IDependency>();
        serviceCollection.AddSingleton(dependencyMock); //cant access directly implementation created inside wrapper, so we will check additional dependency
        var serviceProvider = serviceCollection.BuildServiceProvider();

        var handlerRegistry = new HandlerRegistry(serviceProvider, CommandHandlerWrapperFactory.MessageHandlerOptions);
        handlerRegistry.Register<CommandAHandler>();

        var message = new CommandA();
        var handler = handlerRegistry.GetHandler(message);

        // act
        var result = await handler.Handle(message);

        // assert
        using (new AssertionScope())
        {
            result.IsRight.Should().BeTrue();
            dependencyMock.Received().Act();
        }
    }
    #endregion

    #region Test implementations
    public class CommandA : Command { }
    public class CommandB : Command { }

    public class CommandAHandler : ICommandHandler<CommandA>
    {
        public IDependency Dependency { get; }

        public interface IDependency { void Act(); }

        public CommandAHandler(IDependency dependency)
        {
            Dependency = dependency;
        }

        public Task HandleAsync(CommandA command)
        {
            Dependency.Act();
            return Task.CompletedTask;
        }
    }


    public class CommandABHandler : ICommandHandler<CommandA>, ICommandHandler<CommandB>
    {
        public Task HandleAsync(CommandA command)
        {
            throw new NotImplementedException();
        }
        public Task HandleAsync(CommandB command)
        {
            throw new NotImplementedException();
        }
    }
    #endregion
}