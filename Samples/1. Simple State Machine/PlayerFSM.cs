using UnityEngine;
using Mada_PNG.FSM.Runtime;
using System.Collections.Generic;
using System.Linq;

public class PlayerFSM : MonoBehaviour
{
    private StateMachine<PlayerInputData> _stateMachine;
    private PlayerInputData _inputData;

    void Start()
    {
        var transitionInfo = new[]
        {
            TransitionInfo<PlayerInputData>.From<ExampleState, ExampleState2>(_inputData => _inputData.IsRunning),
            TransitionInfo<PlayerInputData>.From<ExampleState2, ExampleState>(_inputData => !_inputData.IsRunning),
            TransitionInfo<PlayerInputData>.From<ExampleState2, ExampleState3>(_inputData => _inputData.IsJumping),
        };

        _stateMachine = new StateMachine<PlayerInputData>(transitionInfo.ToList(), _inputData);
        _stateMachine.InitializeStateMachine(new ExampleState());
    }

    void Update()
    {
        _stateMachine.Tick();

        // Simulate input data for demonstration purposes
        _inputData = new PlayerInputData
        {
            IsRunning = Input.GetKey(KeyCode.LeftShift),
            IsJumping = Input.GetKeyDown(KeyCode.Space)
        };

        _stateMachine.HandleContext(_inputData);
    }
}
