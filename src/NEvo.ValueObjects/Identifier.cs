using NEvo.Core;
using Newtonsoft.Json;

namespace NEvo.ValueObjects;

public abstract class Identifier<TSelf, TKey> : ValueObject 
                                                        where TSelf : Identifier<TSelf, TKey> 
                                                        where TKey : notnull
{
    public TKey Id { get; }

    [JsonConstructor]
    protected Identifier(TKey id)
    {
        Id = Check.Null(id);
    }

    protected Identifier() { }

    public abstract TSelf Copy();
    public virtual ObjectId ToObjectId() => ObjectId.New(GetType().Name, Id.ToString() ?? string.Empty);
    public override string ToString() => $"({GetType().Name},{Id})";

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Id;
    }
}