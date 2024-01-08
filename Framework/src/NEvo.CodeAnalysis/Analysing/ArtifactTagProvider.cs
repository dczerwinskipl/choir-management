using System.Collections.Concurrent;

namespace NEvo.CodeAnalysis.Analysing;

public class ArtifactTagProvider : IArtifactTagProvider
{
    private readonly ConcurrentDictionary<string, ArtifactTag> _tags = new();

    public ArtifactTag GetTag(string name) => _tags.GetOrAdd(name, name => new ArtifactTag { Key = name, Name = name });
}
