class WalkState : GroundedState
{
    public WalkState(PlayerCharacter character) : base(character)
    {
        base.Enter();
    }
    public override void Enter() { }
    public override void Update()
    {
        base.Update();
        if (character.isSprinting)
        {
            character.PlayerStateMachine.ChangeState(PlayerStateType.Run);
        }
        if (character.velocity.magnitude < 0.1f)
        {
            character.PlayerStateMachine.ChangeState(PlayerStateType.Idle);
        }
    }
    public override void FixedUpdate() { }
    public override void Exit() { }
}