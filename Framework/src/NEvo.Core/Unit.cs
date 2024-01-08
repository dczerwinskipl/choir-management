namespace NEvo.Core;

/// <summary>
/// Represents a void type
/// </summary>
public readonly struct Unit
{
    private static readonly Unit _value = new();
    public static ref readonly Unit Value => ref _value;
    public static Task<Unit> Task => System.Threading.Tasks.Task.FromResult(_value);
}

public readonly struct Null
{
    private static readonly Null _value = new();
    public static ref readonly Null Value => ref _value;
    public static Task<Null> Task => System.Threading.Tasks.Task.FromResult(_value);
}

public class NotFoundException : Exception
{
    public object Id { get; }
    public NotFoundException(object id) { Id = id; }

}