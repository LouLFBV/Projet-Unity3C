/// <summary>
/// State machine responsible for managing the states of a <see cref="PlayerCharacter"/>.
/// </summary>
public class PlayerStateMachine : StateMachine
{
    /// <summary>
    /// Gets the player character associated with this state machine.
    /// </summary>
    public PlayerCharacter PlayerCharacter => _character;
    /// <summary>
    /// Player character associated with this state machine.
    /// </summary>
    private PlayerCharacter _character;
    /// <summary>
    /// Initializes a new instance of the <see cref="PlayerStateMachine"/> class.
    /// </summary>
    /// <param name="character">Player character controlled by this state machine.</param>
    public PlayerStateMachine(PlayerCharacter character)
    {
        _character = character;
    }
}
