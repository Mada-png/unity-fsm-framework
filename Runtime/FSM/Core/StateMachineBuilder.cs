using System;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace Mada_PNG.FSM.Runtime
{
    public class StateMachineBuilder
    {
        protected readonly Dictionary<Type, IState> _states = new();
    }

    public class StateMachineBuilder<TContext> : StateMachineBuilder
    {
        public static StateMachineBuilder<TContext> Create() => new();
        private readonly new Dictionary<Type, IState<TContext>> _states = new();
        protected readonly List<TransitionInfo<TContext>> _transitionInfo = new();
        private BaseStateMachine<TContext> _stateMachine;

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

            _states.TryGetValue(typeof(IFromState), out var fromState);
            _states.TryGetValue(typeof(IToState), out var toState);

            _transitionInfo.Add(new TransitionInfo<TContext>(fromState, toState, condition));
            
            return this;
        }

        public BaseStateMachine<TContext> BuildStateMachine()
        {
            if (_states.Count == 0)
                throw new InvalidOperationException("No states have been added to the state machine.");

            _stateMachine = new BaseStateMachine<TContext>(_transitionInfo);

            return _stateMachine;
        }

        public StateMachineBuilder<TContext> SetInitialState<TState>(Type state) where TState : IState<TContext>
        {
            if (!_states.ContainsKey(typeof(TState)))
                throw new ArgumentException($"Initial state {typeof(TState).Name} is not registered.");

            if (_stateMachine == null)
                throw new InvalidOperationException("State machine has not been built yet. Call BuildStateMachine() first.");

            if (_states.TryGetValue(state, out var initialState))
            {
                if (initialState == null)
                    throw new InvalidOperationException($"State {state.Name} is not registered.");
            }
            else
            {
                throw new InvalidOperationException($"State {state.Name} is not registered.");
            }

            return this;
        }
    }
}