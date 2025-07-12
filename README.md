# Finite State Machine Framework for Unity

A lightweight, modular finite state machine (FSM) system built for Unity.  
Designed to be reusable across projects, extendable, and editor-friendly.

---

## Features

- Reusable `StateMachine`, `State`, and `Transition` architecture
- ScriptableObject-driven transition definitions
- Plug-and-play FSM setup for any MonoBehaviour
- Editor support with visual transition mapping (optional)
- Includes samples for quick onboarding

## Samples

This package ships with three tiers of examples located in the `Samples` folder:
1. **Conditional FSM Sample** – introduces context–aware transitions and conditional logic.
2. **Editor Driven FSM Sample** – builds a state machine from ScriptableObject definitions and visual tools.

## Getting Started

### Creating a simple FSM

The Tier&nbsp;1 sample shows the bare‑bones approach. Define your states by implementing `IState<T>` and register transitions using `TransitionInfo`:

```csharp
// Example taken from the Tier 1 sample
var stateGreen = new ExampleStateGreen(SpriteRenderer);
var stateRed = new ExampleStateRed(SpriteRenderer);
var stateBlue = new ExampleStateBlue(SpriteRenderer);

var transitionInfo = new[]
{
    new TransitionInfo<PlayerInputData>(stateGreen, stateRed, input => input.IsKeyR),
    new TransitionInfo<PlayerInputData>(stateGreen, stateBlue, input => input.IsKeyB),

    new TransitionInfo<PlayerInputData>(stateRed, stateBlue, input => input.IsKeyB),
    new TransitionInfo<PlayerInputData>(stateRed, stateGreen, input => input.IsKeyG, toArgs: new object[] { this.gameObject }),

    new TransitionInfo<PlayerInputData>(stateBlue, stateGreen, input => input.IsKeyG),
    new TransitionInfo<PlayerInputData>(stateBlue, stateRed, input => input.IsKeyR),
};

_stateMachine = new StateMachine<PlayerInputData>(transitionInfo.ToList(), _inputData);
_stateMachine.InitializeStateMachine(stateGreen);

void Update()
{
    fsm.Tick();
    fsm.HandleContext(currentInput);
}
```

### Visualizer window

For editor‑driven setups (Tier&nbsp;3), open **Window&nbsp;→ FSMDebug&nbsp;→ Transition Map Visualizer** to inspect your `TransitionSet` assets.
