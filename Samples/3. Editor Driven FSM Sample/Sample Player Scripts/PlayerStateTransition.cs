using UnityEngine;
using Mada_PNG.FSM.Runtime;

[CreateAssetMenu(fileName = "PlayerStateTransition", menuName = "FSM/PlayerStateTransition", order = 1)]
public class PlayerStateTransition : StateTransitionField<PlayerStateFactory, PlayerInputData, PlayerTrigger>
{

}
