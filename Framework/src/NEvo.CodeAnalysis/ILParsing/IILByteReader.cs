using System.Reflection.Emit;

namespace NEvo.CodeAnalysis.ILParsing;

public interface IILByteReader
{
    byte[] Bytes { get; }
    int Position { get; }

    byte ReadNextByte();
    double ReadNextDouble();
    float ReadNextFloat();
    int ReadNextInt32();
    long ReadNextInt64();
    sbyte ReadNextSByte();
    ushort ReadNextUInt16();
    bool TryGetNextOperation(out OpCode opCode);
}