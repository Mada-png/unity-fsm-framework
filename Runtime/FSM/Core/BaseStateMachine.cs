using GluonGui.Dialog;
using Mada_PNG.FSM.Runtime;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;
using static Codice.CM.WorkspaceServer.WorkspaceTreeDataStore;

namespace Mada_PNG.FSM.Runtime
{
    public abstract class BaseStateMachine<TState> : IStateMachine where TState : IState
    {
        protected Stack<TState> _stateStack = new();
        protected TState _initializedState;
        protected List<TransitionInfo> _transitionInfo = new();

        public TState CurrentState => _stateStack.Peek();

        public virtual void Tick()
        {
            _stateStack.Peek()?.Tick();
        }

        public virtual void FixedTick()
        {
            _stateStack.Peek()?.FixedTick();
        }

        public virtual void PopState()
        {
            var popped = _stateStack.Pop();
            popped.ExitState();

            if (_stateStack.Count == 0)
                _stateStack.Push(_initializedState);

            _stateStack.Peek()?.EnterState();
        }

        public virtual bool TryPeek<T>(Stack<T> stack, out T result)
        {
            if (stack.Count > 0)
            {
                result = stack.Peek();
                return true;
            }

            result = default;
            return false;
        }

        public virtual void DebugPrintStack()
        {
            foreach (IState state in _stateStack)
            {
                Debug.Log(state.GetType().Name);
            }
        }

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

        public virtual void PushState(IState newState, bool exitPreviousState = true, bool removePreviousState = false)
        {
            if (newState == null)
            {
                throw new ArgumentNullException(nameof(newState), "New state cannot be null");
            }

            if (exitPreviousState)
            {
                TryPeek(_stateStack, out TState topItem);
                topItem?.ExitState();
            }

            if (removePreviousState)
            {
                _stateStack.Pop();
            }

            _stateStack.Push((TState)newState);
            newState.EnterState();
        }
    }

    public abstract class BaseStateMachine<TState, TContext> : IStateMachine where TState : IState<TContext>
    {
        protected Stack<TState> _stateStack = new();
        protected TState _initializedState;
        protected List<TransitionInfo<TContext>> _transitionInfo = new();

        public TState CurrentState => _stateStack.Peek();

        public virtual void Tick()
        {
            _stateStack.Peek()?.Tick();
        }

        public virtual void FixedTick()
        {
            _stateStack.Peek()?.FixedTick();
        }

        public virtual void PopState()
        {
            var popped = _stateStack.Pop();
            popped.ExitState();

            if (_stateStack.Count == 0)
                _stateStack.Push(_initializedState);

            _stateStack.Peek()?.EnterState();
        }

        public virtual bool TryPeek<T>(Stack<T> stack, out T result)
        {
            if (stack.Count > 0)
            {
                result = stack.Peek();
                return true;
            }

            result = default;
            return false;
        }

        public virtual void DebugPrintStack()
        {
            foreach (IState state in _stateStack)
            {
                Debug.Log(state.GetType().Name);
            }
        }

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

        public virtual void PushState(IState newState, bool exitPreviousState = true, bool removePreviousState = false)
        {
            if (newState == null)
            {
                throw new ArgumentNullException(nameof(newState), "New state cannot be null");
            }

            if (exitPreviousState)
            {
                TryPeek(_stateStack, out TState topItem);
                topItem?.ExitState();
            }

            if (removePreviousState)
            {
                _stateStack.Pop();
            }

            _stateStack.Push((TState)newState);
            newState.EnterState();
        }
    }

    public class StateMachine : BaseStateMachine<IState>
    {
        public StateMachine(List<TransitionInfo> transitionInfo)
        {
            if (transitionInfo == null || transitionInfo.Count == 0)
            {
                throw new ArgumentNullException(nameof(transitionInfo), "Transition info cannot be null or empty");
            }
            _transitionInfo = transitionInfo;
            _initializedState = (IState)transitionInfo[0].FromState;
            _stateStack.Push(_initializedState);
        }
        public StateMachine(List<TransitionInfo> transitionInfo, int initialTransition)
        {
            if (transitionInfo == null || transitionInfo.Count == 0)
            {
                throw new ArgumentNullException(nameof(transitionInfo), "Transition info cannot be null or empty");
            }
            _transitionInfo = transitionInfo;
            _initializedState = (IState)transitionInfo[initialTransition].FromState;
            _stateStack.Push(_initializedState);
        }
    }

    public class  StateMachine<TContext> : BaseStateMachine<IState<TContext>, TContext>
    {
        public StateMachine(List<TransitionInfo<TContext>> transitionInfo)
        {
            if (transitionInfo == null || transitionInfo.Count == 0)
            {
                throw new ArgumentNullException(nameof(transitionInfo), "Transition info cannot be null or empty");
            }
            _transitionInfo = transitionInfo;
            _initializedState = transitionInfo[0].FromState;
            _stateStack.Push(_initializedState);
        }

        public StateMachine(List<TransitionInfo<TContext>> transitionInfo, int initialTransition)
        {
            if (transitionInfo == null || transitionInfo.Count == 0)
            {
                throw new ArgumentNullException(nameof(transitionInfo), "Transition info cannot be null or empty");
            }
            _transitionInfo = transitionInfo;
            _initializedState = transitionInfo[initialTransition].FromState;
            _stateStack.Push(_initializedState);
        }
    }




    //public class BaseStateMachine<TContext> : BaseStateMachine
    //{
    //    public new IState<TContext> CurrentState => _stateStack.Peek();
    //    private IState<TContext> _initializedState;
    //    private Stack<IState<TContext>> _stateStack = new Stack<IState<TContext>>();
    //    private readonly Dictionary<Type, IState<TContext>> _states = new();

    //    protected new List<TransitionInfo<TContext>> _transitionInfo = new();

    //    public BaseStateMachine()
    //    {
    //    }

    //    /// <summary>
    //    /// No initialization, just a constructor.
    //    /// Sets the first state from the transition info as the initial state.
    //    /// </summary>
    //    /// <param name="transitionInfo"></param>
    //    /// <exception cref="ArgumentNullException"></exception>
    //    public BaseStateMachine(List<TransitionInfo<TContext>> transitionInfo)
    //    {
    //        if (transitionInfo == null || transitionInfo.Count == 0)
    //        {
    //            throw new ArgumentNullException(nameof(transitionInfo), "Transition info cannot be null or empty");
    //        }

    //        _transitionInfo = transitionInfo;
    //        _initializedState = transitionInfo[0].FromState;

    //        _stateStack.Push(_initializedState);

    //        _stateStack.TryPeek(out IState<TContext> topItem);
    //        Debug.Log(topItem);
    //    }

    //    public BaseStateMachine(List<TransitionInfo<TContext>> transitionInfo, int initialTransition)
    //    {
    //        if (transitionInfo == null || transitionInfo.Count == 0)
    //        {
    //            throw new ArgumentNullException(nameof(transitionInfo), "Transition info cannot be null or empty");
    //        }

    //        _transitionInfo = transitionInfo;
    //        _initializedState = transitionInfo[initialTransition].FromState;
    //        _stateStack.Push(_initializedState);
    //    }

    //    public void InitializeAwake(TContext input) { }

    //    /// <summary>
    //    /// Add a new state to the top of the stack 
    //    /// </summary>
    //    /// <param name="newState">New state to be pushed up</param>
    //    public void PushState(IState<TContext> newState, bool removePreviousState)
    //    {
    //        TryPeek<IState<TContext>>(_stateStack, out IState<TContext> topItem);
    //        topItem?.ExitState();

    //        if (removePreviousState && topItem != null)
    //        {
    //            _stateStack.Pop();
    //        }

    //        _stateStack.Push(newState);
    //        newState.EnterState();
    //    }

    //    public void HandleInput(TContext input)
    //    {
    //        _stateStack.Peek()?.HandleContext(input);
    //    }

    //    /// <summary>
    //    /// Prints the names of the types of all states currently in the stack to the debug log.
    //    /// </summary>
    //    /// <remarks>This method iterates through the stack of states and logs the name of each state's
    //    /// type. It is intended for debugging purposes to provide visibility into the current state stack.</remarks>
    //    public new void DebugPrintStack()
    //    {
    //        foreach (IState<TContext> state in _stateStack)
    //        {
    //            Debug.Log(state.GetType().Name);
    //        }
    //    }

    //    /// <summary>
    //    /// Draws gizmos for the current states in the state stack.
    //    /// </summary>
    //    /// <remarks>This method iterates through the state stack and invokes the <see
    //    /// cref="IStateGizmos.DrawGizmos"/> method  for any state that implements the <see cref="IStateGizmos"/>
    //    /// interface. It is typically used to visualize  state-specific information in the editor during
    //    /// development.</remarks>
    //    public new void DrawGizmos()
    //    {
    //        foreach (IState<TContext> state in _stateStack)
    //        {
    //            if (state is IStateGizmos gizmoState)
    //            {
    //                gizmoState.DrawGizmos();
    //            }
    //        }
    //    }
    //}

    //public class EnumTriggerStateMachine<TInput, TTrigger> : BaseStateMachine<TInput>
    //{
    //}



    public interface IStateMachine
    {
        /// <summary>
        /// Advances the state of the system or process by one unit of time or iteration.
        /// </summary>
        /// <remarks>This method is typically called in a loop or at regular intervals to update the system's
        /// state. Ensure that any required preconditions for the system's state are met before calling this
        /// method.</remarks>
        void Tick();

        /// <summary>
        /// Executes logic that should run at fixed time intervals, typically used for physics updates or other
        /// time-sensitive operations.
        /// </summary>
        /// <remarks>This method is intended to be called at consistent intervals, such as during a fixed update
        /// loop in a game engine or simulation.  Ensure that the calling code invokes this method at regular intervals to
        /// maintain predictable behavior.</remarks>
        void FixedTick();

        /// <summary>
        /// Transitions to a new state in the state management system.
        /// </summary>
        /// <remarks>This method allows for flexible state transitions by optionally exiting or removing the previous
        /// state. Ensure that <paramref name="newState"/> is not null to avoid runtime errors.</remarks>
        /// <param name="newState">The new state to transition to. Cannot be null.</param>
        /// <param name="exitPreviousState">Indicates whether the previous state should be exited before transitioning to the new state. If <see
        /// langword="true"/>, the previous state's exit logic will be executed.</param>
        /// <param name="removePreviousState">Indicates whether the previous state should be removed from the state stack. If <see langword="true"/>, the previous
        /// state will be discarded.</param>
        void PushState(IState newState, bool exitPreviousState = true, bool removePreviousState = false);

        /// <summary>
        /// Removes the most recently added state from the stack.
        /// </summary>
        /// <remarks>This method is typically used to revert to the previous state by discarding the current
        /// state.  If the stack is empty, calling this method may result in an exception or undefined behavior,  depending
        /// on the implementation. Ensure the stack is not empty before calling this method.</remarks>
        void PopState();

        /// <summary>
        /// Attempts to retrieve the object at the top of the specified stack without removing it.
        /// </summary>
        /// <typeparam name="T">The type of elements contained in the stack.</typeparam>
        /// <param name="stack">The stack from which to retrieve the top object. Cannot be null.</param>
        /// <param name="result">When this method returns, contains the object at the top of the stack if the operation succeeds;  otherwise, the
        /// default value of <typeparamref name="T"/>. This parameter is passed uninitialized.</param>
        /// <returns><see langword="true"/> if the stack is not empty and the top object was successfully retrieved;  otherwise, <see
        /// langword="false"/>.</returns>
        bool TryPeek<T>(Stack<T> stack, out T result);

        /// <summary>
        /// Prints the names of the types of all states currently in the stack to the debug log.
        /// </summary>
        /// <remarks>This method iterates through the stack of states and logs the name of each state's
        /// type. It is intended for debugging purposes to provide visibility into the current state stack.</remarks>
        void DebugPrintStack();

        /// <summary>
        /// Draws gizmos for the current states in the state stack.
        /// </summary>
        /// <remarks>This method iterates through the state stack and invokes the <see
        /// cref="IStateGizmos.DrawGizmos"/> method  for any state that implements the <see cref="IStateGizmos"/>
        /// interface. It is typically used to visualize  state-specific information in the editor during
        /// development.</remarks>
        void DrawGizmos();
    }
}