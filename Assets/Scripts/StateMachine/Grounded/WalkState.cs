class WalkState : GroundedState
{
    public WalkState(PlayerCharacter character, PlayerStateMachine stateMachine) : base(character, stateMachine)
    {
        base.Enter();
    }
    public override void Enter() { }
    public override void Update()
    {
        base.Update();
        character.acceleration = character.moveInput.x != 0 ? character.groundAcceleration : character.groundDeceleration;
        if (character.isSprinting)
        {
            stateMachine.ChangeState(PlayerStateType.Run);
        }
        if (character.velocity.magnitude < 0.1f)
        {
            stateMachine.ChangeState(PlayerStateType.Idle);
        }
    }
    public override void FixedUpdate() { }
    public override void Exit() { }
}