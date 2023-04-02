namespace NEvo.CQRS.Channeling;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class ChannelAttribute : Attribute
{
    public string Name { get; }
    public ChannelAttribute(string name) => Name = name;
}
