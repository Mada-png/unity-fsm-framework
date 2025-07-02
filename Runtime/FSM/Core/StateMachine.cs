using System.Collections.Generic;
using UnityEngine;

namespace Mada_PNG.FSM.Runtime
{
    public abstract class StateMachine<TInput, TTrigger>
    {
        private Stack<IState<TInput>> _stateStack = new Stack<IState<TInput>>();

        private IState<TInput> _initializedState;
        public IState<TInput> CurrentState => _stateStack.Peek();

        public virtual void InitializeAwake(TInput input){ }


        /// <summary>
        /// Add a new state to the top of the stack 
        /// </summary>
        /// <param name="newState">New state to be pushed up</param>
        public void PushState(IState<TInput> newState, bool removePreviousState)
        {
            TryPeek<IState<TInput>>(_stateStack, out IState<TInput> topItem);
            topItem?.ExitState();

            if (removePreviousState)
            {
                _stateStack.Pop();
            }

            _stateStack.Push(newState);
            newState.EnterState();
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

        public void HandleInput(TInput input)
        {
            _stateStack.Peek()?.HandleInput(input);
        }

        public void DebugPrintStack()
        {
            foreach (IState<TInput> state in _stateStack)
            {
                Debug.Log(state.GetType().Name);
            }
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

        public virtual void FireTrigger(TTrigger trigger)
        {
        }
    }

}
