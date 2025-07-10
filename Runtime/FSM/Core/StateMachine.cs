using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mada_PNG.FSM.Runtime
{
    /// <summary>
    /// Represents the core functionality of a state machine that manages a stack of states.
    /// </summary>
    /// <remarks>This abstract class provides the foundational operations for managing states in a stack-based
    /// state machine. It supports pushing, popping, and transitioning between states, as well as invoking
    /// state-specific behaviors such as ticking and drawing gizmos. Derived classes can extend or customize the
    /// behavior of the state machine.</remarks>
    /// <typeparam name="TState">The type of state managed by the state machine. Must implement the <see cref="IState"/> interface.</typeparam>
    public abstract class StateMachineCore<TState> : IStateMachine
        where TState : IState
    {
        protected Stack<TState> _stateStack = new();
        protected TState _initializedState;
        public TState CurrentState => _stateStack.Peek();
        protected TState _currentState;

        protected List<TransitionInfo> _transitionInfo = new();

        protected StateMachineCore()
        {
        }

        protected StateMachineCore(IEnumerable<TransitionInfo> transition)
        {
            var list = transition.ToList();
            _transitionInfo = list;
        }

        public void InitializeStateMachine(TState initialState)
        {
            _initializedState = initialState;
            PushState(_initializedState, false, false);
        }

        public virtual void InitializeStateMachine(int index)
        {
            _initializedState = (TState)_transitionInfo[index].FromState;
            PushState(_initializedState, false, false);
        }

        public void Tick()
        {
            CurrentStateStack().Tick();
        }

        public void FixedTick()
        {
            CurrentStateStack().FixedTick();
        }

        /// <summary>
        /// Add a new state to the top of the stack 
        /// </summary>
        /// <param name="newState">New state to be pushed up</param>
        public void PushState(TState newState, bool exitPreviousState = true, bool removePreviousState = false)
        {
            if (newState == null)
                throw new ArgumentNullException(nameof(newState), "New state cannot be null");

            if (exitPreviousState && TryPeek(_stateStack, out TState topItem))
                topItem?.ExitState();

            if (removePreviousState && _stateStack.Count > 0)
                _stateStack.Pop();

            _stateStack.Push(newState);
            newState.EnterState();
        }

        // Implementation for IStateMachine.PushState
        void IStateMachine.PushState(IState newState, bool exitPreviousState, bool removePreviousState)
        {
            if (newState is TState typedState)
            {
                PushState(typedState, exitPreviousState, removePreviousState);
            }
            else
            {
                throw new ArgumentException($"Invalid state type. Expected {typeof(TState).Name}, but got {newState.GetType().Name}.");
            }
        }

        public void PopState()
        {
            var popped = _stateStack.Pop();
            popped.ExitState();

            if (_stateStack.Count == 0)
                _stateStack.Push(_initializedState);

            _stateStack.Peek()?.EnterState();
        }

        public bool TryPeek<T>(Stack<T> stack, out T result)
        {
            if (stack.Count > 0)
            {
                result = stack.Peek();
                return true;
            }

            result = default;
            return false;
        }

        public TState CurrentStateStack()
        {
            if (_stateStack.Count > 0)
            {
                return _stateStack.Peek();
            }
            else
            {
                throw new InvalidOperationException("State stack is empty. Did you forget to initialize the State Machine with InitializeStateMachine()?");
            }
        }

        /// <summary>
        /// Prints the names of the types of all states currently in the stack to the debug log.
        /// </summary>
        /// <remarks>This method iterates through the stack of states and logs the name of each state's
        /// type. It is intended for debugging purposes to provide visibility into the current state stack.</remarks>
        public virtual void DebugPrintStack()
        {
            foreach (IState state in _stateStack)
            {
                Debug.Log(state.GetType().Name);
            }
        }

        /// <summary>
        /// Draws gizmos for the current states in the state stack.
        /// </summary>
        /// <remarks>This method iterates through the state stack and invokes the <see
        /// cref="IStateGizmos.DrawGizmos"/> method  for any state that implements the <see cref="IStateGizmos"/>
        /// interface. It is typically used to visualize  state-specific information in the editor during
        /// development.</remarks>
        public virtual void DrawGizmos()
        {
            foreach (IState state in _stateStack)
            {
                if (state is IStateGizmos gizmoState)
                {
                    gizmoState.DrawGizmos();
                }
            }
        }
    }

    /// <summary>
    /// A state machine that manages a stack of states with context-specific behavior.
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    /// <typeparam name="TContext"></typeparam>
    public abstract class StateMachineCore<TState, TContext> : StateMachineCore<TState>, IStateMachine<TContext>
        where TState : IState<TContext>
    {
        private readonly List<TransitionInfo<TContext>> _typedTransitions;

        protected StateMachineCore(IEnumerable<TransitionInfo<TContext>> transition)
        {
            _typedTransitions = transition.ToList();
        }

        public override void InitializeStateMachine(int index)
        {
            _initializedState = (TState)_typedTransitions[index].FromState;
            PushState(_initializedState, false, false);
        }

        public void HandleContext(TContext context)
        {
            CurrentStateStack()?.HandleContext(context);
            HandleConditionalContext(context);
        }

        public void HandleConditionalContext(TContext context)
        {
            foreach (TransitionInfo<TContext> transition in _typedTransitions)
            {
                if (transition.FromState.GetType() == CurrentState.GetType() && transition.Condition(context))
                {
                    PushState((TState)transition.ToState, true, false);
                    return;
                }
            }
        }

        void IStateMachine<TContext>.PushState(IState<TContext> newState, bool exitPreviousState, bool remove)
            => base.PushState((TState)newState, exitPreviousState, remove);
    }

    public class StateMachine : StateMachineCore<IState>
    {
        public StateMachine(IEnumerable<TransitionInfo> transition) : base(transition)
        {
        }
    }

    public class StateMachine<TContext> : StateMachineCore<IState<TContext>, TContext>
    {
        public StateMachine(IEnumerable<TransitionInfo<TContext>> transition, TContext context) : base(transition)
        {
            _transitionInfo = transition.Select(t => new TransitionInfo((IState)t.FromState, (IState)t.ToState, () => t.Condition(context))).ToList();
        }
    }

    public abstract class EnumTriggerStateMachine<TContext, TTrigger> : StateMachineCore<IState<TContext>, TContext>
    {
        protected EnumTriggerStateMachine(IEnumerable<TransitionInfo<TContext>> transition, int initial = 0) : base(transition)
        {
        }
    }
}