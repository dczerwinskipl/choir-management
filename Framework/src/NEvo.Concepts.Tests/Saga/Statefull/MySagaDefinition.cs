using NEvo.Messaging.Events;
using NEvo.Sagas.Stateful;
using NEvo.Sagas.Stateful.Handling;
using NEvo.ValueObjects;

namespace NEvo.Concepts.Tests.Saga.Statefull;



public record MailConfirmed(ObjectId Source, Guid VerificationId) : Event(Source);
public record MailVerificationStarted(ObjectId Source, Guid VerificationId) : Event(Source);

public class MySaga : IStatefulSaga<string>
{
    public Guid Id { get; private set; }
    public bool IsCompleted { get; private set; }
    public string State { get; set; }
    public Guid VerificationId { get; internal set; }

    public MySaga()
    {
        Id = Guid.NewGuid();
        IsCompleted = false;
    }
}

public class MySagaHandler : SagaStateMachineHandler<MySaga, string>
{
    public static class States
    {
        public static readonly string WaitForConfirmation = nameof(WaitForConfirmation);
        public static readonly string Confirmed = nameof(Confirmed);
        public static readonly string Rejected = nameof(Rejected);
    }

    public MySagaHandler()
    {
        FindSaga(
            ByMessage<MailVerificationStarted>(@event => saga => saga.VerificationId == @event.VerificationId),
            ByMessage<MailConfirmed>(@event => saga => saga.VerificationId == @event.VerificationId)
        );

        Saga<MySaga>();

        Initialy(OnMessage<MailVerificationStarted>()
                    .TransitionTo(States.WaitForConfirmation));

        State(States.WaitForConfirmation,
            OnMessage<MailConfirmed>()
                .TransitionTo(States.Confirmed));

        State(States.Confirmed);

        AnyState(
            OnTimer(TimeSpan.FromMinutes(30))
                .TransitionTo(States.Confirmed)
        );
    }
}
