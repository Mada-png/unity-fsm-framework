using System;
using System.Collections.Generic;
using UnityEditorInternal;

namespace Mada_PNG.FSM.Runtime
{
    public class StateMachineBuilder
    {
        protected readonly Dictionary<Type, IState> _states = new();
        protected readonly List<TransitionInfo> _transitionInfo = new();
    }

    public class StateMachineBuilder<TContext> : StateMachineBuilder
    {
        private readonly new Dictionary<Type, IState<TContext>> _states = new();

        public static StateMachineBuilder<TContext> Create() => new();

        public StateMachineBuilder<TContext> AddState<TState>(TState state) where TState : IState<TContext>
        {
            if (state == null)
                throw new ArgumentNullException(nameof(state), "State cannot be null");

            _states[typeof(TState)] = state;

            return this;
        }

        public StateMachineBuilder<TContext> WithTransition<IFromState, IToState>(Func<bool> condition)
            where IFromState : IState<TContext>
            where IToState : IState<TContext>
        {
            if (!_states.ContainsKey(typeof(IFromState)))
                throw new ArgumentException($"From state {typeof(IFromState).Name} is not registered.");

            if (!_states.ContainsKey(typeof(IToState)))
                throw new ArgumentException($"To state {typeof(IToState).Name} is not registered.");

            _transitionInfo.Add(new TransitionInfo(typeof(IFromState), typeof(IToState), condition));
            
            return this;
        }

        public StateMachine<TContext> Build()
        {
            if (_states.Count == 0)
                throw new InvalidOperationException("No states have been added to the state machine.");

            var stateMachine = new StateMachine<TContext>(_transitionInfo);


            return new StateMachine<TContext>(_transitionInfo);
        }

        public StateMachine<TContext> BuildAndInitialize(List<IState<TContext>> states, TransitionInfo transition)
        {
            return new StateMachine<TContext>();
        }
    }
}