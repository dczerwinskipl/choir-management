using System.Reflection;

namespace NEvo.CodeAnalysis.ILParsing;

public interface IILOperandResolverFactory
{
    IILOperandResolver CreateILOperandResolver(MethodBase methodBase);
}
