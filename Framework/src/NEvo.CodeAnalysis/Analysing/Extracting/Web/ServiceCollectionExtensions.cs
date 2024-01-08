using Microsoft.Extensions.DependencyInjection;

namespace NEvo.CodeAnalysis.Analysing.Extracting.Web;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEndpointArtifactExtractor(this IServiceCollection services)
    {
        services.AddSingleton<EndpointArtifactExtractor>();
        services.AddSingleton<IArtifactExtractor, EndpointArtifactExtractor>(sp => sp.GetRequiredService<EndpointArtifactExtractor>());
        services.Configure<CodeAnalyzerOptions>(options =>
        {
            options.ArtifactExtractors.Add(typeof(EndpointArtifactExtractor));
        });
        return services;
    }
}
