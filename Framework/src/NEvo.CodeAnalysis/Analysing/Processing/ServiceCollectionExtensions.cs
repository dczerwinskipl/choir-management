using Microsoft.Extensions.DependencyInjection;
using NEvo.CodeAnalysis.Analysing.ILParsing;

namespace NEvo.CodeAnalysis.Analysing.Processing;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCallArtifactProcessor(this IServiceCollection services)
    {
        services.AddIlInstructionParser();
        services.AddSingleton<CallArtifactProcessor>();
        services.AddSingleton<IArtifactProcessor, CallArtifactProcessor>(sp => sp.GetRequiredService<CallArtifactProcessor>());
        services.Configure<CodeAnalyzerOptions>(options =>
        {
            options.ArtifactProcessors.Add(typeof(CallArtifactProcessor));
        });

        return services;
    }
}
