using System.Reflection;
using System.Reflection.Emit;

namespace NEvo.CodeAnalysis.ILParsing;

public class ILByteReader : IILByteReader
{
    public int Position { get; internal set; }
    public byte[] Bytes { get; internal set; }

    public ILByteReader(MethodBase methodBase)
    {
        Bytes = methodBase.GetMethodBody()?.GetILAsByteArray() ?? new byte[0];
    }

    public bool TryGetNextOperation(out OpCode opCode)
    {
        if (Position >= Bytes.Length)
        {
            opCode = new OpCode();
            return false;
        }

        ushort value = Bytes[Position++];
        if (value != 0xfe)
        {
            opCode = ILOpCodes.SingleByteOpCodes[value];
        }
        else
        {
            value = Bytes[Position++];
            opCode = ILOpCodes.MultiByteOpCodes[value];
        }

        return true;
    }

    public int ReadNextInt32()
    {
        return Bytes[Position++] | (Bytes[Position++] << 8) | (Bytes[Position++] << 0x10) | (Bytes[Position++] << 0x18);
    }

    public long ReadNextInt64()
    {
        return Bytes[Position++] | (Bytes[Position++] << 8) | (Bytes[Position++] << 0x10) | (Bytes[Position++] << 0x18) | (Bytes[Position++] << 0x20) | (Bytes[Position++] << 0x28) | (Bytes[Position++] << 0x30) | (Bytes[Position++] << 0x38);
    }

    public double ReadNextDouble()
    {
        long value = ReadNextInt64();
        return BitConverter.Int64BitsToDouble(value);
    }

    public sbyte ReadNextSByte()
    {
        return (sbyte)Bytes[Position++];
    }

    public byte ReadNextByte()
    {
        return Bytes[Position++];
    }

    public ushort ReadNextUInt16()
    {
        return (ushort)(Bytes[Position++] | (Bytes[Position++] << 8));
    }

    public float ReadNextFloat()
    {
        int value = ReadNextInt32();
        return BitConverter.ToSingle(BitConverter.GetBytes(value), 0);
    }
}
