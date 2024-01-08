using System.Reflection;
using System.Reflection.Emit;

namespace NEvo.CodeAnalysis.ILParsing;

public static class ILOpCodes
{
    public static OpCode[] MultiByteOpCodes
    {
        get
        {
            if (_multiByteOpCodes == null)
                LoadOpCodes();

            return _multiByteOpCodes!;
        }
    }
    public static OpCode[] SingleByteOpCodes
    {
        get
        {
            if (_singleByteOpCodes == null)
                LoadOpCodes();

            return _singleByteOpCodes!;
        }
    }

    private static OpCode[]? _multiByteOpCodes;
    private static OpCode[]? _singleByteOpCodes;

    private static void LoadOpCodes()
    {
        _singleByteOpCodes = new OpCode[0x100];
        _multiByteOpCodes = new OpCode[0x100];
        FieldInfo[] infoArray1 = typeof(OpCodes).GetFields();
        for (int num1 = 0; num1 < infoArray1.Length; num1++)
        {
            FieldInfo info1 = infoArray1[num1];
            if (info1.FieldType == typeof(OpCode))
            {
                OpCode code1 = (OpCode)info1.GetValue(null)!;
                ushort num2 = (ushort)code1.Value;
                if (num2 < 0x100)
                {
                    _singleByteOpCodes[num2] = code1;
                }
                else
                {
                    if ((num2 & 0xff00) != 0xfe00)
                    {
                        throw new Exception("Invalid OpCode.");
                    }
                    _multiByteOpCodes[num2 & 0xff] = code1;
                }
            }
        }
    }
}