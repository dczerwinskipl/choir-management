namespace NEvo.CodeAnalysis.Analysing.Extracting.Code;

public interface IClassMethodArtifactFilter
{
    public bool IsValidCodeArtifact(Type type);
}
