using static NEvo.Sagas.ISagaLogEntry;

namespace NEvo.Sagas;

public interface ISagaCoordinator
{
}

public class SagaCoordinator : ISagaCoordinator
{
    public void Rollback<TKey>(ISaga<TKey> saga)
    {
        foreach (var log in saga.Log.OrderByDescending(l => l.Order))
        {
            switch (log.State)
            {
                case SagaLogEntryState.Error:
                case SagaLogEntryState.New:
                case SagaLogEntryState.Rollbacked:
                    continue;
                case SagaLogEntryState.Commited:
                case SagaLogEntryState.Runned:
                    RollbackLog(log);
                    break;
            }
        }
    }

    public void Commit<TKey>(ISaga<TKey> saga)
    {
        foreach (var log in saga.Log.OrderByDescending(l => l.Order))
        {
            switch (log.State)
            {
                case SagaLogEntryState.Error:
                case SagaLogEntryState.New:
                case SagaLogEntryState.Rollbacked:
                case SagaLogEntryState.Commited:
                    continue;
                case SagaLogEntryState.Runned:
                    CommitLog(log);
                    break;
            }
        }
    }

    private void CommitLog(ISagaLogEntry log)
    {
        if (log.TwoPhaseCommitHandler != null)
        {
            //execute two pahse commit

        }
        log.Commited();
    }

    private void RollbackLog(ISagaLogEntry log)
    {
        if (log.RollbackHandler != null)
        {
            //execute rollback
        }
        log.Rollbacked();
    }
}


public interface ISaga<TKey>
{
    public DateTime Timeout { get; }

    public TKey Id { get; }

    public ICollection<ISagaLogEntry> Log { get; }

    public SagaState State { get; }

    public enum SagaState
    {
        New, Finished, Rollbacking, Rollbacked
    }
}

public interface ISagaLogEntry
{
    public int Order { get; }
    public SagaLogEntryState State { get; }

    public object RollbackHandler { get; }
    public object TwoPhaseCommitHandler { get; }

    void Commited();
    void Rollbacked();

    public enum SagaLogEntryState
    {
        New, Runned, Commited, Error, Rollbacked
    }
}

