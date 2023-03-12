namespace NEvo.Core;

public class RuleBuilder<TResult>
{
    private readonly Dictionary<string, (Func<bool> Predicate, Func<string, Exception> ExceptionFactory)> _rules = new Dictionary<string, (Func<bool> predicate, Func<string, Exception> exceptionFactory)>();
    private readonly Func<string, Exception> _defaultExceptionFactory;

    public RuleBuilder(Func<string, Exception> defaultExceptionFactory)
    {
        _defaultExceptionFactory = defaultExceptionFactory;
    }

    public RuleBuilder<TResult> Rule(string ruleName, Func<bool> predicate, Func<string, Exception>? exceptionFactory = null)
    {
        _rules.Add(ruleName, (predicate, exceptionFactory ?? _defaultExceptionFactory));
        return this;
    }

    public Try<TResult> OnSuccess(Func<TResult> func)
    {
        var rules = _rules.Select(kv => {
                try {
                    return kv.Value.Predicate() ? null : kv.Value.ExceptionFactory(kv.Key);
                }
                catch (Exception exc) {
                    return exc;
                } 
            })
            .OfType<Exception>()
            .ToList();

        return rules.Any() ? 
            Try.Failure<TResult>(rules.Count > 1 ? new AggregateException(rules) : rules.First()) : 
            Try.Of(func);
    }

    public Try<Unit> OnSuccess(Action action)
    {
        var rules = _rules.Select(kv => {
            try
            {
                return kv.Value.Predicate() ? null : kv.Value.ExceptionFactory(kv.Key);
            }
            catch (Exception exc)
            {
                return exc;
            }
        })
            .OfType<Exception>()
            .ToList();

        return rules.Any() ?
            Try.Failure(rules.Count > 1 ? new AggregateException(rules) : rules.First()) :
            Try.Of(action);
    }
}

