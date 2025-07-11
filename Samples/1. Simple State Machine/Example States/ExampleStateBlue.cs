using System;
using UnityEngine;
using Mada_PNG.FSM.Runtime;

public class ExampleStateBlue : IState<PlayerInputData>
{
    private readonly SpriteRenderer _spriteRenderer;

    public ExampleStateBlue(SpriteRenderer spriteRenderer)
    {
        _spriteRenderer = spriteRenderer;
    }

    public void EnterState() 
    {
        _spriteRenderer.color = Color.blue;
    }

    public void Tick() { Debug.Log("Ticking Example State"); }
    public void FixedTick() { Debug.Log("Fixed Ticking Example State"); }
    public void ExitState() { Debug.Log("Exiting Example State"); }
    public void HandleContext(PlayerInputData input) { Debug.Log($"Handling input: {input} in Example State"); }
}
