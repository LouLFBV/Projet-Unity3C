using System.Collections.Generic;
using System;
using UnityEngine;
/// <summary>
/// Manages a collection of <see cref="State"/> instances and controls
/// the active state through a stack-based state machine.
/// </summary>
public class StateMachine
{
    /// <summary>
    /// Gets the state currently at the top of the state stack.
    /// Returns the default value when the stack is empty.
    /// </summary>
    public State CurrentState => _stateStack.Count > 0 ? _stateStack.Peek() : default;
    /// <summary>
    /// Stores the registered states indexed by their concrete type.
    /// </summary>
    private Dictionary<Type, State> _states = new Dictionary<Type, State>();
    /// <summary>
    /// Stack containing the active states, with the current state at the top.
    /// </summary>
    private Stack<State> _stateStack = new Stack<State>();

    /// <summary>
    /// Registers a state in the state machine.
    /// </summary>
    /// <typeparam name="StateT">Type of the state to register.</typeparam>
    /// <param name="state">State instance to register.</param>
    /// <param name="start">
    /// Determines whether the state should immediately become the initial active state.
    /// </param>
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
    /// <summary>
    /// Processes the requested state transitions for the current state.
    /// Handles state popping, pushing, and direct transitions to the next state.
    /// </summary>
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
    /// <summary>
    /// Removes the requested number of states from the state stack,
    /// calling <see cref="State.Exit"/> and <see cref="State.Reset"/> for each removed state.
    /// </summary>
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
    /// <summary>
    /// Handles a requested state push by retrieving the target state
    /// from the registered states and adding it to the state stack.
    /// </summary>
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
    /// <summary>
    /// Handles a requested state transition by exiting the current state
    /// and activating the requested next state.
    /// </summary>
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

    /// <summary>
    /// Pushes a registered state of the specified type onto the state stack.
    /// </summary>
    /// <typeparam name="T">Type of the state to push.</typeparam>
    public void PushState<T>() where T : State => PushState(typeof(T));
    /// <summary>
    /// Pushes a registered state onto the state stack using its type.
    /// </summary>
    /// <param name="stateType">Type of the state to push.</param>
    public void PushState(Type stateType)
    {
        if (!_states.TryGetValue(stateType, out State newState)) return;
        if (newState == null) return;
        if (CurrentState == newState) return;
        _stateStack.Push(newState);
        newState.Enter();
    }
    /// <summary>
    /// Updates the current state and processes any requested state transitions.
    /// </summary>
    public void Update()
    {
        Debug.Log($"<color=blue>Current State: {CurrentState?.GetType().Name}</color>");
        CurrentState?.Update();

        HandleChange();
    }
    /// <summary>
    /// Performs the fixed-time update on the current state.
    /// </summary>
    public void FixedUpdate()
    {
        CurrentState?.FixedUpdate();
    }
}