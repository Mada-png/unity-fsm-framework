using System;
using UnityEngine;
using Mada_PNG.FSM.Runtime;

public class ExampleState3 : IState<PlayerInputData>
{
    public void EnterState() { Debug.Log("Entering Example State"); }
    public void Tick() { Debug.Log("Ticking Example State"); }
    public void FixedTick() { Debug.Log("Fixed Ticking Example State"); }
    public void ExitState() { Debug.Log("Exiting Example State"); }
    public void HandleContext(PlayerInputData input) { Debug.Log($"Handling input: {input} in Example State"); }
}