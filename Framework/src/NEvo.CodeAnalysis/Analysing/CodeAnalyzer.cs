using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NEvo.CodeAnalysis.Analysing.Extracting;
using NEvo.CodeAnalysis.Analysing.Processing;
using NEvo.Core;

namespace NEvo.CodeAnalysis.Analysing;

public class CodeAnalyzerOptions
{
    public required ICollection<Type> ArtifactExtractors { get; init; } = new List<Type>();
    public required ICollection<Type> ArtifactProcessors { get; init; } = new List<Type>();
}

public class CodeAnalyzerContext
{
    private readonly Dictionary<string, Artifact> _artifactsByKey;
    private readonly Dictionary<object, ICodeArtifact> _artifactsByObject;

    public CodeAnalyzerContext(IEnumerable<Artifact> artifacts)
    {
        _artifactsByKey = artifacts.ToDictionary(k => k.Key, k => k);
        _artifactsByObject = artifacts.OfType<ICodeArtifact>().ToDictionary(k => k.Artifact, k => k);
    }

    public Artifact? Get(object artifact) => _artifactsByObject.TryGetValue(artifact, out var value) ? value as Artifact : null;
    public Artifact? Get(string key) => _artifactsByKey.TryGetValue(key, out var artifact) ? artifact : null;
}

public class CodeAnalyzer : ICodeAnalyzer
{
    private readonly IEnumerable<IArtifactExtractor> _codeArtifactExtractors;
    private readonly IEnumerable<IArtifactProcessor> _codeArtifactProcessors;

    public CodeAnalyzer(IOptions<CodeAnalyzerOptions> options, IServiceProvider serviceProvider)
    {
        _codeArtifactExtractors = options.Value
            .ArtifactExtractors
            .Select(t => Check.Null(serviceProvider.GetRequiredService(t) as IArtifactExtractor))
            .ToList();

        _codeArtifactProcessors = options.Value
            .ArtifactProcessors
            .Select(t => Check.Null(serviceProvider.GetRequiredService(t) as IArtifactProcessor))
            .ToList();
    }

    public async Task<IEnumerable<Artifact>> AnalyzeAsync(params Assembly[] assemblies)
    {
        var artifacts = (await assemblies.SelectManyAsync(assembly => _codeArtifactExtractors.SelectManyAsync(extractor => extractor.ExtractCodeArtifactsAsync(assembly)))).ToHashSet();
        var context = new CodeAnalyzerContext(artifacts);
        await artifacts.ForEachAsync(artifact => _codeArtifactProcessors.ForEachAsync(processor => processor.ProcessAsync(artifact, context)));
        return artifacts;
    }
}
