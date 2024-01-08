using System.Reflection.Emit;

namespace NEvo.CodeAnalysis.ILParsing;

public interface IILOperandResolver
{
    object? GetOperand(OpCode code, IILByteReader ilByteReader);
}
