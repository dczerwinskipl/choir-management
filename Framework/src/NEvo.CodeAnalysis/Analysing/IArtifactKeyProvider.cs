namespace NEvo.CodeAnalysis.Analysing;

public interface IArtifactKeyProvider
{
    string GetKey(object artifact);
}

public class ArtifactKeyProvider : IArtifactKeyProvider
{
    public string GetKey(object artifact)
    {
        if ((artifact as Type) != null)
        {
            var type = (Type)artifact;

        }
        return "";
    }
}