using NEvo.Core;
using NEvo.Core.StateManaging;
using NEvo.Monads;

namespace NEvo.Sagas.Stateful
{
    public interface IStateMapper<TState, T>
    {
        public Maybe<T> MapToState(TState state);
        public TState MapFromState(T state);
    }

    public class StringStateMapper<TSaga> : IStateMapper<string, IState<TSaga>>
    {
        private readonly Dictionary<string, IState<TSaga>> _states;

        public StringStateMapper(IEnumerable<IState<TSaga>> states)
        {
            _states = Check.Null(states).ToDictionary(k => k.Name);
        }

        public string MapFromState(IState<TSaga> state) => state.Name;

        public Maybe<IState<TSaga>> MapToState(string stateName) => _states.TryGetValue(stateName, out var state) ? Maybe.Some(state) : Maybe.None;
    }
}