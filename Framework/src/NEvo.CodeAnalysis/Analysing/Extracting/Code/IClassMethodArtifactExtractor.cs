namespace NEvo.CodeAnalysis.Analysing.Extracting.Code;

public interface IClassMethodArtifactExtractor
{
    static string ConstructorArtifactType = "code/Class/Constructor";
    static string MethodArtifactType = "code/Class/Method";
    IEnumerable<Artifact> ExtractCodeArtifacts(Type classType);
}
