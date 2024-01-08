using System.Reflection;

namespace NEvo.CodeAnalysis.Analysing.Extracting;

public interface IArtifactExtractor
{
    Task<IEnumerable<Artifact>> ExtractCodeArtifactsAsync(Assembly assembly);
}
