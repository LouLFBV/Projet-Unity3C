class RunState : GroundedState
{
    public RunState(PlayerCharacter character) : base(character) { }
    public override void Enter()
    {
        base.Enter();
    }
    public override void Update()
    {
        base.Update();
        Character.acceleration = Character.moveInput.x != 0 ? Character.sprintAcceleration : Character.sprintDeceleration;
        if (!Character.isSprinting)
        {
            SetNextState<WalkState>();
            //_stateMachine.ChangeState(PlayerStateType.Walk);
        }
    }
    public override void FixedUpdate() { }
    public override void Exit() { }
}