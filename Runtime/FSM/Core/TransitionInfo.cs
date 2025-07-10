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
        public Func<TContext, bool> Condition { get; }
        public object[]? FromArgs { get; }
        public object[]? ToArgs { get; }

        public TransitionInfo(IState<TContext> fromState, IState<TContext> toState, Func<TContext, bool> condition, object[]? fromArgs = null, object[]? toArgs = null)
        {
            FromState = fromState;
            ToState = toState;
            Condition = condition;
            FromArgs = fromArgs;
            ToArgs = toArgs;
        }

        public static TransitionInfo<TContext> From<TStateFrom, TStateTo>(
            Func<TContext, bool> condition)
            where TStateFrom : IState<TContext>, new()
            where TStateTo : IState<TContext>, new()
        {
            return new TransitionInfo<TContext>(new TStateFrom(), new TStateTo(), condition);
        }
    }
}