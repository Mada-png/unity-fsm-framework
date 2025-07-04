using System;

namespace Mada_PNG.FSM.Runtime
{
    public class TransitionInfo
    {
        public Type FromState { get; }
        public Type ToState { get; }
        public Func<bool> Condition { get; }

        public TransitionInfo(Type fromState, Type toState, Func<bool> condition)
        {
            FromState = fromState;
            ToState = toState;
            Condition = condition;
        }
    }
}