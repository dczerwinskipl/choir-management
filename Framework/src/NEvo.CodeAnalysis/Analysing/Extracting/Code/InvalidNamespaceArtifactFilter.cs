using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;

namespace NEvo.CodeAnalysis.Analysing.Extracting.Code;

public class InvalidNamespaceArtifactFilter : IClassArtifactFilter
{
    private readonly bool _useRegex;
    private readonly HashSet<string>? _namespaces;
    private readonly HashSet<Regex>? _namespaceRegexes;

    public InvalidNamespaceArtifactFilter(IOptions<InvalidNamespaceArtifactFilterOptions> options)
    {
        _useRegex = options.Value.UseRegex;
        if (options.Value.UseRegex)
        {
            _namespaceRegexes = options.Value.Namespaces.Select(n => new Regex(n)).ToHashSet();
        }
        else
        {
            _namespaces = options.Value.Namespaces.ToHashSet();
        }
    }

    public bool IsValidCodeArtifact(Type type) => _useRegex
        ? _namespaceRegexes!.Any(r => r.IsMatch(type.Namespace ?? string.Empty))
        : _namespaces!.Contains(type.Namespace ?? string.Empty);
}