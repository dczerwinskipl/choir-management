namespace NEvo.Sagas.Stateful
{
    public interface IStatefulSaga<TState> : ISaga
    {
        public TState State { get; set; }
    }
}
