namespace NEvo.ValueObjects;

[AttributeUsage(AttributeTargets.Class)]
public class ValueObjectAttribute : Attribute
{
    public bool IsSingleValue { get; }
    public string? DefaultValue { get; }
    public string? Format { get; }

    public ValueObjectAttribute(bool isSingleValue = false, string? defaultValue = null, string? format = null)
    {
        IsSingleValue = isSingleValue;
        DefaultValue = defaultValue;
        Format = format;
    }
}
