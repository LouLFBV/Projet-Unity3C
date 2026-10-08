/// <summary>
/// Represents the walking state of the player while grounded.
/// Handles transitions to sprinting or idle when the player's movement state changes.
/// </summary>
class WalkState : GroundedState
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WalkState"/> class.
    /// </summary>
    /// <param name="character">Player character associated with this state.</param>
    public WalkState(PlayerCharacter character) : base(character)    { }
    /// <summary>
    /// Updates the walking state and evaluates whether a transition to another
    /// grounded movement state is required.
    /// </summary>
    public override void Update()
    {
        base.Update();
        if (Character.IsSprinting)
        {
            SetNextState<RunState>();
        }
        if (Character.Body.Velocity.magnitude < 0.1f)
        {
            SetNextState<IdleState>();
        }
    }
}