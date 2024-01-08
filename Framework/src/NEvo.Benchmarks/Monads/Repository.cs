using NEvo.Core;
using NEvo.Monads;

namespace NEvo.Benchmarks.Monads
{
    public class Repository
    {
        public Model Get_Exception(Guid id) { throw new NotFoundException(id); }
        public Maybe<Model> Get_Maybe(Guid id) { return Maybe.None; }

        internal void Save_Exception(Model model)
        {
            throw new NotImplementedException();
        }

        internal Either<Exception, Unit> Save_Either(Model model)
        {
            throw new NotImplementedException();
        }
    }
}


