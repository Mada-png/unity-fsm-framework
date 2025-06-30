using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStateTransition", menuName = "FSM/PlayerStateTransition", order = 1)]
public class PlayerStateTransition : StateTransitionField<PlayerStateFactory, PlayerInputData, PlayerTrigger>
{

}