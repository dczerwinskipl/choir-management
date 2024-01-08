namespace NEvo.CodeAnalysis.Analysing.Extracting.Code;

public class InvalidNamespaceArtifactFilterOptions
{
    public bool UseRegex { get; set; } = true;
    public IEnumerable<string> Namespaces { get; set; } = Enumerable.Empty<string>();
}
