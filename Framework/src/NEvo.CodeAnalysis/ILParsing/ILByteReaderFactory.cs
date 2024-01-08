using System.Reflection;

namespace NEvo.CodeAnalysis.ILParsing;

public class ILByteReaderFactory : IILByteReaderFactory
{
    public IILByteReader CreateILByteReader(MethodBase method) => new ILByteReader(method);
}
