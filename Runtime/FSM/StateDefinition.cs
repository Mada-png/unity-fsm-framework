using System;
using UnityEngine;

/// <summary>
/// Base class for state definitions.
/// Use when you want to set up a state transition between states.
/// </summary>
/// <typeparam name="TFactory"></typeparam>
/// <typeparam name="TInput"></typeparam>
public abstract class StateDefinition<TFactory, TInput> : ScriptableObject, IStateDefinition<TFactory, TInput>
{
    public abstract Type StateType { get; }

    public abstract IState<TInput> CreateState(TFactory factory);
}

public interface IStateDefinition<TFactory, TInput>
{
    Type StateType { get; }
    IState<TInput> CreateState(TFactory factory);
}