namespace Mada_PNG.FSM.Runtime
{
    public interface IState
    {
        /// <summary>
        /// Enter logic, runs only once
        /// </summary>
        void EnterState();
        /// <summary>
        /// FPS Updates
        /// </summary>
        void Tick();
        /// <summary>
        /// Physics Updates
        /// </summary>
        void FixedTick();
        /// <summary>
        /// Exit logic, runs only once
        /// </summary>
        void ExitState();
    }

    public interface IState<TContext> : IState
    {
        void HandleContext(TContext context);
    }
}
