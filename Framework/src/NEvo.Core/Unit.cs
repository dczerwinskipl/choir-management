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

public struct Null
{
    private static readonly Null _value = new();
    public static ref readonly Null Value => ref _value;
    public static Task<Null> Task => System.Threading.Tasks.Task.FromResult(_value);
}

public class NotFoundException : Exception
{
    public NotFoundException(object id) { }
}