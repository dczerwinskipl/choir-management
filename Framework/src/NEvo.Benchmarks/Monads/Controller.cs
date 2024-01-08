using Microsoft.AspNetCore.Http;
using NEvo.Core.Web;

namespace NEvo.Benchmarks.Monads
{
    public class Controller
    {
        private Service _service;

        public Controller(Service service)
        {
            _service = service;
        }

        public IResult DoSomethingWithModel_Exception_Middleware(Guid id)
        {
            // try/catch as a simluation of middleware
            try
            {
                _service.DoSomethingWithModel_Exception(id);
                return Results.Ok();
            } 
            catch (Exception e)
            {
                return e.ToResult();
            }
        }

        public IResult DoSomethingWithModel_ServiceResult_ExceptionRethrow_Middleware(Guid id)
        {
            // try/catch as a simluation of middleware
            try
            {
                var result = _service.DoSomethingWithModel_Exception_Catch(id);
                if (!result.Success)
                    throw result.Exception;

                return Results.Ok();
            }
            catch (Exception e)
            {
                return e.ToResult();
            }
        }

        public IResult DoSomethingWithModel_ServiceResult(Guid id)
        {
            var result = _service.DoSomethingWithModel_Exception_Catch(id);
            return result.Success ? Results.Ok() : result.Exception.ToResult();
        }

        public IResult DoSomethingWithModel_Either(Guid id) =>
            _service
                .DoSomethingWithModel_Either(id)
                .ToResults();
    }
}


