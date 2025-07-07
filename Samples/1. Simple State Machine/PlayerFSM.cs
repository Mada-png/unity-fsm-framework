using UnityEngine;
using Mada_PNG.FSM.Runtime;
using System.Collections.Generic;
using System.Linq;

public class PlayerFSM : MonoBehaviour
{
    private StateMachine<PlayerInputData> _stateMachine;
    private PlayerInputData _inputData;
    //private List<TransitionInfo<PlayerInputData>> _transitionInfo;
    void Start()
    {
        var transitionInfo = new[]
        {
            new TransitionInfo<PlayerInputData>(new ExampleState(), new  ExampleState2(), () => _inputData.IsRunning),
            new TransitionInfo<PlayerInputData>(new ExampleState2(), new ExampleState(), () => !_inputData.IsRunning),
            new TransitionInfo<PlayerInputData>(new ExampleState2(), new ExampleState3(), () => _inputData.IsJumping)
        };

        _stateMachine = new StateMachine<PlayerInputData>(transitionInfo.ToList());
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

        if (Input.GetKey(KeyCode.LeftShift))
        {
            Debug.Log("Button");
        }
    }
}
