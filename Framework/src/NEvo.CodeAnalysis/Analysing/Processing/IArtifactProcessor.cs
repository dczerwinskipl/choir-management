namespace NEvo.CodeAnalysis.Analysing.Processing;

public interface IArtifactProcessor
{
    public Task ProcessAsync(Artifact artifact, CodeAnalyzerContext context);
}
