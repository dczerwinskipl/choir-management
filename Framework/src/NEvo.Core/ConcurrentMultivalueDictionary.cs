using System.Collections.Concurrent;

namespace NEvo.Core;
public class ConcurrentMultivalueDictionary<TKey, TValue> : ConcurrentDictionary<TKey, ICollection<TValue>> where TKey : notnull
{
    public ConcurrentMultivalueDictionary()
    {
    }

    public ConcurrentMultivalueDictionary(IEnumerable<KeyValuePair<TKey, ICollection<TValue>>> collection) : base(collection)
    {
    }

    public ConcurrentMultivalueDictionary(IEqualityComparer<TKey>? comparer) : base(comparer)
    {
    }

    public ConcurrentMultivalueDictionary(IEnumerable<KeyValuePair<TKey, ICollection<TValue>>> collection, IEqualityComparer<TKey>? comparer) : base(collection, comparer)
    {
    }

    public ConcurrentMultivalueDictionary(int concurrencyLevel, int capacity) : base(concurrencyLevel, capacity)
    {
    }

    public ConcurrentMultivalueDictionary(int concurrencyLevel, IEnumerable<KeyValuePair<TKey, ICollection<TValue>>> collection, IEqualityComparer<TKey>? comparer) : base(concurrencyLevel, collection, comparer)
    {
    }

    public ConcurrentMultivalueDictionary(int concurrencyLevel, int capacity, IEqualityComparer<TKey>? comparer) : base(concurrencyLevel, capacity, comparer)
    {
    }

    public void Add(TKey key, TValue value)
    {
        var collection = GetOrAdd(key, new HashSet<TValue>());
        collection.Add(value);
    }
}