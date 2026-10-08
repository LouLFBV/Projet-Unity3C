using System;

/// <summary>
/// Base class for states managed by a <see cref="StateMachine"/>.
/// Provides lifecycle methods and mechanisms for requesting state transitions.
/// </summary>
public abstract class State
{ 
    #region --- STATE TRANSITION REQUESTS ---

    /// <summary>
    /// Gets the type of the state to transition to.
    /// </summary>
    public Type NextState { get; private set; } = null;
    /// <summary>
    /// Gets the type of the state to push onto the state stack.
    /// </summary>
    public Type PushState { get; private set; } = null;
    /// <summary>
    /// Gets the number of states requested to be popped from the state stack.
    /// </summary>
    public uint Pop { get; private set; } = 0;
    #endregion

    #region --- STATE LIFECYCLE ---

    /// <summary>
    /// Called when the state becomes the current active state.
    /// </summary>
    public virtual void Enter() { }
    /// <summary>
    /// Called every frame while the state is active.
    /// </summary>
    public virtual void Update() { }
    /// <summary>
    /// Called at fixed time intervals while the state is active.
    /// </summary>
    public virtual void FixedUpdate() { }
    /// <summary>
    /// Called when the state is no longer the current active state.
    /// </summary>
    public virtual void Exit() { }
    #endregion

    #region --- TRANSITION MANAGEMENT ---

    /// <summary>
    /// Clears all pending state transition requests.
    /// </summary>
    public void Reset()
    {
        NextState = null;
        PushState = null;
        Pop = 0;
    }

    /// <summary>
    /// Clears the pending transition request corresponding to the specified action type.
    /// </summary>
    /// <param name="actionType">Type of transition request to clear.</param>
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
    /// <summary>
    /// Requests a transition to a state of the specified type.
    /// </summary>
    /// <typeparam name="StateT">Type of the state to transition to.</typeparam>
    /// <param name="force">
    /// If <c>true</c>, replaces an already pending next state.
    /// If <c>false</c>, the request is ignored when another next state is already set.
    /// </param>
    public void SetNextState<StateT>(bool force = false) where StateT : State
    {
        if (!force && NextState != null)
        {
            return;
        }
        NextState = typeof(StateT);
    }
    /// <summary>
    /// Requests the specified state type to be pushed onto the state stack.
    /// </summary>
    /// <typeparam name="StateT">Type of the state to push.</typeparam>
    public void SetPushState<StateT>() where StateT : State
    {
        PushState = typeof(StateT);
    }

    /// <summary>
    /// Requests one or more states to be popped from the state stack.
    /// </summary>
    /// <param name="newPop">Number of states to pop.</param>
    public void SetPopState(uint newPop) 
    {
        Pop = newPop;
    }
    #endregion
}
/// <summary>
/// Defines the types of state stack actions that can be requested.
/// </summary>
public enum ActionType
{
    #region --- ACTION TYPES ---

    /// <summary>
    /// Requests a transition to another state.
    /// </summary>
    Next,
    /// <summary>
    /// Requests a state to be pushed onto the state stack.
    /// </summary>
    Push,
    /// <summary>
    /// Requests one or more states to be removed from the state stack.
    /// </summary>
    Pop
    #endregion
}