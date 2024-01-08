using Microsoft.AspNetCore.Http;
using NEvo.Core.Web;
using NEvo.CQRS.Processing;
using NEvo.Monads;

namespace NEvo.CQRS.Web;

public static class ProcessingEitherWebExtensions
{
    public static IResult ToResults<TResult>(this Either<IMessageProcessingFailures, TResult> either) =>
        either.Map(EitherWebExtensions.OnSuccess, OnFailure);

    public static async Task<IResult> ToResults<TResult>(this Task<Either<IMessageProcessingFailures, TResult>> either) =>
        await either.Map(EitherWebExtensions.OnSuccess, OnFailure);

    private static IResult OnFailure(IMessageProcessingFailures failures) =>
        (failures.Count() == 1
            ? failures.Single().Exception
            : new AggregateException(failures.Select(f => f.Exception))).ToResult();
}
