using System.Reflection;
using System.Reflection.Emit;

namespace NEvo.CodeAnalysis.ILParsing;

public class ILOperandResolver : IILOperandResolver
{
    private readonly MethodBase _methodBase;
    private readonly Module _module;

    public ILOperandResolver(MethodBase methodBase)
    {
        _methodBase = methodBase;
        _module = methodBase.Module;
    }

    public object? GetOperand(OpCode code, IILByteReader ilByteReader)
    {
        int metadataToken;
        switch (code.OperandType)
        {
            case OperandType.InlineBrTarget:
                metadataToken = ilByteReader.ReadNextInt32();
                metadataToken += ilByteReader.Position;
                return metadataToken;
            case OperandType.InlineField:
                metadataToken = ilByteReader.ReadNextInt32();
                return _module.ResolveField(metadataToken, _methodBase.DeclaringType.GetGenericArguments() ?? null, (_methodBase as MethodInfo)?.GetGenericArguments() ?? null);
            case OperandType.InlineMethod:
                metadataToken = ilByteReader.ReadNextInt32();
                try
                {
                    return _module.ResolveMethod(metadataToken, _methodBase.DeclaringType.GetGenericArguments(), (_methodBase as MethodInfo)?.GetGenericArguments() ?? null);
                }
                catch
                {
                    return _module.ResolveMember(metadataToken, _methodBase.DeclaringType.GetGenericArguments(), (_methodBase as MethodInfo)?.GetGenericArguments() ?? null);
                }
            case OperandType.InlineSig:
                metadataToken = ilByteReader.ReadNextInt32();
                return _module.ResolveSignature(metadataToken);
            case OperandType.InlineTok:
                metadataToken = ilByteReader.ReadNextInt32();
                try
                {
                    return _module.ResolveType(metadataToken, _methodBase.DeclaringType.GetGenericArguments(), (_methodBase as MethodInfo)?.GetGenericArguments() ?? null);
                }
                catch
                {
                    // Handle the exception if needed
                }
                break;
            case OperandType.InlineType:
                metadataToken = ilByteReader.ReadNextInt32();
                return _module.ResolveType(metadataToken, _methodBase.DeclaringType.GetGenericArguments(), (_methodBase as MethodInfo)?.GetGenericArguments() ?? null);
            case OperandType.InlineI:
                return ilByteReader.ReadNextInt32();
            case OperandType.InlineI8:
                return ilByteReader.ReadNextInt64();
            case OperandType.InlineNone:
                return null;
            case OperandType.InlineR:
                return ilByteReader.ReadNextDouble();
            case OperandType.InlineString:
                metadataToken = ilByteReader.ReadNextInt32();
                return _module.ResolveString(metadataToken);
            case OperandType.InlineSwitch:
                int count = ilByteReader.ReadNextInt32();
                int[] casesAddresses = new int[count];
                for (int i = 0; i < count; i++)
                {
                    casesAddresses[i] = ilByteReader.ReadNextInt32();
                }
                int[] cases = new int[count];
                for (int i = 0; i < count; i++)
                {
                    cases[i] = ilByteReader.Position + casesAddresses[i];
                }
                // Return cases or handle them as needed
                break;
            case OperandType.InlineVar:
                return ilByteReader.ReadNextUInt16();
            case OperandType.ShortInlineBrTarget:
                return ilByteReader.ReadNextSByte() + ilByteReader.Position;
            case OperandType.ShortInlineI:
                return ilByteReader.ReadNextSByte();
            case OperandType.ShortInlineR:
                return ilByteReader.ReadNextFloat();
            case OperandType.ShortInlineVar:
                return ilByteReader.ReadNextByte();
            default:
                throw new Exception("Unknown operand type.");
        }

        return null;
    }
}
