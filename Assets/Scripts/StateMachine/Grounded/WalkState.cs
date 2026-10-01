class WalkState : GroundedState
{
    public WalkState(PlayerCharacter character) : base(character)
    {
        base.Enter();
    }
    public override void Enter() {}
    public override void Update()
    {
        base.Update();
        Character.acceleration = Character.moveInput.x != 0 ? Character.groundAcceleration : Character.groundDeceleration;
        if (Character.isSprinting)
        {
            SetNextState<RunState>();
            //_stateMachine.ChangeState(PlayerStateType.Run);
        }
        if (Character.Body.Velocity.magnitude < 0.1f)
        {
            SetNextState<IdleState>();
            //_stateMachine.ChangeState(PlayerStateType.Idle);
        }
    }
    public override void FixedUpdate() { }
    public override void Exit() { }
}