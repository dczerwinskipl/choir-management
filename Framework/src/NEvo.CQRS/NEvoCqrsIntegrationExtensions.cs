using Microsoft.Extensions.DependencyInjection;
using NEvo.CQRS.Processing.Registering;

namespace Microsoft.Extensions.Hosting;

public static class NEvoCqrsIntegrationExtensions
{
    public static THost UseNEvoCqrs<THost>(this THost application, params Action<IMessageHandlerRegistry>[] configureHandlers) where THost : IHost
    {
        var registry = application.Services.GetRequiredService<IMessageHandlerRegistry>();
        foreach (var configure in configureHandlers)
        {
            configure(registry);
        }
        return application;
    }
}
