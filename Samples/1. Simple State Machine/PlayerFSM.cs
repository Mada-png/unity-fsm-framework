using UnityEngine;
using Mada_PNG.FSM.Runtime;

public class PlayerFSM : MonoBehaviour
{
    private StateMachine<PlayerInputData> _stateMachine;
    private PlayerInputData _inputData;

    void Start()
    {
        var statemachine1 = StateMachineBuilder<PlayerInputData>.Create();
        statemachine1.AddState(new ExampleState());

        var statemachine2 = StateMachineBuilder<PlayerInputData>
            .Create()
            .AddState(new ExampleState())
            .AddState(new ExampleState2())
            .AddState(new ExampleState3())
            .WithTransition<ExampleState, ExampleState2>(() => _inputData.IsRunning)
            .WithTransition<ExampleState2, ExampleState3>(() => _inputData.IsJumping);

        Debug.Log(statemachine1);
        Debug.Log(statemachine2);
    }

    void Update()
    {
        
    }
}
