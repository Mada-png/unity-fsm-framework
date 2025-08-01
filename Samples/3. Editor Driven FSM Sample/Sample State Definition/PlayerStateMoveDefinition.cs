using System;
using UnityEngine;
using Mada_PNG.FSM.Runtime;

[CreateAssetMenu(fileName = "PlayerStateMoveDefinition", menuName = "FSM/PlayerState/Move")]
public class PlayerStateMoveDefinition : PlayerStateDefinition
{
    public override Type StateType => typeof(PlayerStateMove);

    public override IState<PlayerInputData> CreateState(PlayerStateFactory factory)
    {
        return factory.MoveState();
    }
}
