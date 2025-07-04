using System;
using Mada_PNG.FSM.Runtime;

public class ExampleState : IState<PlayerInputData>
{
    public void EnterState() { Console.WriteLine("Entering Example State"); }
    public void Tick() { Console.WriteLine("Ticking Example State"); }
    public void FixedTick() { Console.WriteLine("Fixed Ticking Example State"); }
    public void ExitState() { Console.WriteLine("Exiting Example State"); }
    public void HandleContext(PlayerInputData input) { Console.WriteLine($"Handling input: {input} in Example State"); }
}