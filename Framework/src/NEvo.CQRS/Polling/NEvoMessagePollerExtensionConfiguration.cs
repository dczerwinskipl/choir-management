using Microsoft.Extensions.DependencyInjection;
using NEvo.Polling;

namespace NEvo.Core;

public static class NEvoMessagePollerBuilderExtensions
{
    public static NEvoServicesBuilder AddMessagePoller(this NEvoServicesBuilder builder, Action<MessagePollerOptions> configurePoller)
        => builder.UseExtension(new NEvoMessagePollerExtensionConfiguration(configurePoller));
}

public class NEvoMessagePollerExtensionConfiguration : INEvoExtensionConfiguration
{
    private readonly Action<MessagePollerOptions> _configurePoller;

    public NEvoMessagePollerExtensionConfiguration(Action<MessagePollerOptions> configurePoller)
    {
        _configurePoller = configurePoller;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure(_configurePoller);
        services.AddHostedService<MessagePollerHostedService>();
    }
}
