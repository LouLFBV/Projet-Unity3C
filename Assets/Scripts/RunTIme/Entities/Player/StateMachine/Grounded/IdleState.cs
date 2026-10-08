using UnityEngine;
/// <summary>
/// Represents the idle state of the player while grounded.
/// Handles the transition to the walking state when horizontal movement is detected.
/// </summary>
class IdleState : GroundedState
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IdleState"/> class.
    /// </summary>
    /// <param name="character">Player character associated with this state.</param>
    public IdleState(PlayerCharacter character) : base(character) { }
    /// <summary>
    /// Updates the idle state and transitions to walking when the player starts moving.
    /// </summary>
    public override void Update() 
    {
        base.Update();
        if (Mathf.Abs(Character.Body.Velocity.x) > 0.1f)
        {
            SetNextState<WalkState>();
        }
    }
}