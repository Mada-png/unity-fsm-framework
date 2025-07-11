using UnityEngine;

namespace Mada_PNG.FSM.Runtime
{
    public abstract class StateTransitionField<TFactory, TInput, TTrigger> : StateTransition, IStateTransition<TFactory, TInput, TTrigger>
    {
        [Header("Trigger")]
        public TTrigger trigger;

        [Header("State transitions from and next")]
        public StateDefinition<TFactory, TInput> fromStateDefinition;
        public StateDefinition<TFactory, TInput> nextStateDefinition;

        public TTrigger Trigger => trigger;

        public IStateDefinition<TFactory, TInput> FromStateDefinition => fromStateDefinition;

        public IStateDefinition<TFactory, TInput> NextStateDefinition => nextStateDefinition;
    }
}
