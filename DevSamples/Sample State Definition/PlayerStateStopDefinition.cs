using System;
using UnityEngine;
using Mada_PNG.FSM.Runtime;

[CreateAssetMenu(fileName = "PlayerStateStopDefinition", menuName = "FSM/PlayerState/Stop")]
public class PlayerStateStopDefinition : PlayerStateDefinition
{
    public override Type StateType => typeof(PlayerStateStop);

    public override IState<PlayerInputData> CreateState(PlayerStateFactory factory)
    {
        throw new NotImplementedException();
    }
}
