using NEvo.Core;
using NEvo.Monads;

namespace NEvo.Benchmarks.Monads
{
    public class Service
    {
        private readonly Repository _repository;
        public Service(Repository repository)
        {
            _repository = repository;
        }

        public ServiceResult DoSomethingWithModel_Exception(Guid id)
        {
            var model = _repository.Get_Exception(id);
            model.DoSomething_Exception();
            _repository.Save_Exception(model);
            return new ServiceResult();
        }

        public ServiceResult DoSomethingWithModel_Exception_Catch(Guid id)
        {
            try
            {
                return DoSomethingWithModel_Exception(id);
            }
            catch (Exception exc)
            {
                return new ServiceResult(exc);
            }
        }

        public ServiceResult DoSomethingWithModel_Exception_TryOf(Guid id) =>
            Try.Of(() => _repository.Get_Exception(id))
                .Then(model => model
                                .DoSomething_Either()
                                .Then(() => _repository.Save_Either(model)))
                .Map(
                    success => new ServiceResult(),
                    failure => new ServiceResult(failure)
                );

        public Either<Exception, Unit> DoSomethingWithModel_Either(Guid id) =>
            _repository
                .Get_Maybe(id)
                .Match(
                    some: model => model
                                    .DoSomething_Either()
                                    .Then(() => _repository.Save_Either(model)),
                    none: () => Either.Failure(new NotFoundException(id))
                );
    }
}


