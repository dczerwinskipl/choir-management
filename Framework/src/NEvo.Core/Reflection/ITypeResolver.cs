namespace NEvo.Core.Reflection;

public interface ITypeResolver
{
    Type GetType(string name);
    string GetName(Type type, TypeResolvingOptions options = TypeResolvingOptions.Assembly);
}
