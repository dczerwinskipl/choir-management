using System.Reflection;

namespace NEvo.CodeAnalysis.ILParsing;

public interface IILMethodBodyReader
{
    IEnumerable<ILInstruction> ReadInstructions(MethodBase methodBase);
}
