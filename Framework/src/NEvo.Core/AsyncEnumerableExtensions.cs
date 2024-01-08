/* all of that should be in NEvo MVC integration project */

namespace NEvo.Core;

public static class AsyncEnumerableExtensions
{
    public static Task<List<T>> ToListAsync<T>(this IAsyncEnumerable<T> source)
    {
        return source == null ? throw new ArgumentNullException(nameof(source)) : ExecuteAsync();

        async Task<List<T>> ExecuteAsync()
        {
            var list = new List<T>();

            await foreach (var element in source)
            {
                list.Add(element);
            }

            return list;
        }
    }

    public static async Task<IEnumerable<TResult>> SelectManyAsync<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, Task<IEnumerable<TResult>>> selector)
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        var results = await Task.WhenAll(source.Select(selector));

        return results.SelectMany(x => x);
    }

    public static async Task ForEachAsync<TData>(this IEnumerable<TData> enumerable, Func<TData, Task> action)
    {
        foreach (var item in enumerable)
            await action(item);
    }
}