using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/Transition Set")]
public class TransitionSet : ScriptableObject
{
    public StateTransition[] Transitions;
}
