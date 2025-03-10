using System.Collections.Generic;
using UnityEngine;

public class FSMController
{
    private IState currentState;
    private Dictionary<System.Type, IState> states = new();

    public void RegisterState(IState state)
    {
        states[state.GetType()] = state;
    }

    public void SetState<T>() where T : IState
    {
        currentState?.OnExit();
        if (states.TryGetValue(typeof(T),out IState newState))
        {
            currentState = newState;
            currentState.OnEnter();
        }
        else
        {
            Debug.LogError($"State {typeof(T)} not registered.");
        }
    }
    public void OnUpdate()
    {
        currentState?.OnUpdate();
    }
}
