using UnityEngine;
using Mada_PNG.FSM.Runtime;
using System.Linq;

public class PlayerFSM : MonoBehaviour
{
    //private BaseStateMachine<PlayerInputData> _stateMachine;
    private PlayerInputData _inputData;

    void Start()
    {
        //_stateMachine = StateMachineBuilder<PlayerInputData>
        //    .Create()
        //    .AddState(new ExampleState())
        //    .AddState(new ExampleState2())
        //    .AddState(new ExampleState3())
        //    .WithTransition<ExampleState, ExampleState2>(() => _inputData.IsRunning)
        //    .WithTransition<ExampleState2, ExampleState3>(() => _inputData.IsJumping)
        //    .BuildStateMachine();


        //var transitionInfo = new[]
        //{
        //    new TransitionInfo<PlayerInputData>(new ExampleState(), new  ExampleState2(), () => _inputData.IsRunning),
        //    new TransitionInfo<PlayerInputData>(new ExampleState2(), new ExampleState3(), () => _inputData.IsJumping)
        //};

        //_stateMachine = new BaseStateMachine<PlayerInputData>(transitionInfo.ToList());
    }

    void Update()
    {
        //_stateMachine.Tick();
    }
}
