public abstract class PlayerState : State
{
    protected PlayerCharacter Character => _character;

    private PlayerCharacter _character;

    public PlayerState(PlayerCharacter character)
    {
        _character = character;
    }
}