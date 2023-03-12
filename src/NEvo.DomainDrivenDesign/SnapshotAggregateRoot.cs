using NEvo.Core;
using NEvo.Messaging.Events;
using NEvo.ValueObjects;

namespace NEvo.DomainDrivenDesign;

public abstract class AggregateRoot<TAggregateRoot, TKey> where TAggregateRoot : AggregateRoot<TAggregateRoot, TKey>
{
    public TKey Id { get; }

    protected AggregateRoot(TKey id)
    {
        Id = Check.Null(id);
    }

#pragma warning disable CS8618 // ORM only
    protected AggregateRoot()
#pragma warning restore CS8618 // ORM only
    {

    }

    public static RuleBuilder<TResult> Rule<TResult>(string ruleName, Func<bool> predicate, Func<string, Exception>? exceptionFactory = null)
    {
        var ruleBuilder = new RuleBuilder<TResult>(ruleName => new DomainRuleValidationException(ruleName));
        return ruleBuilder.Rule(ruleName, predicate, exceptionFactory);
    }

    public RuleBuilder<Unit> Rule(string ruleName, Func<bool> predicate, Func<string, Exception>? exceptionFactory = null) => Rule<Unit>(ruleName, predicate, exceptionFactory); 
}

public abstract class SnapshotAggregateRoot<TAggregateRoot, TKey> : AggregateRoot<TAggregateRoot, TKey> where TAggregateRoot : SnapshotAggregateRoot<TAggregateRoot, TKey>
{
    private readonly List<Event> _pendingEvents = new List<Event>();
    public void Publish<TEvent>(TEvent @event) where TEvent : Event
    {
        @event.Source = GetSource();
        _pendingEvents.Add(@event);
    }

    protected virtual ObjectId? GetSource() => ObjectId.New(GetType().Name, Id.ToString());

    protected SnapshotAggregateRoot() : base() { }

    protected SnapshotAggregateRoot(TKey id) : base(id)
    {
    }
}
