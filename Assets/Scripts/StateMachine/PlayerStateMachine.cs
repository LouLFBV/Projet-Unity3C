public class PlayerStateMachine : StateMachine
{
    public PlayerCharacter PlayerCharacter => _character;
    private PlayerCharacter _character;

    public PlayerStateMachine(PlayerCharacter character)
    {
        _character = character;
    }
}
