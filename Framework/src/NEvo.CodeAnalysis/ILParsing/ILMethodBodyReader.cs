using System.Reflection;

namespace NEvo.CodeAnalysis.ILParsing;

public class ILMethodBodyReader : IILMethodBodyReader
{
    private readonly IILOperandResolverFactory _operandResolverFactory;
    private readonly IILByteReaderFactory _ilByteReaderFactory;

    public ILMethodBodyReader(IILOperandResolverFactory ilOperandResolverFactory, IILByteReaderFactory ilByteReaderFactory)
    {
        _ilByteReaderFactory = ilByteReaderFactory;
        _operandResolverFactory = ilOperandResolverFactory;
    }

    public ILMethodBodyReader() : this(new ILOperandResolverFactory(), new ILByteReaderFactory())
    {
    }

    public IEnumerable<ILInstruction> ReadInstructions(MethodBase methodBase)
    {
        var ilByteReader = _ilByteReaderFactory.CreateILByteReader(methodBase);
        var ilOperandResolver = _operandResolverFactory.CreateILOperandResolver(methodBase);

        while (ilByteReader.TryGetNextOperation(out var opCode))
        {
            yield return new ILInstruction(ilByteReader.Position - 1, opCode, ilOperandResolver.GetOperand(opCode, ilByteReader));
        }
    }
}
