using System.Collections.Generic;
using System;
using UnityEngine;

public class StateMachine
{
    public State CurrentState => _stateStack.Count > 0 ? _stateStack.Peek() : default;
    private Dictionary<Type, State> _states = new Dictionary<Type, State>();
    private Stack<State> _stateStack = new Stack<State>();


    public void Register<StateT>(StateT state, bool start = false) where StateT : State
    {
        if (start)
        {
            _stateStack.Push(state);
            state.Enter();
        }
        if (state == null)
        {
            Debug.LogError("Cannot register a null state.");
            return;
        }
        Type stateType = typeof(StateT);
        if (!_states.ContainsKey(stateType))
        {
            _states.Add(stateType, state);
        }
        else
        {
            Debug.LogWarning($"State of type {stateType} already exists in the state machine.");
        }
    }

    //private void ChangeState(State newState)
    //{
    //    if (CurrentState != null)
    //    {
    //        State stateToExit = CurrentState;

    //        _stateStack.Pop();
    //        stateToExit.Exit();
    //        stateToExit.Reset(null); 
    //    }

    //    _stateStack.Push(newState);
    //    CurrentState.Enter();
    //}

    private void HandleChange()
    {
        if (CurrentState == null)
        {
            Debug.LogWarning("CurrentState is null. No state to handle change.");
            return;
        }


        if (CurrentState.Pop != 0)
        {
            HandlePop();
        }
        if (CurrentState.PushState != null)
        {
            HandlePush();
        }
        if (CurrentState.NextState != null)
        {
            HandleNext();
        }

    }

    private void HandlePop()
    {
        uint popCount = Math.Min(CurrentState.Pop, (uint)_stateStack.Count - 1);
        for (int i = 0; i < popCount; i++)
        {
            State stateToExit = _stateStack.Pop();
            stateToExit.Exit();
            stateToExit.Reset();
        }
    }

    private void HandlePush()
    {
        Type pushStateType = CurrentState.PushState;
        if (pushStateType == null) return;
        if (_states.TryGetValue(pushStateType, out State pushState))
        {
            CurrentState.Reset(ActionType.Push);
            PushState(pushStateType);
            //CurrentState.Enter();
        }
        else
        {
            Debug.LogError($"State of type {pushStateType} not found in the state machine.");
        }
    }

    private void HandleNext()
    {
        Type nextStateType = CurrentState.NextState;
        if (nextStateType == null) return;
        if (_states.TryGetValue(nextStateType, out State nextState))
        {
            CurrentState.Reset();
            CurrentState.Exit();
            _stateStack.Push(nextState);
            //_stateStack.Pop();
            CurrentState.Enter();
        }
        else
        {
            Debug.LogError($"State of type {nextStateType} not found in the state machine.");
        }
    }


    public void PushState<T>() where T : State => PushState(typeof(T));

    public void PushState(Type stateType)
    {
        if (!_states.TryGetValue(stateType, out State newState)) return;
        if (newState == null) return;
        if (CurrentState == newState) return;
        _stateStack.Push(newState);
        newState.Enter();
    }

    public void Update()
    {
        Debug.Log($"<color=blue>Current State: {CurrentState?.GetType().Name}</color>");
        CurrentState?.Update();

        HandleChange();
    }
    public void FixedUpdate()
    {
        CurrentState?.FixedUpdate();
    }
}