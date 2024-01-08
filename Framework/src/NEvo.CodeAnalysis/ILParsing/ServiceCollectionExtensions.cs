using Microsoft.Extensions.DependencyInjection;
using NEvo.CodeAnalysis.ILParsing;

namespace NEvo.CodeAnalysis.Analysing.ILParsing;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddIlInstructionParser(this IServiceCollection services)
    {
        services.AddSingleton<ILInstructionParser>();
        services.AddSingleton<IILMethodBodyReader, ILMethodBodyReader>();
        services.AddSingleton<IILOperandResolverFactory, ILOperandResolverFactory>();
        services.AddSingleton<IILByteReaderFactory, ILByteReaderFactory>();

        return services;
    }
}
