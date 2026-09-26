using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    private Stack<PlayerState> _stateStack = new Stack<PlayerState>();
    private Dictionary<PlayerStateType, PlayerState> _stateDictionary = new Dictionary<PlayerStateType, PlayerState>();
    public PlayerState CurrentState => _stateStack.Count > 0 ? _stateStack.Peek() : null;

    public void Initialized(Dictionary<PlayerStateType, PlayerState> newStateDictionary) => _stateDictionary = newStateDictionary;

    public void ChangeState(PlayerStateType newState)
    {
        if (CurrentState is TPState)
        {
            return;
        }
        if (CurrentState != null)
        {
            PlayerState stateToExit = CurrentState;

            _stateStack.Pop();
            stateToExit.Exit();
        }

        _stateStack.Push(_stateDictionary[newState]);
        CurrentState.Enter();
    }

    public void PushState(PlayerStateType newState)
    {
        _stateStack.Push(_stateDictionary[newState]);
        _stateDictionary[newState].Enter();
    }

    public void PopState()
    {
        if (CurrentState != null)
        {
            _stateStack.Pop();
        }
    }

    public void Update()
    {
        Debug.Log($"<color=blue>Current State: {CurrentState?.GetType().Name}</color>");
        CurrentState?.Update();
    }
    public void FixedUpdate()
    {
        CurrentState?.FixedUpdate();
    }
}

public enum PlayerStateType
{
    Idle,
    Walk,
    Run,
    Jump,
    Fall,
    TP,
    WallJump,
    VineSwing,
    Death,
    UI
}