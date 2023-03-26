namespace NEvo.ValueObjects;

/// <summary>
/// Base class for ValueObjects.
/// 
/// For better results, use records.
/// </summary>
public abstract class ValueObject
{
    protected static bool EqualOperator(ValueObject left, ValueObject right) => !(left is null || right is null) && (ReferenceEquals(left, right) || left.Equals(right));

    protected static bool NotEqualOperator(ValueObject left, ValueObject right) => !(EqualOperator(left, right));

    protected abstract IEnumerable<object> GetEqualityComponents();

    public static bool operator ==(ValueObject one, ValueObject two) => EqualOperator(one, two);
    public static bool operator !=(ValueObject one, ValueObject two) => NotEqualOperator(one, two);

    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType())
        {
            return false;
        }

        var other = (ValueObject)obj;

        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Select(x => x != null ? x.GetHashCode() : 0)
            .Aggregate((x, y) => x ^ y);
    }
}