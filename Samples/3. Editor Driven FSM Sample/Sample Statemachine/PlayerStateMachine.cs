using Mada_PNG.FSM.Runtime;

public class PlayerStateMachine : EnumTriggerStateMachine<PlayerInputData, PlayerTrigger>
{
    private PlayerStateFactory _stateFactory;

    private TransitionSet _transitionSet; 
    private TransitionMap<PlayerInputData, PlayerTrigger> _transitionMap;

    public void InitializeAwake()
    {
        _stateFactory = new PlayerStateFactory(this);

        _transitionMap = TransitionMapBuilder.Build<PlayerInputData, PlayerTrigger, PlayerStateFactory>(
            _stateFactory,
            _transitionSet
        );
    }
}
