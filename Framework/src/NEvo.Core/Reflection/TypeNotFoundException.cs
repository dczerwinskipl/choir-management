namespace NEvo.Core.Reflection;

public class TypeNotFoundException : Exception
{
    public string TypeName { get; }
    public TypeNotFoundException(string typeName) : base($"Type {typeName} could not be found. Try providing more details like Namespace, Assembly or omit from Version")
    {
        TypeName = typeName;
    }
}
