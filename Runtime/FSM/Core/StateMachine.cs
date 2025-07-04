using GluonGui.Dialog;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mada_PNG.FSM.Runtime
{
    public class StateMachine
    {
        public IState CurrentState => _stateStack.Peek();
        private IState _initializedState;

        protected List<TransitionInfo> _transitionInfo = new();
        private Stack<IState> _stateStack = new Stack<IState>();

        public StateMachine()
        {
        }

        /// <summary>
        /// FPS Update
        /// </summary>
        public void Tick()
        {
            _stateStack.Peek()?.Tick();
        }

        /// <summary>
        /// Physics Update
        /// </summary>
        public void FixedTick()
        {
            _stateStack.Peek()?.FixedTick();
        }

        /// <summary>
        /// Remove the state on top of the stack
        /// </summary>
        /// <param name="state"></param>
        public void PopState()
        {
            var popped = _stateStack.Pop();
            popped.ExitState();

            if (_stateStack.Count == 0)
                _stateStack.Push(_initializedState);

            _stateStack.Peek()?.EnterState();
        }

        public static bool TryPeek<T>(Stack<T> stack, out T result)
        {
            if (stack.Count > 0)
            {
                result = stack.Peek();
                return true;
            }

            result = default;
            return false;
        }
    }

    public class StateMachine<TInput> : StateMachine
    {
        private IState<TInput> _initializedState;
        private Stack<IState<TInput>> _stateStack = new Stack<IState<TInput>>();

        public StateMachine()
        {
        }

        public StateMachine(List<TransitionInfo> transitionInfo)
        {
            _transitionInfo = transitionInfo ?? throw new ArgumentNullException(nameof(transitionInfo), "Transition info cannot be null");
        }

        public void InitializeAwake(TInput input) { }

        /// <summary>
        /// Add a new state to the top of the stack 
        /// </summary>
        /// <param name="newState">New state to be pushed up</param>
        public void PushState(IState<TInput> newState, bool removePreviousState)
        {
            TryPeek<IState<TInput>>(_stateStack, out IState<TInput> topItem);
            topItem?.ExitState();

            if (removePreviousState && topItem != null)
            {
                _stateStack.Pop();
            }

            _stateStack.Push(newState);
            newState.EnterState();
        }

        public void HandleInput(TInput input)
        {
            _stateStack.Peek()?.HandleContext(input);
        }

        /// <summary>
        /// Prints the names of the types of all states currently in the stack to the debug log.
        /// </summary>
        /// <remarks>This method iterates through the stack of states and logs the name of each state's
        /// type. It is intended for debugging purposes to provide visibility into the current state stack.</remarks>
        public void DebugPrintStack()
        {
            foreach (IState<TInput> state in _stateStack)
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
        public void DrawGizmos()
        {
            foreach (IState<TInput> state in _stateStack)
            {
                if (state is IStateGizmos gizmoState)
                {
                    gizmoState.DrawGizmos();
                }
            }
        }
    }

    public class EnumTriggerStateMachine<TInput, TTrigger> : StateMachine<TInput>
    {
        public virtual void FireTrigger(TTrigger trigger)
        {
        }
    }
}