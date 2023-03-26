namespace NEvo.Core.StateManaging;

public interface ITransition<TContext>
{
    string Name { get; }
}
