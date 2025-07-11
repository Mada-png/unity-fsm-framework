using System;
using UnityEngine;
using Mada_PNG.FSM.Runtime;

[CreateAssetMenu(fileName = "PlayerStateTurnDefinition", menuName = "FSM/PlayerState/Turn")]
public class PlayerStateTurningDefinition : PlayerStateDefinition
{
    public override Type StateType => typeof(PlayerStateTurn);

    public override IState<PlayerInputData> CreateState(PlayerStateFactory factory)
    {
        return factory.TurnState();
    }
}
