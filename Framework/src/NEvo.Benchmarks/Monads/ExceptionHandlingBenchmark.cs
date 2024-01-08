using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Http;
using NEvo.Core;
using NEvo.Core.Web;
using NEvo.Monads;

namespace NEvo.Benchmarks.Monads
{
    public class ExceptionHandlingBenchmark
    {
        private readonly Repository _repository;
        private readonly Service _service;
        private readonly Controller _controller;

        public ExceptionHandlingBenchmark()
        {
            _repository = new Repository();
            _service = new Service(_repository);
            _controller = new Controller(_service);
        }

        [Benchmark]
        public IResult NotFound_Maybe()
        {
            return _repository
                        .Get_Maybe(Guid.NewGuid())
                        .Match(
                            found => Results.Ok(found),
                            none: () => Results.NotFound());
        }

        [Benchmark]
        public IResult NotFound_Exception_Catch()
        {
            try
            {
                return Results.Ok(_repository.Get_Exception(Guid.NewGuid()));
            }
            catch (Exception e)
            {
                return e.ToResult(); //Results.NotFound();
            }
        }

        [Benchmark]
        public IResult NotFound_Exception_TryOf()
        {
            return Try
                    .Of(() => _repository.Get_Exception(Guid.NewGuid()))
                    .ToResults();
        }

        [Benchmark]
        public Either<Exception, Unit> Service_Either()
        {
            return _service.DoSomethingWithModel_Either(Guid.NewGuid());
        }

        [Benchmark]
        public ServiceResult Service_Exception_Catch()
        {
            return _service.DoSomethingWithModel_Exception_Catch(Guid.NewGuid());
        }

        [Benchmark]
        public ServiceResult Service_Exception_TryOf()
        {
            return _service.DoSomethingWithModel_Exception_TryOf(Guid.NewGuid());
        }


        [Benchmark]
        public IResult DoSomethingWithModel_ServiceResult_ExceptionRethrow_Middleware()
        {
            return _controller.DoSomethingWithModel_ServiceResult_ExceptionRethrow_Middleware(Guid.NewGuid());
        }

        [Benchmark]
        public IResult DoSomethingWithModel_Exception_Middleware()
        {
            return _controller.DoSomethingWithModel_Exception_Middleware(Guid.NewGuid());
        }

        [Benchmark]
        public IResult DoSomethingWithModel_ServiceResult()
        {
            return _controller.DoSomethingWithModel_ServiceResult(Guid.NewGuid());
        }

        [Benchmark]
        public IResult DoSomethingWithModel_Either()
        {
            return _controller.DoSomethingWithModel_Either(Guid.NewGuid());
        }
    }
}


