using System;
using UnityEngine;
using Mada_PNG.FSM.Runtime;


public class  ExampleStateGreen : IState<PlayerInputData>
{
    private SpriteRenderer _spriteRenderer;
    private GameObject _sampleObject;

    public ExampleStateGreen(SpriteRenderer spriteRenderer)
    {
        _spriteRenderer = spriteRenderer;
    }

    public void EnterState()
    {
        _spriteRenderer.color = Color.green;
    }

    public void Tick() { Debug.Log("Standing"); }
    public void FixedTick() { Debug.Log("Fixed Ticking Example State"); }
    public void ExitState() { Debug.Log("Do something"); }
    public void HandleContext(PlayerInputData input) { Debug.Log($"Handling input: {input} in Example State"); }
}
