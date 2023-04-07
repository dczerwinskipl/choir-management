using System.Collections.Concurrent;
using System.Reflection;
using Microsoft.Extensions.Options;

namespace NEvo.Core.Reflection;

public class TypeResolver : ITypeResolver
{
    private readonly ConcurrentDictionary<string, Type?> _resolvedTypes = new ConcurrentDictionary<string, Type?>();
    private readonly IEnumerable<Assembly> _assemblies;

    public TypeResolver(IOptions<TypeResolverOptions> options)
    {
        _assemblies = Check.Null(options.Value.Assemblies);
    }

    internal TypeResolver(IEnumerable<Assembly> assemblies)
    {
        _assemblies = Check.Null(assemblies);
    }

    public string GetName(Type type, TypeResolvingOptions options = TypeResolvingOptions.Assembly) =>
        options switch
        {
            TypeResolvingOptions.Name => type.Name,
            TypeResolvingOptions.Namespace => Check.NullOrEmpty(type.FullName),
            TypeResolvingOptions.Assembly => $"{type.FullName}, {type.Assembly.GetName().Name}",
            TypeResolvingOptions.Version => $"{type.FullName}, {type.Assembly.GetName().Name}, Version={type.Assembly.GetName().Version}",
            TypeResolvingOptions.AssemblyQualifiedName => Check.NullOrEmpty(type.AssemblyQualifiedName),
            _ => throw new NotImplementedException()
        };


    public Type GetType(string name) => _resolvedTypes.GetOrAdd(name, GetTypeInternal) ?? throw new TypeNotFoundException(name);

    private Type? GetTypeInternal(string name)
    {
        var types = _assemblies.SelectMany(assembly => GetTypeFromAssemblyInternal(assembly, name)).ToList();
        return types.Count() <= 1 ? types.SingleOrDefault() : throw new MultipleTypesFoundException(name, types);
    }

    private IEnumerable<Type> GetTypeFromAssemblyInternal(Assembly assembly, string name)
    {
        var resolvingOptions = GetTypeResolvingOptions(name);
        return assembly.GetTypes().Where(t => GetName(t, resolvingOptions).Equals(name));
    }

    private TypeResolvingOptions GetTypeResolvingOptions(string name)
    {
        var nameParts = name.Count(c => c == ',') + 1;
        return nameParts switch
        {
            1 => name.Any(c => c == '.') ? TypeResolvingOptions.Namespace : TypeResolvingOptions.Name,
            2 => TypeResolvingOptions.Assembly,
            3 => TypeResolvingOptions.Version,
            4 => TypeResolvingOptions.AssemblyQualifiedName,
            _ => throw new NotImplementedException()
        };
    }
}
