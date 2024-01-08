using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

namespace NEvo.CodeAnalysis.ILParsing;

public class ILInstructionParser
{
    private const string StateMachineMoveNextMethodName = nameof(IAsyncStateMachine.MoveNext);
    private readonly IILMethodBodyReader _methodBodyReader;
    private readonly ILogger<ILInstructionParser> _logger;

    public ILInstructionParser(ILogger<ILInstructionParser> logger) : this(logger, new ILMethodBodyReader()) { }

    public ILInstructionParser(ILogger<ILInstructionParser> logger, IILMethodBodyReader methodBodyReader)
    {
        _logger = logger;
        _methodBodyReader = methodBodyReader;
    }

    public IEnumerable<ILInstruction> ParseMethodInfo(MethodBase methodBase)
    {
        var methodToParse =
            IsStateMachine(methodBase, out var stateMachineBody) ? stateMachineBody
            : IsGenerciMethodDefinition(methodBase, out var genericMethodBase) ? genericMethodBase
            : methodBase;
        return methodToParse != null ? _methodBodyReader.ReadInstructions(methodToParse) : Enumerable.Empty<ILInstruction>();
    }

    private bool IsStateMachine(MethodBase methodBase, out MethodBase? stateMachineBody)
    {
        var stateMachineAttribute = methodBase.GetCustomAttributes(typeof(StateMachineAttribute), true).FirstOrDefault() as StateMachineAttribute;
        if (stateMachineAttribute != null)
        {
            stateMachineBody = stateMachineAttribute.StateMachineType.GetMethod(StateMachineMoveNextMethodName, BindingFlags.NonPublic | BindingFlags.Instance)!;
            return true;
        }

        stateMachineBody = null;
        return false;
    }

    private bool IsGenerciMethodDefinition(MethodBase methodBase, out MethodBase? genericMethodBase)
    {
        if (methodBase is MethodInfo methodInfo && methodBase.IsGenericMethodDefinition)
        {
            var canCreate = true;
            var genericArguments = methodBase.GetGenericArguments().Select(arg =>
            {
                var constraints = arg.GetGenericParameterConstraints();
                if (constraints.Length > 1)
                {
                    var buildType = CreateCustomType(methodBase, $"{methodBase.Name}_{arg.Name}", constraints);
                    canCreate = buildType != null;
                    return buildType;
                }
                return constraints.FirstOrDefault() ?? typeof(object);
            }).ToArray();
            genericMethodBase = canCreate ? methodInfo.MakeGenericMethod(genericArguments) : null;
            return true;
        }
        genericMethodBase = null;
        return false;
    }

    private Type? CreateCustomType(MethodBase methodBase, string typeName, Type[] constraints)
    {
        AssemblyName assemblyName = new("DynamicAssembly");
        AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);

        ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("DynamicModule");

        TypeBuilder typeBuilder = moduleBuilder.DefineType(typeName, TypeAttributes.Public);

        // Implement interfaces
        foreach (Type interfaceType in constraints.Where(c => c.IsInterface))
        {
            typeBuilder.AddInterfaceImplementation(interfaceType);
            ImplementInterfaceMembers(typeBuilder, interfaceType);
        }

        var baseClasses = constraints.Where(c => !c.IsInterface);
        if (baseClasses.Count() > 1)
        {
            _logger.LogWarning("Cannot parse method {MethodName} in type {TypeName}. Found complex constrains on generic type.", methodBase.Name, methodBase.ReflectedType.AssemblyQualifiedName);
            return null;
        }
        else
        {
            var baseType = baseClasses.FirstOrDefault();
            if (baseType != null)
            {
                typeBuilder.SetParent(baseType);
                ImplementBaseClassMembers(typeBuilder, baseType);
            }

            Type generatedType = typeBuilder.CreateType();
            return generatedType;
        }
    }


    private static void ImplementInterfaceMembers(TypeBuilder typeBuilder, Type interfaceType)
    {
        foreach (MethodInfo method in interfaceType.GetMethods())
        {
            ImplementMethod(typeBuilder, method);
        }
    }

    private static void ImplementBaseClassMembers(TypeBuilder typeBuilder, Type baseType)
    {
        foreach (MethodInfo method in baseType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
        {
            if (method.IsAbstract || method.IsVirtual)
            {
                ImplementMethod(typeBuilder, method);
            }
        }
    }

    private static void ImplementMethod(TypeBuilder typeBuilder, MethodInfo method)
    {
        MethodBuilder methodBuilder = typeBuilder.DefineMethod(
            method.Name,
            MethodAttributes.Public | MethodAttributes.Virtual,
            method.ReturnType,
            method.GetParameters().Select(p => p.ParameterType).ToArray()
        );

        ILGenerator ilGenerator = methodBuilder.GetILGenerator();

        if (method.ReturnType == typeof(void))
        {
            ilGenerator.Emit(OpCodes.Ret);
        }
        else
        {
            LocalBuilder returnValue = ilGenerator.DeclareLocal(method.ReturnType);

            if (method.ReturnType.IsValueType)
            {
                ilGenerator.Emit(OpCodes.Ldloca, returnValue);
                ilGenerator.Emit(OpCodes.Initobj, method.ReturnType);
            }
            else
            {
                ilGenerator.Emit(OpCodes.Ldnull);
                ilGenerator.Emit(OpCodes.Stloc, returnValue);
            }

            ilGenerator.Emit(OpCodes.Ldloc, returnValue);
            ilGenerator.Emit(OpCodes.Ret);
        }
    }
}
