using UnityEngine;
using Mada_PNG.FSM.Runtime;
using System.Linq;

public class PlayerFSM : MonoBehaviour
{
    private StateMachine<PlayerInputData> _stateMachine;
    private PlayerInputData _inputData;
    public SpriteRenderer SpriteRenderer;

    void Start()
    {
        var stateGreen = new ExampleStateGreen(SpriteRenderer);
        var stateRed = new ExampleStateRed(SpriteRenderer);
        var stateBlue = new ExampleStateBlue(SpriteRenderer);

        var transitionInfo = new[]
        {
            new TransitionInfo<PlayerInputData>(stateGreen, stateRed, _inputData => _inputData.IsKeyR),
            new TransitionInfo<PlayerInputData>(stateGreen, stateBlue, _inputData => _inputData.IsKeyB),

            new TransitionInfo<PlayerInputData>(stateRed, stateBlue, _inputData => _inputData.IsKeyB),
            new TransitionInfo<PlayerInputData>(stateRed, stateGreen, _inputData => _inputData.IsKeyG, toArgs: new object[] { this.gameObject }),

            new TransitionInfo<PlayerInputData>(stateBlue, stateGreen, _inputData => _inputData.IsKeyG),
            new TransitionInfo<PlayerInputData>(stateBlue, stateRed, _inputData => _inputData.IsKeyR),
        };

        _stateMachine = new StateMachine<PlayerInputData>(transitionInfo.ToList(), _inputData);
        _stateMachine.InitializeStateMachine(stateGreen);
    }

    void Update()
    {
        _stateMachine.Tick();

        // Simulate input data for demonstration purposes
        _inputData = new PlayerInputData
        {
            IsKeyR = Input.GetKeyDown(KeyCode.R),
            IsKeyB = Input.GetKeyDown(KeyCode.B),
            IsKeyG = Input.GetKeyDown(KeyCode.G),
        };

        _stateMachine.HandleContext(_inputData);
    }
}
