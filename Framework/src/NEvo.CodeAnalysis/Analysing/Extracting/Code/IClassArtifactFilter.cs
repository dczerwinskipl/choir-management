namespace NEvo.CodeAnalysis.Analysing.Extracting.Code;

public interface IClassArtifactFilter
{
    public bool IsValidCodeArtifact(Type type);
}
