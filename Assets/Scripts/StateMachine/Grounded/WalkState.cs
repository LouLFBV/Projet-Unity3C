class WalkState : GroundedState
{
    public WalkState(PlayerCharacter character) : base(character)    {    }
    public override void Enter()
    {
        base.Enter();
    }
    public override void Update()
    {
        base.Update();
        if (Character.IsSprinting)
        {
            SetNextState<RunState>();
        }
        if (Character.Body.Velocity.magnitude < 0.1f)
        {
            SetNextState<IdleState>();
        }
    }
    public override void FixedUpdate() { }
    public override void Exit() { }
}