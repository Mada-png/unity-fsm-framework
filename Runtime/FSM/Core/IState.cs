namespace Mada_PNG.FSM.Runtime
{
    public interface IState<TInput>
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
        void HandleInput(TInput input);
    }
}