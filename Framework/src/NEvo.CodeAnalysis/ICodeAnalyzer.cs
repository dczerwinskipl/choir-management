using System.Reflection;
using NEvo.CodeAnalysis.Analysing;

namespace NEvo.CodeAnalysis;

public interface ICodeAnalyzer
{
    Task<IEnumerable<Artifact>> AnalyzeAsync(params Assembly[] assemblies);
}