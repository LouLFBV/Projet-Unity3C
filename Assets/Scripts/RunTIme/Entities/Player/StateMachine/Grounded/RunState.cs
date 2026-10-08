/// <summary>
/// Represents the sprinting state of the player while grounded.
/// Handles the transition back to walking when sprinting is no longer active.
/// </summary>
class RunState : GroundedState
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RunState"/> class.
    /// </summary>
    /// <param name="character">Player character associated with this state.</param>
    public RunState(PlayerCharacter character) : base(character) { }
    /// <summary>
    /// Called when the player enters the running state.
    /// Triggers the character's sprint behavior.
    /// </summary>
    public override void Enter()
    {
        base.Enter();
        Character.TriggerSprint();
    }
    /// <summary>
    /// Updates the running state and transitions back to walking
    /// when the player is no longer sprinting.
    /// </summary>
    public override void Update()
    {
        base.Update();
        if (!Character.IsSprinting)
        {
            SetNextState<WalkState>();
        }
    }
}