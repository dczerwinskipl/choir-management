/* all of that should be in NEvo MVC integration project */
namespace NEvo.Core;

public static class EnumerableExtensions
{
    public static void ForEach<TData>(this IEnumerable<TData> enumerable, Action<TData> action)
    {
        foreach (var item in enumerable)
            action(item);
    }
}
