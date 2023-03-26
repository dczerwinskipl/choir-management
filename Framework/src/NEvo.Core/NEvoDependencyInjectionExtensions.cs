using Microsoft.Extensions.DependencyInjection;

namespace NEvo.Core;

public static class NEvoDependencyInjectionExtensions
{
    public static IServiceCollection AddNEvo(this IServiceCollection serviceCollection) => AddNEvo(serviceCollection, _ => { });
    public static IServiceCollection AddNEvo(this IServiceCollection serviceCollection, Action<NEvoServicesBuilder> extensions)
    {
        Check.Null(serviceCollection);
        Check.Null(extensions);

        var servicesBuilder = new NEvoServicesBuilder();
        serviceCollection.AddSingleton<INEvoAppDetailsProvider, NEvoAppDetailsProvider>(_ => new NEvoAppDetailsProvider());
        extensions(servicesBuilder);
        servicesBuilder.ApplyExtensions(serviceCollection);
        return serviceCollection;
    }
}