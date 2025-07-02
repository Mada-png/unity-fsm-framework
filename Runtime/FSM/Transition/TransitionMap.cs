using System.Collections.Generic;
using System;

namespace Mada_PNG.FSM.Runtime
{
    public class TransitionMap<TInput, TTrigger>
    {
        // map of state type and trigger to next-state factory
        private readonly Dictionary<ValueTuple<System.Type, TTrigger>, Func<IState<TInput>>> _map
                 = new Dictionary<ValueTuple<System.Type, TTrigger>, Func<IState<TInput>>>();

        // register a transition from state S on trigger T to factory F
        public void Add<S>(TTrigger trigger, Func<IState<TInput>> nextStateFactory)
            where S : IState<TInput>
        {
            var key = (typeof(S), trigger);
            _map[key] = nextStateFactory;
        }

        public void Add(Type stateType, TTrigger trigger, Func<IState<TInput>> nextStateFactory)
        {
            var key = (stateType, trigger);
            _map[key] = nextStateFactory;
        }

        // try to look up the next-state factory
        public bool TryGetNext(IState<TInput> currentState,
                               TTrigger trigger,
                               out Func<IState<TInput>> nextFactory)
        {
            var key = (currentState.GetType(), trigger);
            return _map.TryGetValue(key, out nextFactory);
        }
    }
}