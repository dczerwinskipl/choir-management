using Microsoft.Extensions.DependencyInjection;

namespace NEvo.CodeAnalysis.Analysing;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCodeAnalyzer(this IServiceCollection services)
    {
        services.AddSingleton<IArtifactTagProvider, ArtifactTagProvider>();
        services.AddSingleton<ICodeAnalyzer, CodeAnalyzer>();
        return services;
    }
}
