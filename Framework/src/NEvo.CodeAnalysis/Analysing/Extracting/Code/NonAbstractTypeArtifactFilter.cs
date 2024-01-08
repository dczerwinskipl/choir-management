namespace NEvo.CodeAnalysis.Analysing.Extracting.Code;

public class NonAbstractTypeArtifactFilter : IClassArtifactFilter
{
    public bool IsValidCodeArtifact(Type type) => !(type.IsAbstract && !type.IsSealed) && !type.IsInterface;
}
