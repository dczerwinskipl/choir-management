using Microsoft.AspNetCore.Http;
using NEvo.Monads;

namespace NEvo.Core.Web;


public static class EitherWebExtensions
{

    public static IResult ToResults<TResult>(this Either<Exception, TResult> either) =>
        either.Map(OnSuccess, OnFailure);

    public static async Task<IResult> ToResults<TResult>(this Task<Either<Exception, TResult>> either) =>
        await either.Map(OnSuccess, OnFailure);

    public static IResult OnSuccess<TResult>(TResult result) => Results.Ok(result);

    public static IResult OnFailure(Exception exc) =>
        exc switch
        {
            ArgumentException argumentException => Results.BadRequest(
                    error: argumentException.Message
                ),
            _ => Results.Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    detail: exc.Message
                )
        };
}

