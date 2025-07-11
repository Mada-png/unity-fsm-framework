using System;
using UnityEngine;
using Mada_PNG.FSM.Runtime;


public class ExampleState : IState<PlayerInputData>
{
    public void EnterState() { Debug.Log("Standing Still"); }
    public void Tick() { Debug.Log("Standing"); }
    public void FixedTick() { Debug.Log("Fixed Ticking Example State"); }
    public void ExitState() { Debug.Log("Do something"); }
    public void HandleContext(PlayerInputData input) { Debug.Log($"Handling input: {input} in Example State"); }
}
