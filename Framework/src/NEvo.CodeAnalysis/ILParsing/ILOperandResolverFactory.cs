using System.Reflection;

namespace NEvo.CodeAnalysis.ILParsing;

public class ILOperandResolverFactory : IILOperandResolverFactory
{
    public IILOperandResolver CreateILOperandResolver(MethodBase methodBase) => new ILOperandResolver(methodBase);
}
