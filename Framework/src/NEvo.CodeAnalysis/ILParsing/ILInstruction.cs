using System.Reflection.Emit;

namespace NEvo.CodeAnalysis.ILParsing;

public record ILInstruction(int Offset, OpCode OpCode, object? Operand);
