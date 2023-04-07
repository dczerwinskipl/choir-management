using System.Runtime.Serialization;

namespace NEvo.Core;

public class MultivalueDictionary<TKey, TValue> : Dictionary<TKey, ICollection<TValue>> where TKey : notnull
{
    public MultivalueDictionary()
    {
    }

    public MultivalueDictionary(IDictionary<TKey, ICollection<TValue>> dictionary) : base(dictionary)
    {
    }

    public MultivalueDictionary(IEnumerable<KeyValuePair<TKey, ICollection<TValue>>> collection) : base(collection)
    {
    }

    public MultivalueDictionary(IEqualityComparer<TKey>? comparer) : base(comparer)
    {
    }

    public MultivalueDictionary(int capacity) : base(capacity)
    {
    }

    public MultivalueDictionary(IDictionary<TKey, ICollection<TValue>> dictionary, IEqualityComparer<TKey>? comparer) : base(dictionary, comparer)
    {
    }

    public MultivalueDictionary(IEnumerable<KeyValuePair<TKey, ICollection<TValue>>> collection, IEqualityComparer<TKey>? comparer) : base(collection, comparer)
    {
    }

    public MultivalueDictionary(int capacity, IEqualityComparer<TKey>? comparer) : base(capacity, comparer)
    {
    }

    protected MultivalueDictionary(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public void Add(TKey key, TValue value)
    {
        var collection = GetOrAdd(key, new HashSet<TValue>());
        collection.Add(value);
    }

    public void Add(TKey key, IEnumerable<TValue> value)
    {
        var collection = GetOrAdd(key, new HashSet<TValue>());
        foreach (var item in value)
            collection.Add(item);
    }

    public ICollection<TValue> GetOrAdd(TKey key, HashSet<TValue> values)
    {
        if (TryGetValue(key, out var existingValues))
            return existingValues;

        this[key] = values;
        return values;
    }
}
