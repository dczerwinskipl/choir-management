using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace NEvo.CodeAnalysis.Web;

public static class EndpointExtensions
{
    public static TApplication UseCodeDocumentation<TApplication>(this TApplication application, string basePath, params Assembly[] assemblies) where TApplication : IEndpointRouteBuilder
    {
        application.MapGet(basePath, async ([FromServices] ICodeAnalyzer codeAnalyzer) =>
            await codeAnalyzer.AnalyzeAsync(assemblies)
        );
        return application;
    }
}
