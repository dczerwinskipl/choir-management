namespace NEvo.CodeAnalysis.Analysing.Extracting.Code;

public class NonGenericTypeDefinitionArtifactFilter : IClassArtifactFilter
{
    public bool IsValidCodeArtifact(Type type) => !type.IsGenericTypeDefinition;
}
