using Microsoft.Extensions.DependencyInjection;
using NEvo.Core.Reflection;

namespace NEvo.Core;

public static class NEvoDependencyInjectionExtensions
{
    public static IServiceCollection AddNEvo(this IServiceCollection serviceCollection) => AddNEvo(serviceCollection, _ => { });
    public static IServiceCollection AddNEvo(this IServiceCollection serviceCollection, Action<NEvoServicesBuilder> extensions)
    {
        Check.Null(serviceCollection);
        Check.Null(extensions);

        AddCoreServices(serviceCollection);
        ApplyExtensions(serviceCollection, extensions);

        return serviceCollection;
    }

    private static void ApplyExtensions(IServiceCollection serviceCollection, Action<NEvoServicesBuilder> extensions)
    {
        var servicesBuilder = new NEvoServicesBuilder();
        extensions(servicesBuilder);
        servicesBuilder.ApplyExtensions(serviceCollection);
    }

    private static void AddCoreServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<INEvoAppDetailsProvider, NEvoAppDetailsProvider>(_ => new NEvoAppDetailsProvider());

        serviceCollection.Configure<TypeResolverOptions>(options => options.Assemblies.AddRange(AppDomain.CurrentDomain.GetAssemblies().Where(a => a.GetName().Name?.StartsWith(nameof(NEvo)) ?? false)));
        serviceCollection.AddSingleton<ITypeResolver, TypeResolver>();
    }
}