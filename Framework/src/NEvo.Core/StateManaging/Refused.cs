namespace NEvo.Core.StateManaging;

public record Refused(string Reason)
{
    public record TransitionNotFound(string Transition, string? State, string Reason) : Refused(Reason);
    public record TransactionValidationFailure(string Transition, string State, string ValidationState) : Refused(ValidationState);
    public record StateError(Exception Error) : Refused(Error.Message);
}

public class TransitionRefusedException : Exception
{
    public Refused Refused { get; }

    public TransitionRefusedException(Refused refused) : base(refused.Reason)
    {
        Refused = refused;
    }

}