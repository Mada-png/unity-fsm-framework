using System;
using UnityEngine;
using Mada_PNG.FSM.Runtime;

public class ExampleStateRed : IState<PlayerInputData>
{
    private SpriteRenderer _spriteRenderer;
    private RedStateConfig _config;

    public ExampleStateRed(RedStateConfig config, SpriteRenderer spriteRenderer)
    {
        _spriteRenderer = spriteRenderer;
        _config = config;
    }

    public void EnterState() 
    { 
        _spriteRenderer.color = _config.Color;
    }

    public void Tick() { Debug.Log("Running"); }
    public void FixedTick() { Debug.Log("Fixed Ticking Example State"); }
    public void ExitState() { Debug.Log("Stop running"); }
    public void HandleContext(PlayerInputData input) { Debug.Log($"Handling input: {input} in Example State"); }
}
