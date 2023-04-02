namespace NEvo.Core.Reflection;

public class MultipleTypesFoundException : Exception
{
    public string TypeName { get; }
    public MultipleTypesFoundException(string typeName, IEnumerable<Type> types) : base($"Multiple types [{string.Join(",", types.Select(t => t.AssemblyQualifiedName))}] for name {typeName} has been found. Try providing more details like Namespace, Assembly or omit from Version")
    {
        TypeName = typeName;
    }
}
