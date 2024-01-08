using System.Text.Json.Serialization;

namespace NEvo.CodeAnalysis.Analysing;

public record Artifact : IEquatable<Artifact>
{
    public required string Key { get; init; }
    public required string Name { get; init; }
    public required string Type { get; init; }
    public ICollection<ArtifactTag> Tags { get; init; } = new List<ArtifactTag>();
    public ICollection<ArtifactInfo> Info { get; init; } = new List<ArtifactInfo>();
    public ICollection<ArtifactRelation> Relations { get; init; } = new List<ArtifactRelation>();

    public override int GetHashCode()
    {
        return Key.GetHashCode();
    }

    public virtual bool Equals(Artifact? other)
    {
        return other is not null && other.Key.Equals(Key);
    }
}

public interface ICodeArtifact
{
    [JsonIgnore]
    public object Artifact { get; }
}

public record CodeArtifact<T> : Artifact, ICodeArtifact where T : notnull
{
    [JsonIgnore]
    public required T Artifact { get; init; }

    object ICodeArtifact.Artifact => Artifact;
}

public class ArtifactTag
{
    public string? Key { get; init; }
    public string? Name { get; init; }
}

public abstract class ArtifactInfo
{
    public string? Type { get; init; }
}

public class ArtifactRelation
{
    public string? Type { get; init; }
    public string? ArtifactKey { get; init; }
}