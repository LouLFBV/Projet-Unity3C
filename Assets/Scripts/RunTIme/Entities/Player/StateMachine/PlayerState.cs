/// <summary>
/// Base class for player-specific states managed by a <see cref="PlayerStateMachine"/>.
/// Provides access to the associated <see cref="PlayerCharacter"/>.
/// </summary>
public abstract class PlayerState : State
{
    /// <summary>
    /// Gets the player character associated with this state.
    /// </summary>
    protected PlayerCharacter Character => _character;
    /// <summary>
    /// Player character associated with this state.
    /// </summary>
    private PlayerCharacter _character;
    /// <summary>
    /// Initializes a new instance of the <see cref="PlayerState"/> class.
    /// </summary>
    /// <param name="character">Player character associated with this state.</param
    protected PlayerState(PlayerCharacter character)
    {
        _character = character;
    }
}