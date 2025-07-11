# Finite State Machine Framework for Unity

A lightweight, modular finite state machine (FSM) system built for Unity.  
Designed to be reusable across projects, extendable, and editor-friendly.

---

## ✨ Features

- 🔁 Reusable `StateMachine`, `State`, and `Transition` architecture
- 🎯 ScriptableObject-driven transition definitions
- 🧱 Plug-and-play FSM setup for any MonoBehaviour
- 🛠️ Editor support with visual transition mapping (optional)
- 🧪 Includes samples for quick onboarding

## 📚 Samples

This package ships with three tiers of examples located in the `Samples` folder:

1. **Simple State Machine** – a minimal, code-driven FSM that demonstrates the core API.
2. **Conditional FSM Sample** – introduces context–aware transitions and conditional logic.
3. **Editor Driven FSM Sample** – builds a state machine from ScriptableObject definitions and visual tools.

## 🚀 Getting Started

### Creating a simple FSM

The Tier&nbsp;1 sample shows the bare‑bones approach. Define your states by implementing `IState<T>` and register transitions using `TransitionInfo`:

```csharp
// Example taken from the Tier 1 sample
var transitions = new[]
{
    TransitionInfo<PlayerInputData>.From<ExampleState, ExampleState2>(input => input.IsRunning),
    TransitionInfo<PlayerInputData>.From<ExampleState2, ExampleState>(input => !input.IsRunning),
    TransitionInfo<PlayerInputData>.From<ExampleState2, ExampleState3>(input => input.IsJumping),
};

var fsm = new StateMachine<PlayerInputData>(transitions.ToList(), new PlayerInputData());
fsm.InitializeStateMachine(new ExampleState());

void Update()
{
    fsm.Tick();
    fsm.HandleContext(currentInput);
}
```

### Visualizer window

For editor‑driven setups (Tier&nbsp;3), open **Window&nbsp;→ FSMDebug&nbsp;→ Transition Map Visualizer** to inspect your `TransitionSet` assets.
