using System;
using UnityEngine;
using Mada_PNG.FSM.Runtime;

public class ExampleStateRed : IState<PlayerInputData>
{
    private SpriteRenderer _spriteRenderer;
    public ExampleStateRed(SpriteRenderer spriteRenderer)
    {
        _spriteRenderer = spriteRenderer;
    }

    public void EnterState() 
    { 
        _spriteRenderer.color = Color.red;
    }

    public void Tick() { Debug.Log("Running"); }
    public void FixedTick() { Debug.Log("Fixed Ticking Example State"); }
    public void ExitState() { Debug.Log("Stop running"); }
    public void HandleContext(PlayerInputData input) { Debug.Log($"Handling input: {input} in Example State"); }
}
