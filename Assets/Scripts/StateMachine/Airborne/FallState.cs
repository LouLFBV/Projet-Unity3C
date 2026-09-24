class FallState : AirboneState
{
    public FallState(PlayerCharacter character) : base(character) { }
    public override void Enter() 
    {
        base.Enter();
    }
    public override void Update() 
    {
        base.Update();
        if (character.IsGrounded)
        {
            character.PlayerStateMachine.ChangeState(PlayerStateType.Idle);
        }
    }
    public override void FixedUpdate() { }
    public override void Exit() { }
}