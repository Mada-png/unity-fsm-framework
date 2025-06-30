using UnityEngine;

public class PlayerStateFactory : StateFactory<PlayerStateMachine>
{
    public PlayerStateFactory(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        StateMachine = stateMachine;
    }

    public PlayerStateIdle IdleState() => new PlayerStateIdle();
    public PlayerStateMove MoveState() => new PlayerStateMove();
    public PlayerStateTurn TurnState() => new PlayerStateTurn();
    public PlayerStateStop StopState() => new PlayerStateStop();
}
