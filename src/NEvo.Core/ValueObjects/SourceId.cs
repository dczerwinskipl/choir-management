using NEvo.Core;

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

    private SourceId(string id, string type)
    {
        Id = Check.NullOrEmpty(id);
        Type = Check.NullOrEmpty(type);
    }

    /// <summary>
    /// Create new isntance of source
    /// </summary>
    /// <param name="id">source identifier</param>
    /// <param name="type">source type</param>
    /// <returns>New SourceId instance</returns>
    public static SourceId New(string id, string type) => new SourceId(id, type);

    /// <summary>
    /// Create a copy of current SourceId
    /// </summary>
    /// <returns>New SourceId instance</returns>
    public SourceId Copy() => new SourceId(Id, Type);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Id;
        yield return Type;
    }
}
