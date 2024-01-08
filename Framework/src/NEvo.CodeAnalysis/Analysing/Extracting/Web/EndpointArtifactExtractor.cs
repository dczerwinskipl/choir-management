using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using NEvo.CodeAnalysis.Analysing.Extracting.Code;

namespace NEvo.CodeAnalysis.Analysing.Extracting.Web;

public class EndpointArtifactExtractor : IArtifactExtractor
{
    private readonly EndpointDataSource _endpointDataSource;

    public EndpointArtifactExtractor(EndpointDataSource endpointDataSource)
    {
        _endpointDataSource = endpointDataSource;
    }

    public Task<IEnumerable<Artifact>> ExtractCodeArtifactsAsync(Assembly assembly) =>
        Task.FromResult<IEnumerable<Artifact>>(_endpointDataSource.Endpoints.Select(ToCodeArtifacts).OfType<Artifact>().ToList());

    private Artifact? ToCodeArtifacts(Endpoint endpoint)
    {
        var metadata = endpoint.Metadata;
        var method = metadata.OfType<MethodInfo>().FirstOrDefault();

        if (method is null)
        {
            return null;
        }
        var (methodName, _) = ClassMethodArtifactExtractor.ExtractNames(method);

        return new CodeArtifact<MethodBase>
        {
            Key = $"{IClassMethodArtifactExtractor.MethodArtifactType}/{endpoint.DisplayName ?? methodName}", //also add generic params!,
            Name = endpoint.DisplayName ?? methodName,
            Artifact = method,
            Type = IClassMethodArtifactExtractor.MethodArtifactType
        };
    }
}
