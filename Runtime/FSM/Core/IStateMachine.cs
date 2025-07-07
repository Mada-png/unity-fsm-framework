using System.Collections.Generic;

namespace Mada_PNG.FSM.Runtime
{
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
         //bool TryPeek<T>(Stack<T> stack, out T result);

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
    public interface IStateMachine<TContext> : IStateMachine
    {
        void PushState(IState<TContext> newState, bool exitPreviousState = true, bool removePreviousState = false);
        void HandleContext(TContext context);
    }
}