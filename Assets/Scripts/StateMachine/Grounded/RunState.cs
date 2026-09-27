class RunState : GroundedState
{
    public RunState(PlayerCharacter character, PlayerStateMachine stateMachine) : base(character, stateMachine) { }
    public override void Enter()
    {
        base.Enter();
    }
    public override void Update()
    {
        base.Update();
        character.acceleration = character.moveInput.x != 0 ? character.sprintAcceleration : character.sprintDeceleration;
        if (!character.isSprinting)
        {
            stateMachine.ChangeState(PlayerStateType.Walk);
        }
    }
    public override void FixedUpdate() { }
    public override void Exit() { }
}