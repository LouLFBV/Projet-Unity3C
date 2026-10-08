using UnityEngine;
/// <summary>
/// Represents the state in which the player is airborne and falling.
/// Handles falling gravity and transitions to grounded movement states upon landing.
/// </summary>
class FallState : AirboneState
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FallState"/> class.
    /// </summary>
    /// <param name="character">Player character associated with this state.</param>
    public FallState(PlayerCharacter character) : base(character) { }
    /// <summary>
    /// Initializes the falling state by applying the falling gravity.
    /// </summary>
    public override void Enter() 
    {
        base.Enter();
        Character.Body.SetGravity(true, Character.FallingGravity);
    }
    /// <summary>
    /// Updates the falling state and transitions to the appropriate grounded state
    /// when the player lands on a surface.
    /// </summary>
    public override void Update()
    {
        base.Update();

        if (Character.HitNormal.y > 0.5f)
        {
            Debug.Log("<color=green>FallState</color> - Grounded");

            if (Character.GetPlayerDirection() != 0)
            {
                SetNextState<WalkState>();
            }
            else
            {
                SetNextState<IdleState>();
            }
        }
    }
}