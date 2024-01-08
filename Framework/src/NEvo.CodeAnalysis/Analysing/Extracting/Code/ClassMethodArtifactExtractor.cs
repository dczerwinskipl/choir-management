using System.Reflection;
using System.Runtime.CompilerServices;

namespace NEvo.CodeAnalysis.Analysing.Extracting.Code;

public class ClassMethodArtifactExtractor : IClassMethodArtifactExtractor
{
    private readonly IArtifactTagProvider _tagProvider;
    public ClassMethodArtifactExtractor(IArtifactTagProvider tagProvider)
    {
        _tagProvider = tagProvider;
    }

    public IEnumerable<Artifact> ExtractCodeArtifacts(Type classType) =>
        classType
            .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)
            .Cast<MethodBase>()
            .Union(classType.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance))
            .Where(method => method.DeclaringType != typeof(object))
            .Where(method => !method.GetCustomAttributes(typeof(CompilerGeneratedAttribute), true).Any())
            .Select(method => ToCodeArtifacts(classType, method))
            .ToList();

    public Artifact ToCodeArtifacts(Type classType, MethodBase method)
    {
        var tags = new List<ArtifactTag> { _tagProvider.GetTag(method is ConstructorInfo ? "Constructor" : "Method") };
        if (classType != method.DeclaringType)
            tags.Add(_tagProvider.GetTag("Derived"));
        var (methodName, isConstructor) = ExtractNames(method, classType);

        return new CodeArtifact<MethodBase>
        {
            Artifact = method,
            Type = isConstructor ? IClassMethodArtifactExtractor.ConstructorArtifactType : IClassMethodArtifactExtractor.MethodArtifactType,
            Key = $"{IClassMethodArtifactExtractor.MethodArtifactType}/{classType.AssemblyQualifiedName}/{methodName}",
            Name = methodName,
            Tags = tags
        };
    }

    //todo: move somewhere
    public static (string MethodName, bool IsConstructor) ExtractNames(MethodBase method, Type? classType = null)
    {
        var generics = string.Empty;
        if (method.IsGenericMethod)
        {
            generics = $"<{string.Join(",", method.GetGenericArguments().Select(a => a.FullName))}>";
        }
        var args = $"({string.Join(",", method.GetParameters().Select(a => $"{a.ParameterType.FullName} {a.Name}"))})";
        return ($"{(method is ConstructorInfo ? classType?.Name ?? method.ReflectedType?.Name ?? method.DeclaringType?.Name : method.Name)}{generics}{args}", method.IsConstructor);
    }
}
