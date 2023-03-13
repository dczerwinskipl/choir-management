using static System.Net.Mime.MediaTypeNames;
using System.Collections.Generic;

namespace NEvo.Monads;

public readonly partial struct Maybe<T>
{
    private readonly T? _value;

    private readonly bool _hasValue;

    private Maybe(T value)
    {
        _hasValue = true;
        _value = value;
    }

    public static implicit operator Maybe<T>(T? value) => value is not null ? new(value) : new();
    public static implicit operator Maybe<T>(Maybe.MaybeNone _) => new();

    public T Value
    {
        get
        {
            if (!HasNoValue)
                throw new InvalidOperationException("Maybe has no value");
#pragma warning disable CS8603 // Possible null reference return.
            return _value;
#pragma warning restore CS8603 // Possible null reference return.
        }
    }
    public bool HasValue => _hasValue;
    public bool HasNoValue => !_hasValue;
}