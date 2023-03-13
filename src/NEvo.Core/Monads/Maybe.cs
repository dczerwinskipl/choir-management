using NEvo.Core;

namespace NEvo.Monads;

public static class Maybe
{
    public class MaybeNone
    {
    }

    public static MaybeNone None { get; } = new MaybeNone();

    public static Maybe<T> Some<T>(T value) => Check.Null(value);
}
