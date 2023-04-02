using Microsoft.Extensions.DependencyInjection;
using NEvo.Azure;
using NEvo.Azure.Administrating;
using NEvo.Azure.Polling;
using NEvo.Azure.Publishing;
using NEvo.Azure.Transporting;
using NEvo.CQRS.Transporting;
using NEvo.Polling;
using NEvo.Publishing;

namespace NEvo.Core;

public static class NEvoAzureMessageBusServicesBuilderExtensions
{
    public static NEvoServicesBuilder AddAzureServiceBus(this NEvoServicesBuilder builder, Action<AzureServiceBusClientData> configureClientData)
        => builder.UseExtension(new NEvoAzureMessageBusExtensionConfiguration(configureClientData));
}

public class NEvoAzureMessageBusExtensionConfiguration : INEvoExtensionConfiguration
{
    private readonly Action<AzureServiceBusClientData> _configureClientData;

    public NEvoAzureMessageBusExtensionConfiguration(Action<AzureServiceBusClientData> configureClientData)
    {
        _configureClientData = configureClientData;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure(_configureClientData);
        services.AddSingleton<IAzureServiceBusAdministrator, AzureServiceBusAdministrator>();
        
        //new idea of registering extensions, TODO: do same to poller?
        services.Configure<TransportChannelFactoryOptions>(options =>
        {
            options.MessagePublisherTransportChannelFactories.Add(new AzureServiceBusTransportChannelFactory());
        });
        services.AddScoped<IAzureServiceBusMessagePublisher, AzureServiceBusMessagePublisher>();


        services.AddSingleton<IMessagePollerFactory<AzureServiceBusMessagePoller>, AzureServiceBusMessagePollerFactory>();
    }
}