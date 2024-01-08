using System.Reflection;
using NEvo.CodeAnalysis.ILParsing;
using static System.Reflection.Emit.OpCodes;

namespace NEvo.CodeAnalysis.Analysing.Processing;

public class CallArtifactProcessor : IArtifactProcessor
{
    private readonly ILInstructionParser _parser;
    public const string CallMethodRelationType = "code/Method/CallMethod";
    public const string CallConstructorRelationType = "code/Method/CallConstructor";
    public const string CalledByRelationType = "code/Method/CalledByMethod";

    public CallArtifactProcessor(ILInstructionParser parser)
    {
        _parser = parser;
    }

    public Task ProcessAsync(Artifact artifact, CodeAnalyzerContext context)
    {
        if (artifact is CodeArtifact<MethodBase> methodArtifact)
        {
            //todo: add cache
            var instructions = _parser.ParseMethodInfo(methodArtifact.Artifact);

            Console.WriteLine();
            Console.WriteLine("-----------------");
            Console.WriteLine(artifact.Name);
            Console.WriteLine("-----------------");
            Console.WriteLine();

            foreach (var ins in instructions)
            {
                Console.WriteLine($"{ins.OpCode.Name} - {ins.Operand?.GetType()} - {ins.Operand}");
            }

            var calledMethods = instructions
                .Where(instruction => instruction.OpCode == Call || instruction.OpCode == Callvirt)
                .Select(instruction => instruction.Operand)
                .OfType<MethodInfo>()
                .Distinct()
                .ToArray();

            foreach (var calledMethod in calledMethods)
            {
                var calledMethodArtifact = context.Get(calledMethod);
                if (calledMethodArtifact != null)
                {
                    var callRelation = new ArtifactRelation { Type = CallMethodRelationType, ArtifactKey = calledMethodArtifact.Key };
                    artifact.Relations.Add(callRelation);
                    var calledByRelation = new ArtifactRelation { Type = CalledByRelationType, ArtifactKey = artifact.Key };
                    calledMethodArtifact.Relations.Add(calledByRelation);
                }
                else
                {
                    var callRelation = new ArtifactRelation { Type = CallMethodRelationType, ArtifactKey = $"External/{calledMethod.ReflectedType.FullName}.{calledMethod.Name}" };
                    artifact.Relations.Add(callRelation);
                }
            }

            var calledConstructors = instructions
               .Where(instruction => /*instruction.OpCode == System.Reflection.Emit.OpCodes.Call*/ instruction.Operand is ConstructorInfo)
               .Select(instruction => instruction.Operand)
               .OfType<ConstructorInfo>()
               .Distinct()
               .ToArray();

            foreach (var calledConstructor in calledConstructors)
            {
                var calledMethodArtifact = context.Get(calledConstructor);
                if (calledMethodArtifact != null)
                {
                    var callRelation = new ArtifactRelation { Type = CallConstructorRelationType, ArtifactKey = calledMethodArtifact.Key };
                    artifact.Relations.Add(callRelation);
                    var calledByRelation = new ArtifactRelation { Type = CalledByRelationType, ArtifactKey = artifact.Key };
                    calledMethodArtifact.Relations.Add(calledByRelation);
                }
            }
        }

        return Task.CompletedTask;
    }
}
