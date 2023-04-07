namespace NEvo.Monads;

public static class MaybeEnumerableExtensions
{
    public static IEnumerable<T> UnPack<T>(this IEnumerable<Maybe<T>> enumerable) => enumerable.Where(m => m.HasValue).Select(m => m.Value);
}

public static class MaybeEitherExtensions
{
    public static Either<TLeft, TRight> Match<T, TLeft, TRight>(this Maybe<T> maybe, Func<T, Either<TLeft, TRight>> some, Func<TLeft> none) => maybe.Match(
        value => some(value),
        () => Either.Left<TLeft, TRight>(none())
    );

    public static Task<Either<TLeft, TRight>> MatchAsync<T, TLeft, TRight>(this Maybe<T> maybe, Func<T, Task<Either<TLeft, TRight>>> some, Func<TLeft> none) => maybe.Match(
        async value => await some(value),
        () => Either.TaskLeft<TLeft, TRight>(none())
    );

    public static Maybe<T> MaybeFirst<T>(this IEnumerable<T> enumerable) => enumerable.FirstOrDefault();
    public static Maybe<T> MaybeFirst<T>(this IEnumerable<T> enumerable, T defaultValue) => enumerable.FirstOrDefault(defaultValue);
    public static Maybe<T> MaybeFirst<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate) => enumerable.FirstOrDefault(predicate);

    public static Maybe<T> MaybeSingle<T>(this IEnumerable<T> enumerable) => enumerable.SingleOrDefault();
    public static Maybe<T> MaybeSingle<T>(this IEnumerable<T> enumerable, T defaultValue) => enumerable.SingleOrDefault(defaultValue);
    public static Maybe<T> MaybeSingle<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate) => enumerable.SingleOrDefault(predicate);


    public static Maybe<TValue> MaybeGetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) => dictionary.TryGetValue(key, out var value) ? value : Maybe.None;

}