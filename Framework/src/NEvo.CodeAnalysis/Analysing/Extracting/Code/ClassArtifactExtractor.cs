using System.Reflection;
using Microsoft.Extensions.Options;

namespace NEvo.CodeAnalysis.Analysing.Extracting.Code;

public class ClassArtifactExtractorOptions
{
    public required ICollection<IClassArtifactFilter> Filters { get; init; } = new List<IClassArtifactFilter>();
}

public class ClassArtifactExtractor : IArtifactExtractor
{
    public static string ArtifactType = "code/Class";

    public static string ClassMethodRelationType = "code/Class/Method";
    public static string MethodClassRelationType = "code/Method/Class";

    public static string ClassPropertyRelationType = "code/Class/Property";
    public static string PropertyClassRelationType = "code/Property/Class";

    private readonly IEnumerable<IClassArtifactFilter> _filters;
    private readonly IClassMethodArtifactExtractor _classMethodArtifactExtractor;
    private readonly IArtifactTagProvider _tagProvider;

    public ClassArtifactExtractor(IOptions<ClassArtifactExtractorOptions> options, IClassMethodArtifactExtractor classMethodArtifactExtractor, IArtifactTagProvider tagProvider)
    {
        _filters = options.Value.Filters;
        _classMethodArtifactExtractor = classMethodArtifactExtractor;
        _tagProvider = tagProvider;
    }

    public IEnumerable<Artifact> ToCodeArtifacts(Type classType)
    {
        var classKey = $"{ArtifactType}/{classType.AssemblyQualifiedName}";
        var methods = _classMethodArtifactExtractor.ExtractCodeArtifacts(classType);
        var classRelations = methods.Where(cr => cr.Type == IClassMethodArtifactExtractor.MethodArtifactType).Select(m =>
        {
            m.Relations.Add(new ArtifactRelation
            {
                ArtifactKey = classKey,
                Type = MethodClassRelationType,
            });

            return new ArtifactRelation
            {
                ArtifactKey = m.Key,
                Type = ClassMethodRelationType
            };
        }).ToList();

        yield return new CodeArtifact<Type>
        {
            Artifact = classType,
            Type = ArtifactType,
            Key = classKey,
            Name = classType.Name, //TODO: add extractor class
            Relations = classRelations,
            Tags = new List<ArtifactTag> { _tagProvider.GetTag("Class") }
        };

        foreach (var method in methods)
        {
            yield return method;
        }
    }

    public Task<IEnumerable<Artifact>> ExtractCodeArtifactsAsync(Assembly assembly) =>
        Task.FromResult<IEnumerable<Artifact>>(assembly
            .DefinedTypes
            .Where(t => _filters.All(f => f.IsValidCodeArtifact(t)))
            .SelectMany(ToCodeArtifacts)
            .ToList());
}
