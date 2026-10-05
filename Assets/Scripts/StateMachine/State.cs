using System;

public abstract class State
{
    public Type NextState { get; private set; } = null;
    public Type PushState { get; private set; } = null;
    public uint Pop { get; private set; } = 0;

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
    public virtual void Exit() { }
    public void Reset()
    {
        NextState = null;
        PushState = null;
        Pop = 0;
    }

    public void Reset(ActionType actionType)
    {
        switch (actionType)
        {
            case ActionType.Next:
                NextState = null;
                break;
            case ActionType.Push:
                PushState = null;
                break;
            case ActionType.Pop:
                Pop = 0;
                break;
        }
    }
    public void SetNextState<StateT>(bool force = false) where StateT : State
    {
        if (!force && NextState != null)
        {
            return;
        }
        NextState = typeof(StateT);
    }
    public void SetPushState<StateT>() where StateT : State
    {
        PushState = typeof(StateT);
    }

    public void SetPopState(uint newPop) 
    {
        Pop = newPop;
    }
}

public enum ActionType
{
    Next,
    Push,
    Pop
}