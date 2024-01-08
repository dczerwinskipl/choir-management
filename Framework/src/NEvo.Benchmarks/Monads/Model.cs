using NEvo.Core;
using NEvo.Monads;

namespace NEvo.Benchmarks.Monads
{
    public record Model()
    {
        public void DoSomething_Exception()
        {
        }

        public Either<Exception, Unit> DoSomething_Either()
        {
            return Either.Success();
        }
    }
}


