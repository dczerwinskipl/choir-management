using NEvo.Core;
using Newtonsoft.Json;

namespace NEvo.ValueObjects;

/// <summary>
/// Type that represents abstract source as value object
/// </summary>
public class SourceId : ValueObject
{
    /// <summary>
    /// Type of object
    /// </summary>
    public string Type { get; }
    /// <summary>
    /// Identifier of object
    /// </summary>
    public string Id { get; }

    [JsonConstructor]
    private SourceId(string type, string id)
    {
        Type = Check.NullOrEmpty(type);
        Id = Check.NullOrEmpty(id);
    }

    /// <summary>
    /// Create new isntance of source
    /// </summary>
    /// <param name="id">source identifier</param>
    /// <param name="type">source type</param>
    /// <returns>New SourceId instance</returns>
    public static SourceId New(string type, string id) => new SourceId(type, id);

    /// <summary>
    /// Create a copy of current SourceId
    /// </summary>
    /// <returns>New SourceId instance</returns>
    public SourceId Copy() => new SourceId(Id, Type);

    public override string ToString() => $"({Type},{Id})";

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Id;
        yield return Type;
    }
}
