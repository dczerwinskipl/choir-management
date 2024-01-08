using Microsoft.AspNetCore.Http;
using NEvo.Monads;

namespace NEvo.Core.Web;

public static class EitherWebExtensions
{
    public static IResult ToResults<TResult>(this Either<Exception, TResult> either) =>
        either.Map(OnSuccess, exc => exc.ToResult());

    public static async Task<IResult> ToResults<TResult>(this Task<Either<Exception, TResult>> either) =>
        await either.Map(OnSuccess, exc => exc.ToResult());

    public static IResult OnSuccess<TResult>(TResult result) => Results.Ok(result);

    // that should go to some ResultsExtensions or sth
    public static IResult ToResult(this Exception exc) =>
        exc switch
        {
            ArgumentException argumentException => Results.BadRequest(
                    error: argumentException.Message
                ),
            NotFoundException notFoundException => Results.NotFound(notFoundException.Id),
            _ => Results.Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    detail: exc.Message
                )
        };
}

