#nullable enable
using System;

namespace Mada_PNG.FSM.Runtime
{
    public class TransitionInfo
    {
        public IState FromState { get; }
        public IState ToState { get; }
        public Func<bool> Condition { get; }

        public object[]? FromArgs { get; }
        public object[]? ToArgs { get; }

        public TransitionInfo(IState fromState, IState toState, Func<bool> condition, object[]? fromArgs = null, object[]? toArgs = null)
        {
            FromState = fromState;
            ToState = toState;
            Condition = condition;
            FromArgs = fromArgs;
            ToArgs = toArgs;
        }
    }

    public class TransitionInfo<TContext>
    {
        public IState<TContext> FromState { get; }
        public IState<TContext> ToState { get; }
        public Func<bool> Condition { get; }
        public object[]? FromArgs { get; }
        public object[]? ToArgs { get; }

        public TransitionInfo(IState<TContext> fromState, IState<TContext> toState, Func<bool> condition, object[]? fromArgs = null, object[]? toArgs = null)
        {
            FromState = fromState;
            ToState = toState;
            Condition = condition;
            FromArgs = fromArgs;
            ToArgs = toArgs;
        }
    }
}