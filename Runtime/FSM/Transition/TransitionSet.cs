using UnityEngine;

namespace Mada_PNG.FSM.Runtime
{
    [CreateAssetMenu(menuName = "StateMachine/Transition Set")]
    public class TransitionSet : ScriptableObject
    {
        public StateTransition[] Transitions;
    }
}
