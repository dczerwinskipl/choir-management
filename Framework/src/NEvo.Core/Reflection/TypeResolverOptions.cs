using System.Reflection;

namespace NEvo.Core.Reflection;

public class TypeResolverOptions
{
    public List<Assembly> Assemblies { get; set; } = new List<Assembly>();
}
