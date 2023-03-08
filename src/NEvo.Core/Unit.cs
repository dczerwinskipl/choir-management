namespace NEvo.Core;

/// <summary>
/// Represents a void type
/// </summary>
public struct Unit
{
    private static readonly Unit _value = new();
    public static ref readonly Unit Value => ref _value;
    public static Task<Unit> Task => System.Threading.Tasks.Task.FromResult(_value);
}