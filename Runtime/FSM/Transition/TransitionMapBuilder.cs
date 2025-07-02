using System;
using System.Linq;
using UnityEngine;

namespace Mada_PNG.FSM.Runtime
{
    public static class TransitionMapBuilder
    {
        public static TransitionMap<TInput, TTrigger> Build<TInput, TTrigger, TFactory>(
            TFactory factory,
            TransitionSet transitionSet
            )
        {
            var map = new TransitionMap<TInput, TTrigger>();

            foreach (var transition in transitionSet.Transitions.OfType<IStateTransition<TFactory, TInput, TTrigger>>())
            {
                map.Add(transition.FromStateDefinition.StateType, transition.Trigger, () => transition.NextStateDefinition.CreateState(factory));
            }

            return map; 
        }
    }
}  