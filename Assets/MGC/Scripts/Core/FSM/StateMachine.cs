using System;
using System.Collections.Generic;

public class StateMachine<TStateType> where TStateType : Enum
{
    private Dictionary<TStateType, IState> states = new();
    private IState currentState;

    public TStateType CurrentStateType { get; private set; }

    public void AddState(TStateType key, IState state)
    {
        states[key] = state;
    }

    public void ChangeState(TStateType newState)
    {
        if (Equals(CurrentStateType, newState)) return;

        currentState?.Exit();
        CurrentStateType = newState;
        currentState = states[newState];
        currentState?.Enter();
    }

    public void Update()
    {
        currentState?.Update();
    }
}