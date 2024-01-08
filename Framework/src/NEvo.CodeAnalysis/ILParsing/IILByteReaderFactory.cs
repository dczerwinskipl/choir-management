using System.Reflection;

namespace NEvo.CodeAnalysis.ILParsing;

public interface IILByteReaderFactory
{
    IILByteReader CreateILByteReader(MethodBase method);
}
